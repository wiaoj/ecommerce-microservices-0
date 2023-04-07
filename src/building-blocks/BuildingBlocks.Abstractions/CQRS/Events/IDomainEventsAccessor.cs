using BuildingBlocks.Abstractions.CQRS.Events.Internal;

namespace BuildingBlocks.Abstractions.CQRS.Events;
public interface IDomainEventsAccessor {
	public IReadOnlyList<IDomainEvent> UnCommittedDomainEvents { get; }
}