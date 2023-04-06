using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Domain;

namespace BuildingBlocks.Abstractions.CQRS.Events;
public interface IAggregatesDomainEventsRequestStore {
	public IReadOnlyList<IDomainEvent> AddEventsFromAggregate<T>(T aggregate) where T : IHaveAggregate;
	public void AddEvents(IReadOnlyList<IDomainEvent> events);
	public IReadOnlyList<IDomainEvent> GetAllUncommittedEvents();
}