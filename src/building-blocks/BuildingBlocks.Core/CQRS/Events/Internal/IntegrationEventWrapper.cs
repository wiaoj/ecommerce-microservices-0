using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Core.Messaging;

namespace BuildingBlocks.Core.CQRS.Events.Internal;
public record IntegrationEventWrapper<TypeDomainEventType> : IntegrationEvent where TypeDomainEventType : IDomainEvent;