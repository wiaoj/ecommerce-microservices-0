using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using BuildingBlocks.Abstractions.Messaging.Events;
using BuildingBlocks.Core.CQRS.Events.Internal;
using BuildingBlocks.Core.Reflection.Extensions;
using System.Reflection;

namespace BuildingBlocks.Core.CQRS.Events;
public static class EventsExtensions {
	public static IEnumerable<Type> GetHandledIntegrationEventTypes(this Assembly[] assemblies) {
		//List<Type> messageHandlerTypes = typeof(IIntegrationEventHandler<>).GetAllTypesImplementingOpenGenericInterface(assemblies).ToList();

		//IEnumerable<Type> inheritsTypes = messageHandlerTypes.SelectMany(x => x.GetInterfaces())
		//	.Where(x => x.GetInterfaces().Any(i => i.IsGenericType) && x.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>));

		//foreach(Type? inheritsType in inheritsTypes) {
		//	Type messageType = inheritsType.GetGenericArguments().First();
		//	if(messageType.IsAssignableTo(typeof(IIntegrationEvent))) {
		//		yield return messageType;
		//	}
		//}
		return GetHandledEventTypes(typeof(IIntegrationEventHandler<>), assemblies);
	}

	public static IEnumerable<Type> GetHandledDomainNotificationEventTypes(this Assembly[] assemblies) {


		//return default;
		//List<Type> messageHandlerTypes = typeof(IDomainNotificationEventHandler<>).GetAllTypesImplementingOpenGenericInterface(assemblies).ToList();

		//IEnumerable<Type> inheritsTypes = messageHandlerTypes.SelectMany(x => x.GetInterfaces())
		//	.Where(x => x.GetInterfaces().Any(i => i.IsGenericType) && x.GetGenericTypeDefinition() == typeof(IDomainNotificationEventHandler<>));

		//foreach(Type? inheritsType in inheritsTypes) {
		//	Type messageType = inheritsType.GetGenericArguments().First();
		//	if(messageType.IsAssignableTo(typeof(IDomainNotificationEvent))) {
		//		yield return messageType;
		//	}
		//}
		return GetHandledEventTypes(typeof(IDomainNotificationEventHandler<>), assemblies);
	}

	private static IEnumerable<Type> GetHandledEventTypes(Type messageHandlerType, Assembly[] assemblies) {
		List<Type> messageHandlerTypes = messageHandlerType.GetAllTypesImplementingOpenGenericInterface(assemblies).ToList();

		IEnumerable<Type> inheritsTypes = messageHandlerTypes.SelectMany(x => x.GetInterfaces())
			.Where(x => x.GetInterfaces()
			.Any(i => i.IsGenericType) && x.GetGenericTypeDefinition() == messageHandlerType);

		foreach(Type? inheritsType in inheritsTypes) {
			Type messageType = inheritsType.GetGenericArguments().First();
			if(messageType.IsAssignableTo(messageHandlerType)) {
				yield return messageType;
			}
		}
	}

	public static IEnumerable<Type> GetHandledDomainEventTypes(this Assembly[] assemblies) {
		//List<Type> messageHandlerTypes = typeof(IDomainEventHandler<>).GetAllTypesImplementingOpenGenericInterface(assemblies).ToList();

		//IEnumerable<Type> inheritsTypes = messageHandlerTypes.SelectMany(x => x.GetInterfaces())
		//	.Where(x => x.GetInterfaces().Any(i => i.IsGenericType) && x.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>));

		//foreach(Type? inheritsType in inheritsTypes) {
		//	Type messageType = inheritsType.GetGenericArguments().First();
		//	if(messageType.IsAssignableTo(typeof(IDomainEvent))) {
		//		yield return messageType;
		//	}
		//}
		return GetHandledEventTypes(typeof(IDomainEventHandler<>), assemblies);
	}

	public static IEnumerable<IDomainNotificationEvent> GetWrappedDomainNotificationEvents(this IEnumerable<IDomainEvent> domainEvents) {
		return GetWrappedEvents<IDomainNotificationEvent>(domainEvents, typeof(IHaveNotificationEvent), typeof(DomainNotificationEventWrapper<>));
		//foreach(IDomainEvent domainEvent in domainEvents.Where(x => typeof(IHaveNotificationEvent).IsAssignableFrom(x.GetType()))) {
		//	Type genericType = typeof(DomainNotificationEventWrapper<>).MakeGenericType(domainEvent.GetType());

		//	IDomainNotificationEvent? domainNotificationEvent = (IDomainNotificationEvent?)Activator.CreateInstance(genericType, domainEvent);

		//	if(domainNotificationEvent is not null) {
		//		yield return domainNotificationEvent;
		//	}
		//}
	}

	private static IEnumerable<Type> GetWrappedEvents<Type>(IEnumerable<IDomainEvent> events, System.Type eventType, System.Type wrapperEventType) {
		foreach(IDomainEvent @event in events.Where(x => eventType.IsAssignableFrom(x.GetType()))) {
			System.Type genericType = wrapperEventType.MakeGenericType(@event.GetType());

			Type? typeEvent = (Type?)Activator.CreateInstance(genericType, @event);

			if(typeEvent is null) {
				continue;
			}

			yield return typeEvent;
		}
	}

	public static IEnumerable<IIntegrationEvent> GetWrappedIntegrationEvents(this IEnumerable<IDomainEvent> domainEvents) {
		return GetWrappedEvents<IIntegrationEvent>(domainEvents, typeof(IHaveExternalEvent), typeof(IntegrationEventWrapper<>));
		//foreach(IDomainEvent domainEvent in domainEvents.Where(x => typeof(IHaveExternalEvent).IsAssignableFrom(x.GetType()))) {
		//	Type genericType = typeof(IntegrationEventWrapper<>).MakeGenericType(domainEvent.GetType());

		//	IIntegrationEvent? domainNotificationEvent = (IIntegrationEvent?)Activator.CreateInstance(genericType, domainEvent);

		//	if(domainNotificationEvent is not null) {
		//		yield return domainNotificationEvent;
		//	}
		//}
	}
}