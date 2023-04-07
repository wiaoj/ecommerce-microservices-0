using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.Bus;
using BuildingBlocks.Abstractions.Messaging.Message;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Abstractions.Serialization;
using BuildingBlocks.Core.Types;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace BuildingBlocks.Core.Messaging.MessagePersistence;
public class MessagePersistenceService : IMessagePersistenceService {
	private readonly ILogger<MessagePersistenceService> logger;
	private readonly IMessagePersistenceRepository messagePersistenceRepository;
	private readonly IMessageSerializer messageSerializer;
	private readonly IMediator mediator;
	private readonly IBus bus;
	private readonly ISerializer serializer;

	public MessagePersistenceService(
		ILogger<MessagePersistenceService> logger,
		IMessagePersistenceRepository messagePersistenceRepository,
		IMessageSerializer messageSerializer,
		IMediator mediator,
		IBus bus,
		ISerializer serializer
	) {
		this.logger = logger;
		this.messagePersistenceRepository = messagePersistenceRepository;
		this.messageSerializer = messageSerializer;
		this.mediator = mediator;
		this.bus = bus;
		this.serializer = serializer;
	}

	public Task<IReadOnlyList<StoreMessage>> GetByFilterAsync(
		Expression<Func<StoreMessage, Boolean>>? predicate,
		CancellationToken cancellationToken
	) {
		return this.messagePersistenceRepository.GetByFilterAsync(predicate ?? (_ => true), cancellationToken);
	}

	public async Task AddPublishMessageAsync<TypeMessageEnvelope>(
		TypeMessageEnvelope messageEnvelope,
		CancellationToken cancellationToken
	)
		where TypeMessageEnvelope : MessageEnvelope {
		await this.AddMessageCore(messageEnvelope, MessageDeliveryType.Outbox, cancellationToken);
	}

	public async Task AddReceivedMessageAsync<TypeMessageEnvelope>(
		TypeMessageEnvelope messageEnvelope,
		CancellationToken cancellationToken
	)
		where TypeMessageEnvelope : MessageEnvelope {
		await this.AddMessageCore(messageEnvelope, MessageDeliveryType.Inbox, cancellationToken);
	}

	public async Task AddInternalMessageAsync<TypeCommand>(
		TypeCommand internalCommand,
		CancellationToken cancellationToken
	)
		where TypeCommand : class, IInternalCommand {
		await this.AddMessageCore(new MessageEnvelope(internalCommand), MessageDeliveryType.Internal, cancellationToken);
	}

	public async Task AddNotificationAsync(
		IDomainNotificationEvent notification,
		CancellationToken cancellationToken
	) {
		await this.messagePersistenceRepository.AddAsync(
			new StoreMessage(
				notification.EventId,
				TypeMapper.GetFullTypeName(notification.GetType()), // same process so we use full type name
				this.serializer.Serialize(notification),
				MessageDeliveryType.Internal
			),
			cancellationToken
		);
	}

	private async Task AddMessageCore(
		MessageEnvelope messageEnvelope,
		MessageDeliveryType deliveryType,
		CancellationToken cancellationToken
	) {
		ArgumentNullException.ThrowIfNull(messageEnvelope.Message, nameof(messageEnvelope.Message));

		Guid id = messageEnvelope.Message is IMessage message
			? message.MessageId
			: messageEnvelope.Message is IInternalCommand command ? command.InternalCommandId : Guid.NewGuid();

		await this.messagePersistenceRepository.AddAsync(
			new StoreMessage(
				id,
				TypeMapper.GetFullTypeName(messageEnvelope.Message.GetType()), // because each service has its own persistence and same process (outbox,inbox), full name message type but in microservices we should just use type name for message
				this.messageSerializer.Serialize(messageEnvelope),
				deliveryType
			),
			cancellationToken
		);

		this.logger.LogInformation(
			"Message with id: {MessageID} and delivery type: {DeliveryType} saved in persistence message store",
			id,
			deliveryType.ToString()
		);
	}

	public async Task ProcessAsync(Guid messageId, CancellationToken cancellationToken) {
		StoreMessage? message = await this.messagePersistenceRepository.GetByIdAsync(messageId, cancellationToken);

		if(message is null) {
			return;
		}

		switch(message.DeliveryType) {
			case MessageDeliveryType.Inbox:
				await this.ProcessInbox(message, cancellationToken);
				break;
			case MessageDeliveryType.Internal:
				await this.ProcessInternal(message, cancellationToken);
				break;
			case MessageDeliveryType.Outbox:
				await this.ProcessOutbox(message, cancellationToken);
				break;
		}

		await this.messagePersistenceRepository.ChangeStateAsync(message.Id, MessageStatus.Processed, cancellationToken);
	}

	public async Task ProcessAllAsync(CancellationToken cancellationToken) {
		IReadOnlyList<StoreMessage> messages = await this.messagePersistenceRepository.GetByFilterAsync(
			x => x.MessageStatus != MessageStatus.Processed,
			cancellationToken
		);

		foreach(StoreMessage message in messages) {
			await this.ProcessAsync(message.Id, cancellationToken);
		}
	}

	private async Task ProcessOutbox(StoreMessage message, CancellationToken cancellationToken) {
		MessageEnvelope? messageEnvelope = this.messageSerializer.Deserialize<MessageEnvelope>(message.Data, true);

		if(messageEnvelope is null || messageEnvelope.Message is null) {
			return;
		}

		Object? data = this.messageSerializer.Deserialize(
			messageEnvelope.Message.ToString()!,
			TypeMapper.GetType(message.DataType)
		);

		if(data is IMessage) {
			// we should pass a object type message or explicit our message type, not cast to IMessage (data is IMessage integrationEvent) because masstransit doesn't work with IMessage cast.
			await this.bus.PublishAsync(data, messageEnvelope.Headers, cancellationToken);

			this.logger.LogInformation(
				"Message with id: {MessageId} and delivery type: {DeliveryType} processed from the persistence message store",
				message.Id,
				message.DeliveryType
			);
		}
	}

	private async Task ProcessInternal(StoreMessage message, CancellationToken cancellationToken) {
		MessageEnvelope? messageEnvelope = this.messageSerializer.Deserialize<MessageEnvelope>(message.Data, true);

		if(messageEnvelope is null || messageEnvelope.Message is null) {
			return;
		}

		Object? data = this.messageSerializer.Deserialize(
			messageEnvelope.Message.ToString()!,
			TypeMapper.GetType(message.DataType)
		);

		if(data is IDomainNotificationEvent domainNotificationEvent) {
			await this.mediator.Publish(domainNotificationEvent, cancellationToken);

			this.logger.LogInformation(
				"Domain-Notification with id: {EventID} and delivery type: {DeliveryType} processed from the persistence message store",
				message.Id,
				message.DeliveryType
			);
		}

		if(data is IInternalCommand internalCommand) {
			await this.mediator.Send(internalCommand, cancellationToken);

			this.logger.LogInformation(
				"InternalCommand with id: {EventID} and delivery type: {DeliveryType} processed from the persistence message store",
				message.Id,
				message.DeliveryType
			);
		}
	}

	private Task ProcessInbox(StoreMessage message, CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task ProcessAsync(Guid messageId, MessageDeliveryType deliveryType, CancellationToken cancellationToken) {
		throw new NotImplementedException();
	}
}