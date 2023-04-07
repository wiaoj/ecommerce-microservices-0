using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.Events;

namespace BuildingBlocks.Abstractions.CQRS.Events;
public interface IEventMapper : IIDomainNotificationEventMapper, IIntegrationEventMapper { }

public interface IIDomainNotificationEventMapper {
	public IReadOnlyList<IDomainNotificationEvent?>? MapToDomainNotificationEvents(IReadOnlyList<IDomainEvent> domainEvents);
	public IDomainNotificationEvent? MapToDomainNotificationEvent(IDomainEvent domainEvent);
}

public interface IIntegrationEventMapper {
	public IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents);
	public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent);
}