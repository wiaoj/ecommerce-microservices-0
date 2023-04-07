using MediatR;

namespace BuildingBlocks.Abstractions.CQRS.Events;
public interface IEventHandler<in TypeEvent> : INotificationHandler<TypeEvent> where TypeEvent : INotification { }