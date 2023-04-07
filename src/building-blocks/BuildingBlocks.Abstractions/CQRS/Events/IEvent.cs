using MediatR;

namespace BuildingBlocks.Abstractions.CQRS.Events;
/// <summary>
/// The event interface.
/// </summary>
public interface IEvent : INotification {
	/// <summary>
	/// Gets the event identifier.
	/// </summary>
	public Guid EventId { get; }

	/// <summary>
	/// Gets the event/aggregate root version.
	/// </summary>
	public Int64 EventVersion { get; }

	/// <summary>
	/// Gets the date the <see cref="IEvent"/> occurred on.
	/// </summary>
	public DateTime OccurredOn { get; }

	public DateTimeOffset TimeStamp { get; }

	/// <summary>
	/// Gets type of this event.
	/// </summary>
	public String EventType { get; }
}