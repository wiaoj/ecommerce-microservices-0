using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;

namespace BuildingBlocks.Core.CQRS.Events;
public class DomainNotificationEventPublisher : IDomainNotificationEventPublisher {
	private readonly IMessagePersistenceService messagePersistenceService;

	public DomainNotificationEventPublisher(IMessagePersistenceService messagePersistenceService) {
		this.messagePersistenceService = messagePersistenceService;
	}

	public Task PublishAsync(IDomainNotificationEvent domainNotificationEvent, CancellationToken cancellationToken) {
		ArgumentNullException.ThrowIfNull(domainNotificationEvent, nameof(domainNotificationEvent));

		return this.messagePersistenceService.AddNotificationAsync(domainNotificationEvent, cancellationToken);
	}

	public async Task PublishAsync(IDomainNotificationEvent[] domainNotificationEvents, CancellationToken cancellationToken) {
		ArgumentNullException.ThrowIfNull(domainNotificationEvents, nameof(domainNotificationEvents));

		foreach(IDomainNotificationEvent domainNotificationEvent in domainNotificationEvents) {
			await this.messagePersistenceService.AddNotificationAsync(domainNotificationEvent, cancellationToken);
		}
	}
}