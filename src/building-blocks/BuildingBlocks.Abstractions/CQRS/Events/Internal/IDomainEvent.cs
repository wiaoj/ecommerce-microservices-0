namespace BuildingBlocks.Abstractions.CQRS.Events.Internal;
public interface IDomainEvent : IEvent {
	/// <summary>
	/// Gets the identifier of the aggregate which has generated the event.
	/// </summary>
	public dynamic AggregateId { get; }
	public Int64 AggregateSequenceNumber { get; }
	public IDomainEvent WithAggregate(dynamic aggregateId, Int64 version);
}