namespace BuildingBlocks.Abstractions.Persistence.EventStore;
public record AppendResult(Int64 GlobalPosition, Int64 NextExpectedVersion) {
	public static readonly AppendResult None = new(0L, -1L);
};