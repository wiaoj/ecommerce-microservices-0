using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Core.Types;

namespace BuildingBlocks.Core.CQRS.Events;
public abstract record Event : IEvent {
	public Guid EventId { get; protected set; } = Guid.NewGuid();

	public Int64 EventVersion { get; protected set; } = -1;

	public DateTime OccurredOn { get; protected set; } = DateTime.Now;

	public DateTimeOffset TimeStamp { get; protected set; } = DateTimeOffset.Now;

	public String EventType => TypeMapper.GetFullTypeName(this.GetType());
}