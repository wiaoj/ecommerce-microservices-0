namespace BuildingBlocks.Abstractions.CQRS.Events.Internal;
public interface IDomainEventPublisher {
	public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken);
	public Task PublishAsync(IDomainEvent[] domainEvents, CancellationToken cancellationToken);
}