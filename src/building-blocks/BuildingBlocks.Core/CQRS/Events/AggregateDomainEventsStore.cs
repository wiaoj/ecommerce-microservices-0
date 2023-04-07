using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Domain;

namespace BuildingBlocks.Core.CQRS.Events;
public class AggregatesDomainEventsStore : IAggregatesDomainEventsRequestStore {
	private readonly List<IDomainEvent> uncommittedDomainEvents = new();

	public IReadOnlyList<IDomainEvent> AddEventsFromAggregate<Type>(Type aggregate) where Type : IHaveAggregate {
		IReadOnlyList<IDomainEvent> events = aggregate.GetUncommittedDomainEvents();

		this.AddEvents(events);

		return events;
	}

	public void AddEvents(IReadOnlyList<IDomainEvent> events) {
		if(events.Any()) {
			this.uncommittedDomainEvents.AddRange(events);
		}
	}

	public IReadOnlyList<IDomainEvent> GetAllUncommittedEvents() {
		return this.uncommittedDomainEvents;
	}
}