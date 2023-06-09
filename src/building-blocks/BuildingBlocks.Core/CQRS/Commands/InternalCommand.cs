using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Core.Types;

namespace BuildingBlocks.Core.CQRS.Commands;
public abstract record InternalCommand : IInternalCommand {
	public Guid InternalCommandId { get; protected set; } = Guid.NewGuid();

	public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;

	public String Type => TypeMapper.GetFullTypeName(this.GetType());
}