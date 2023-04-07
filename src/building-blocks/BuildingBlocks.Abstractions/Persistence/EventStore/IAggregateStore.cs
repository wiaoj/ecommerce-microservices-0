using BuildingBlocks.Abstractions.Domain.EventSourcing;

namespace BuildingBlocks.Abstractions.Persistence.EventStore;
/// <summary>
/// This AggregateStore act like a repository for the AggregateRoot.
/// </summary>
public interface IAggregateStore {
	/// <summary>
	/// Load the aggregate from the store with a aggregate id
	/// </summary>
	/// <typeparam name="TypeAggregate">Type of aggregate.</typeparam>
	/// <typeparam name="TypeId">Type of Id.</typeparam>
	/// <param name="aggregateId">Id of aggregate.</param>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>Task with aggregate as result.</returns>
	public Task<TypeAggregate?> GetAsync<TypeAggregate, TypeId>(TypeId aggregateId, CancellationToken cancellationToken)
		where TypeAggregate : class, IEventSourcedAggregate<TypeId>, new();

	/// <summary>
	/// Store an aggregate state to the store with using some events (use for updating, adding and deleting).
	/// </summary>
	/// <typeparam name="TypeAggregate">Type of aggregate.</typeparam>
	/// <typeparam name="TypeId">Type of Id.</typeparam>
	/// <param name="aggregate">Aggregate object to be saved.</param>
	/// <param name="expectedVersion">Expected version saved from earlier. -1 if new.</param>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>Task of operation.</returns>
	public Task<AppendResult> StoreAsync<TypeAggregate, TypeId>(TypeAggregate aggregate,
															 ExpectedStreamVersion? expectedVersion = null,
															 CancellationToken cancellationToken = default)
		where TypeAggregate : class, IEventSourcedAggregate<TypeId>, new();

	/// <summary>
	/// Store an aggregate state to the store with using some events (use for updating, adding and deleting).
	/// </summary>
	/// <typeparam name="TypeAggregate">Type of aggregate.</typeparam>
	/// <typeparam name="TypeId">Type of Id.</typeparam>
	/// <param name="aggregate">Aggregate object to be saved.</param>
	/// <param name="cancellationToken">Optional cancellation token.</param>
	/// <returns>Task of operation.</returns>
	public Task<AppendResult> StoreAsync<TypeAggregate, TypeId>(TypeAggregate aggregate, CancellationToken cancellationToken)
		where TypeAggregate : class, IEventSourcedAggregate<TypeId>, new();

	/// <summary>
	/// Check if aggregate exists in the store.
	/// </summary>
	/// <param name="aggregateId"></param>
	/// <param name="cancellationToken"></param>
	/// <typeparam name="TypeAggregate"></typeparam>
	/// <typeparam name="TypeId"></typeparam>
	/// <returns></returns>
	public Task<bool> Exists<TypeAggregate, TypeId>(TypeId aggregateId, CancellationToken cancellationToken)
		where TypeAggregate : class, IEventSourcedAggregate<TypeId>, new();
}