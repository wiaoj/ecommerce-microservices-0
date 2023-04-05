namespace BuildingBlocks.Abstractions.CQRS.Events.Internal;
public interface IDomainNotificationEvent<TDomainEventType> : IDomainNotificationEvent where TDomainEventType : IDomainEvent {
	public TDomainEventType DomainEvent { get; set; }
}

public interface IDomainNotificationEvent : IEvent { }