using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Domain;
using BuildingBlocks.Core.Domain.Exceptions;
using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace BuildingBlocks.Core.Domain;
public abstract class Aggregate<TypeId> : Entity<TypeId>, IAggregate<TypeId> {
	[NonSerialized]
	private readonly ConcurrentQueue<IDomainEvent> uncommittedDomainEvents = new();
	private const Int64 NEW_AGGREGATE_VERSION = default;

	public Int64 OriginalVersion => NEW_AGGREGATE_VERSION;

	public void CheckRule(IBusinessRule rule) {
		if(rule.IsBroken) {
			throw new BusinessRuleValidationException(rule);
		}
	}

	protected void AddDomainEvents(IDomainEvent domainEvent) {
		Boolean isDomainEventAdded = this.uncommittedDomainEvents.Any(x => Equals(x.EventId, domainEvent.EventId));
		if(isDomainEventAdded is false) {
			this.uncommittedDomainEvents.Enqueue(domainEvent);
		}
	}

	public IReadOnlyList<IDomainEvent> DequeueUncommittedDomainEvents() {
		IReadOnlyList<IDomainEvent> events = this.GetUncommittedDomainEvents();
		this.MarkUncommittedDomainEventAsCommitted();
		return events;
	}

	public IReadOnlyList<IDomainEvent> GetUncommittedDomainEvents() {
		return this.uncommittedDomainEvents.ToImmutableList();
	}

	public Boolean HasUncommittedDomainEvents() {
		return this.uncommittedDomainEvents.Any();
	}

	public void MarkUncommittedDomainEventAsCommitted() {
		this.uncommittedDomainEvents.Clear();
	}
}