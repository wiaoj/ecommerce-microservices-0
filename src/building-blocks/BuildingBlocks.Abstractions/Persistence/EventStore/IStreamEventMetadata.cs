namespace BuildingBlocks.Abstractions.Persistence.EventStore;
public interface IStreamEventMetadata {
	public String EventId { get; }
	public Int64? LogPosition { get; }
	public Int64 StreamPosition { get; }
}