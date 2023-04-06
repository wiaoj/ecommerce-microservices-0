namespace BuildingBlocks.Abstractions.CQRS.Events.Internal;
public interface IDomainEventHandler<in TypeEvent> : IEventHandler<TypeEvent> where TypeEvent : IDomainEvent { }