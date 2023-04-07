namespace BuildingBlocks.Abstractions.Types;
public interface IMachineInstanceInfo {
	public String ClientGroup { get; }
	public Guid ClientId { get; }
}