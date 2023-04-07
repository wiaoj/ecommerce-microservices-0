using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.Bus;
using BuildingBlocks.Abstractions.Messaging.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BuildingBlocks.Core.CQRS.Events;
public class EventProcessor : IEventProcessor {
	private readonly IPublisher publisher;
	private readonly IServiceProvider serviceProvider;

	public EventProcessor(IPublisher publisher, IServiceProvider serviceProvider) {
		this.publisher = publisher;
		this.serviceProvider = serviceProvider;
	}

	public async Task PublishAsync<TypeEvent>(TypeEvent @event, CancellationToken cancellationToken) where TypeEvent : IEvent {
		IDomainEventPublisher domainEventPublisher = this.serviceProvider.GetRequiredService<IDomainEventPublisher>();
		IDomainNotificationEventPublisher domainNotificationEventPublisher = this.serviceProvider.GetRequiredService<IDomainNotificationEventPublisher>();
		IBus integrationEventPublisher = this.serviceProvider.GetRequiredService<IBus>();

		if(@event is IIntegrationEvent integrationEvent) {
			await integrationEventPublisher.PublishAsync(integrationEvent, null, cancellationToken: cancellationToken);

			return;
		}

		if(@event is IDomainEvent domainEvent) {
			await domainEventPublisher.PublishAsync(domainEvent, cancellationToken);

			return;
		}

		if(@event is IDomainNotificationEvent notificationEvent) {
			await domainNotificationEventPublisher.PublishAsync(notificationEvent, cancellationToken);
		}
	}

	public async Task PublishAsync<TypeEvent>(TypeEvent[] events, CancellationToken cancellationToken) where TypeEvent : IEvent {
		foreach(TypeEvent @event in events) {
			await this.PublishAsync(@event, cancellationToken);
		}
	}

	public async Task DispatchAsync<TypeEvent>(TypeEvent @event, CancellationToken cancellationToken) where TypeEvent : IEvent {
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		if(@event is IIntegrationEvent integrationEvent) {
			await this.publisher.Publish(integrationEvent, cancellationToken);

			Log.Logger.Debug(
				"Dispatched integration notification event {IntegrationEventName} with payload {IntegrationEventContent}",
				integrationEvent.GetType().FullName,
				integrationEvent);

			return;
		}

		if(@event is IDomainEvent domainEvent) {
			await this.publisher.Publish(domainEvent, cancellationToken);

			Log.Logger.Debug(
				"Dispatched domain event {DomainEventName} with payload {DomainEventContent}",
				domainEvent.GetType().FullName,
				domainEvent);

			return;
		}

		if(@event is IDomainNotificationEvent notificationEvent) {
			await this.publisher.Publish(notificationEvent, cancellationToken);

			Log.Logger.Debug(
				"Dispatched domain notification event {DomainNotificationEventName} with payload {DomainNotificationEventContent}",
				notificationEvent.GetType().FullName,
				notificationEvent);
			return;
		}

		await this.publisher.Publish(@event, cancellationToken);
	}

	public async Task DispatchAsync<TypeEvent>(TypeEvent[] events, CancellationToken cancellationToken) where TypeEvent : IEvent {
		foreach(TypeEvent @event in events) {
			await this.DispatchAsync(@event, cancellationToken);
		}
	}
}