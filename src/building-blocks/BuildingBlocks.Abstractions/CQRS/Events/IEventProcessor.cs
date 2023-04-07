namespace BuildingBlocks.Abstractions.CQRS.Events;

/// <summary>
/// Internal Event Bus.
/// </summary>
public interface IEventProcessor {
	/// <summary>
	/// Send the events to outbox for saving and publishing to broker in the background (At-Least one Delivery).
	/// </summary>
	/// <typeparam name="TypeEvent">Type of event.</typeparam>
	/// <param name="event"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task PublishAsync<TypeEvent>(TypeEvent @event, CancellationToken cancellationToken)
		where TypeEvent : IEvent;

	/// <summary>
	/// Send the event to outbox for saving and publishing to broker in the background (At-Least one Delivery).
	/// </summary>
	/// <typeparam name="TypeEvent">Type of event.</typeparam>
	/// <param name="events"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task PublishAsync<TypeEvent>(TypeEvent[] events, CancellationToken cancellationToken)
		where TypeEvent : IEvent;

	/// <summary>
	/// Dispatch event internally to corresponding handler for executing.
	/// </summary>
	/// <typeparam name="TypeEvent">Type of event.</typeparam>
	/// <param name="event"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task DispatchAsync<TypeEvent>(TypeEvent @event, CancellationToken cancellationToken)
		where TypeEvent : IEvent;

	/// <summary>
	/// Dispatch events internally to corresponding handlers for executing.
	/// </summary>
	/// <typeparam name="TypeEvent">Type of event.</typeparam>
	/// <param name="events"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task DispatchAsync<TypeEvent>(TypeEvent[] events, CancellationToken cancellationToken)
		where TypeEvent : IEvent;
}