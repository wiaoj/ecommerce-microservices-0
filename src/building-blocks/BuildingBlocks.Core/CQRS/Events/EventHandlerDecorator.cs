using BuildingBlocks.Abstractions.CQRS.Events;

namespace BuildingBlocks.Core.CQRS.Events;
public class EventHandlerDecorator<TEvent> : IEventHandler<TEvent> where TEvent : IEvent {
	private readonly IEventHandler<TEvent> eventHandler;

	public EventHandlerDecorator(IEventHandler<TEvent> eventHandler) {
		this.eventHandler = eventHandler;
	}

	public async Task Handle(TEvent notification, CancellationToken cancellationToken) {
		// TODO: Using Activity for tracing

		await this.eventHandler.Handle(notification, cancellationToken);
	}
}