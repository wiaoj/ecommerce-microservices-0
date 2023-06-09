using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;

namespace BuildingBlocks.Abstractions.Persistence.EventStore;
public interface IStreamEvent : IEvent {
	public IDomainEvent Data { get; }

	public IStreamEventMetadata? Metadata { get; }
}

public interface IStreamEvent<out Type> : IStreamEvent where Type : IDomainEvent {
	public new Type Data { get; }
}