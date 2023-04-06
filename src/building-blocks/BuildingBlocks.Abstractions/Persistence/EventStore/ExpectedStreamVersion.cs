namespace BuildingBlocks.Abstractions.Persistence.EventStore;
public record ExpectedStreamVersion(Int64 Value) {
	public static readonly ExpectedStreamVersion NoStream = new(-1L);
	public static readonly ExpectedStreamVersion Any = new(-2L);
}

public record StreamReadPosition(Int64 Value) {
	public static readonly StreamReadPosition Start = new(0L);
}

public record StreamTruncatePosition(Int64 Value);