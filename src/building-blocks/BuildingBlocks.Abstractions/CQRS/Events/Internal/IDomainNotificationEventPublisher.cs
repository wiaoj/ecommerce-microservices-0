namespace BuildingBlocks.Abstractions.CQRS.Events.Internal;
public interface IDomainNotificationEventPublisher {
	public Task PublishAsync(IDomainNotificationEvent domainNotificationEvent, CancellationToken cancellationToken);
	public Task PublishAsync(IDomainNotificationEvent[] domainNotificationEvents, CancellationToken cancellationToken);
}