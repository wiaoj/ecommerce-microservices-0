using BuildingBlocks.Abstractions.Messaging.Events;

namespace BuildingBlocks.Core.Messaging;
public record IntegrationEvent : Message, IIntegrationEvent;