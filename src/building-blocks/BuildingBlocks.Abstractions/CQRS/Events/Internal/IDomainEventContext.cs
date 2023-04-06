namespace BuildingBlocks.Abstractions.CQRS.Events.Internal;
public interface IDomainEventContext {
	public IReadOnlyList<IDomainEvent> GetAllUncommittedEvents();
	public void MarkUncommittedDomainEventAsCommitted();
}