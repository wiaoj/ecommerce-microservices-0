using BuildingBlocks.Abstractions.CQRS.Events.Internal;

namespace BuildingBlocks.Core.CQRS.Events.Internal;
public abstract record DomainEvent : Event, IDomainEvent {
	public dynamic AggregateId { get; protected set; } = null!;
	public Int64 AggregateSequenceNumber { get; protected set; }

	public virtual IDomainEvent WithAggregate(dynamic aggregateId, Int64 version) {
		this.AggregateId = aggregateId;
		this.AggregateSequenceNumber = version;

		return this;
	}
}