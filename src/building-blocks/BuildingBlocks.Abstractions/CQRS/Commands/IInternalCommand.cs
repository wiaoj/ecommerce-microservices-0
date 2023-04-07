namespace BuildingBlocks.Abstractions.CQRS.Commands;
public interface IInternalCommand : ICommand {
	public Guid InternalCommandId { get; }
	public DateTime OccurredOn { get; }
	public String Type { get; }
}