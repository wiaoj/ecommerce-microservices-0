namespace BuildingBlocks.Abstractions.CQRS.Commands;
public interface IInternalCommand : ICommand {
	public Guid Id { get; }
	public DateTime OccurredOn { get; }
	public String Type { get; }
}