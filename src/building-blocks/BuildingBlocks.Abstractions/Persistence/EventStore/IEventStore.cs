using BuildingBlocks.Abstractions.Domain.EventSourcing;

namespace BuildingBlocks.Abstractions.Persistence.EventStore;
public interface IEventStore {
	/// <summary>
	/// Check if specific stream exists in the store
	/// </summary>
	/// <param name="streamId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<Boolean> StreamExists(String streamId, CancellationToken cancellationToken);

	/// <summary>
	/// Gets events for an specific stream.
	/// </summary>
	/// <param name="streamId">Id of our aggregate or stream.</param>
	/// <param name="fromVersion">All events after this should be returned.</param>
	/// <param name="maxCount">Number of items to read.</param>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>public Task with events for aggregate.</returns>
	public Task<IEnumerable<IStreamEvent>> GetStreamEventsAsync(
		String streamId,
		StreamReadPosition? fromVersion,
		Int32 maxCount,
		CancellationToken cancellationToken);

	/// <summary>
	/// Gets events for an specific stream.
	/// </summary>
	/// <param name="streamId">Id of our aggregate or stream.</param>
	/// <param name="fromVersion">All events after this should be returned.</param>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>public Task with events for aggregate.</returns>
	public Task<IEnumerable<IStreamEvent>> GetStreamEventsAsync(
		String streamId,
		StreamReadPosition? fromVersion,
		CancellationToken cancellationToken);

	/// <summary>
	/// Append event to aggregate with no stream.
	/// </summary>
	/// <param name="streamId">Id of our aggregate or stream.</param>
	/// <param name="event">domain event to append the aggregate.</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<AppendResult> AppendEventAsync(
		String streamId,
		IStreamEvent @event,
		CancellationToken cancellationToken);

	/// <summary>
	/// Append event to aggregate with a existing or none existing stream.
	/// </summary>
	/// <param name="streamId">Id of our aggregate or stream.</param>
	/// <param name="event">domain event to append the aggregate.</param>
	/// <param name="expectedRevision"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<AppendResult> AppendEventAsync(
		String streamId,
		IStreamEvent @event,
		ExpectedStreamVersion expectedRevision,
		CancellationToken cancellationToken);

	/// <summary>
	/// Append events to aggregate with a existing or none existing stream.
	/// </summary>
	/// <param name="streamId">Id of our aggregate or stream.</param>
	/// <param name="events">domain event to append the aggregate.</param>
	/// <param name="expectedRevision"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task<AppendResult> AppendEventsAsync(
		String streamId,
		IReadOnlyCollection<IStreamEvent> events,
		ExpectedStreamVersion expectedRevision,
		CancellationToken cancellationToken);

	/// <summary>
	/// Rehydrating aggregate from events in the event store.
	/// </summary>
	/// <param name="streamId"></param>
	/// <param name="fromVersion"></param>
	/// <param name="defaultAggregateState">Initial state of the aggregate.</param>
	/// <param name="fold"></param>
	/// <param name="cancellationToken"></param>
	/// <typeparam name="TAggregate"></typeparam>
	/// <typeparam name="TId"></typeparam>
	/// <returns></returns>
	public Task<TAggregate?> AggregateStreamAsync<TAggregate, TId>(
		String streamId,
		StreamReadPosition fromVersion,
		TAggregate defaultAggregateState,
		Action<Object> fold,
		CancellationToken cancellationToken)
		where TAggregate : class, IEventSourcedAggregate<TId>, new();

	/// <summary>
	///  Rehydrating aggregate from events in the event store.
	/// </summary>
	/// <param name="streamId"></param>
	/// <param name="defaultAggregateState">Initial state of the aggregate.</param>
	/// <param name="fold"></param>
	/// <param name="cancellationToken"></param>
	/// <typeparam name="TAggregate"></typeparam>
	/// <typeparam name="TId"></typeparam>
	/// <returns></returns>
	public Task<TAggregate?> AggregateStreamAsync<TAggregate, TId>(
		String streamId,
		TAggregate defaultAggregateState,
		Action<Object> fold,
		CancellationToken cancellationToken)
		where TAggregate : class, IEventSourcedAggregate<TId>, new();

	/// <summary>
	/// Commit events to the event store.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public Task CommitAsync(CancellationToken cancellationToken);
}
