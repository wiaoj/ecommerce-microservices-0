using BuildingBlocks.Abstractions.CQRS.Events.Internal;

namespace BuildingBlocks.Abstractions.Persistence.EventStore.Projections;
public interface IHaveReadProjection {
	public Task ProjectAsync<Type>(IStreamEvent<Type> streamEvent, CancellationToken cancellationToken) where Type : IDomainEvent;
}