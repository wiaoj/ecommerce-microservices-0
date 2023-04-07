using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.Events;
using BuildingBlocks.Abstractions.Messaging.Message;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;

namespace BuildingBlocks.Core.CQRS.Events;
public class DomainEventPublisher : IDomainEventPublisher {
	private readonly IEventProcessor eventProcessor;
	private readonly IMessagePersistenceService messagePersistenceService;
	private readonly IDomainEventsAccessor domainEventsAccessor;
	private readonly IDomainNotificationEventPublisher domainNotificationEventPublisher;
	private readonly IServiceProvider serviceProvider;

	public DomainEventPublisher(
		IEventProcessor eventProcessor,
		IMessagePersistenceService messagePersistenceService,
		IDomainNotificationEventPublisher domainNotificationEventPublisher,
		IDomainEventsAccessor domainEventsAccessor,
		IServiceProvider serviceProvider) {

		ArgumentNullException.ThrowIfNull(domainNotificationEventPublisher, nameof(domainNotificationEventPublisher));
		ArgumentNullException.ThrowIfNull(eventProcessor, nameof(eventProcessor));
		ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));
		this.messagePersistenceService = messagePersistenceService;
		this.domainEventsAccessor = domainEventsAccessor;

		this.domainNotificationEventPublisher = domainNotificationEventPublisher;
		this.eventProcessor = eventProcessor;
		this.serviceProvider = serviceProvider;
	}

	public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default) {
		return this.PublishAsync(new[] { domainEvent }, cancellationToken);
	}

	public async Task PublishAsync(IDomainEvent[] domainEvents, CancellationToken cancellationToken = default) {
		ArgumentNullException.ThrowIfNull(domainEvents, nameof(domainEvents));

		if(domainEvents.Any() is false) {
			return;
		}

		// https://github.com/dotnet-architecture/eShopOnContainers/issues/700#issuecomment-461807560
		// https://github.com/dotnet-architecture/eShopOnContainers/blob/e05a87658128106fef4e628ccb830bc89325d9da/src/Services/Ordering/Ordering.Infrastructure/OrderingContext.cs#L65
		// http://www.kamilgrzybek.com/design/how-to-publish-and-handle-domain-events/
		// http://www.kamilgrzybek.com/design/handling-domain-events-missing-part/
		// https://www.ledjonbehluli.com/posts/domain_to_integration_event/

		// Dispatch our domain events before commit
		IReadOnlyList<IDomainEvent> eventsToDispatch = domainEvents.ToList();

		if(eventsToDispatch.Any() is false) {
			eventsToDispatch = this.domainEventsAccessor.UnCommittedDomainEvents.ToImmutableList();
		}

		await this.eventProcessor.DispatchAsync(eventsToDispatch.ToArray(), cancellationToken);

		// Save wrapped integration and notification events to outbox for further processing after commit
		IDomainNotificationEvent[] wrappedNotificationEvents = eventsToDispatch.GetWrappedDomainNotificationEvents().ToArray();
		await this.domainNotificationEventPublisher.PublishAsync(wrappedNotificationEvents.ToArray(), cancellationToken);

		IIntegrationEvent[] wrappedIntegrationEvents = eventsToDispatch.GetWrappedIntegrationEvents().ToArray();
		foreach(IIntegrationEvent? wrappedIntegrationEvent in wrappedIntegrationEvents) {
			await this.messagePersistenceService.AddPublishMessageAsync(
				new MessageEnvelope(wrappedIntegrationEvent, new Dictionary<String, Object?>()),
				cancellationToken);
		}

		IReadOnlyList<IEventMapper> eventMappers = this.serviceProvider.GetServices<IEventMapper>().ToImmutableList();

		// Save event mapper events into outbox for further processing after commit
		IReadOnlyList<IIntegrationEvent> integrationEvents =
			GetIntegrationEvents(this.serviceProvider, eventMappers, eventsToDispatch);
		if(integrationEvents.Any()) {
			foreach(IIntegrationEvent integrationEvent in integrationEvents) {
				await this.messagePersistenceService.AddPublishMessageAsync(
					new MessageEnvelope(integrationEvent, new Dictionary<String, Object?>()),
					cancellationToken);
			}
		}

		IReadOnlyList<IDomainNotificationEvent> notificationEvents =
			this.GetNotificationEvents(this.serviceProvider, eventMappers, eventsToDispatch);

		if(notificationEvents.Any()) {
			foreach(IDomainNotificationEvent notification in notificationEvents) {
				await this.messagePersistenceService.AddNotificationAsync(notification, cancellationToken);
			}
		}
	}

	private IReadOnlyList<IDomainNotificationEvent> GetNotificationEvents(
		IServiceProvider serviceProvider,
		IReadOnlyList<IEventMapper> eventMappers,
		IReadOnlyList<IDomainEvent> eventsToDispatch) {
		IReadOnlyList<IIDomainNotificationEventMapper> notificationEventMappers =
			serviceProvider.GetServices<IIDomainNotificationEventMapper>().ToImmutableList();

		List<IDomainNotificationEvent> notificationEvents = new();

		if(eventMappers.Any()) {
			foreach(IEventMapper eventMapper in eventMappers) {
				List<IDomainNotificationEvent?>? items = eventMapper.MapToDomainNotificationEvents(eventsToDispatch)?.ToList();
				if(items is not null && items.Any()) {
					notificationEvents.AddRange(items.Where(x => x is not null)!);
				}
			}
		} else if(notificationEventMappers.Any()) {
			foreach(IIDomainNotificationEventMapper notificationEventMapper in notificationEventMappers) {
				List<IDomainNotificationEvent?>? items = notificationEventMapper.MapToDomainNotificationEvents(eventsToDispatch)?.ToList();
				if(items is not null && items.Any()) {
					notificationEvents.AddRange(items.Where(x => x is not null)!);
				}
			}
		}

		return notificationEvents.ToImmutableList();
	}

	private static IReadOnlyList<IIntegrationEvent> GetIntegrationEvents(
		IServiceProvider serviceProvider,
		IReadOnlyList<IEventMapper> eventMappers,
		IReadOnlyList<IDomainEvent> eventsToDispatch) {
		IReadOnlyList<IIntegrationEventMapper> integrationEventMappers =
			serviceProvider.GetServices<IIntegrationEventMapper>().ToImmutableList();

		List<IIntegrationEvent> integrationEvents = new();

		if(eventMappers.Any()) {
			foreach(IEventMapper eventMapper in eventMappers) {
				List<IIntegrationEvent?>? items = eventMapper.MapToIntegrationEvents(eventsToDispatch)?.ToList();
				if(items is not null && items.Any()) {
					integrationEvents.AddRange(items.Where(x => x is not null)!);
				}
			}
		} else if(integrationEventMappers.Any()) {
			foreach(IIntegrationEventMapper integrationEventMapper in integrationEventMappers) {
				List<IIntegrationEvent?>? items = integrationEventMapper.MapToIntegrationEvents(eventsToDispatch)?.ToList();
				if(items is not null && items.Any()) {
					integrationEvents.AddRange(items.Where(x => x is not null)!);
				}
			}
		}

		return integrationEvents.ToImmutableList();
	}
}