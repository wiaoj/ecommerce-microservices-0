using BuildingBlocks.Abstractions.CQRS.Events.Internal;

namespace BuildingBlocks.Core.CQRS.Events.Internal;
public record DomainNotificationEventWrapper<TypeDomainEvent>(TypeDomainEvent DomainEvent) : DomainNotificationEvent
    where TypeDomainEvent : IDomainEvent;