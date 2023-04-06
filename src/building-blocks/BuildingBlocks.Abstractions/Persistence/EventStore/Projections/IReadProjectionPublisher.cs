using BuildingBlocks.Abstractions.CQRS.Events.Internal;

namespace BuildingBlocks.Abstractions.Persistence.EventStore.Projections;
public interface IReadProjectionPublisher {
	public Task PublishAsync(IStreamEvent streamEvent, CancellationToken cancellationToken);
	public Task PublishAsync<Type>(IStreamEvent<Type> streamEvent, CancellationToken cancellationToken) where Type : IDomainEvent;
}