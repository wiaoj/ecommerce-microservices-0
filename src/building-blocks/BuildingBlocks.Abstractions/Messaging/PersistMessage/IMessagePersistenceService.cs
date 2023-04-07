using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.Message;
using System.Linq.Expressions;

namespace BuildingBlocks.Abstractions.Messaging.PersistMessage;

// Ref: http://www.kamilgrzybek.com/design/the-outbox-pattern/
// Ref: https://event-driven.io/en/outbox_inbox_patterns_and_delivery_guarantees_explained/
// Ref: https://debezium.io/blog/2019/02/19/reliable-microservices-data-exchange-with-the-outbox-pattern/
// Ref: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/subscribe-events#designing-atomicity-and-resiliency-when-publishing-to-the-event-bus
// Ref: https://github.com/kgrzybek/modular-monolith-with-ddd#38-internal-processing
public interface IMessagePersistenceService {
	public Task<IReadOnlyList<StoreMessage>> GetByFilterAsync(
		Expression<Func<StoreMessage, Boolean>>? predicate,
		CancellationToken cancellationToken);

	public Task AddPublishMessageAsync<TypeMessageEnvelope>(
		TypeMessageEnvelope messageEnvelope,
		CancellationToken cancellationToken) where TypeMessageEnvelope : MessageEnvelope;

	public Task AddReceivedMessageAsync<TypeMessageEnvelope>(
		TypeMessageEnvelope messageEnvelope,
		CancellationToken cancellationToken) where TypeMessageEnvelope : MessageEnvelope;

	public Task AddInternalMessageAsync<TypeCommand>(
		TypeCommand internalCommand,
		CancellationToken cancellationToken) where TypeCommand : class, IInternalCommand;

	public Task AddNotificationAsync(
		IDomainNotificationEvent notification,
		CancellationToken cancellationToken);

	public Task ProcessAsync(Guid messageId, MessageDeliveryType deliveryType, CancellationToken cancellationToken);

	public Task ProcessAllAsync(CancellationToken cancellationToken);
}