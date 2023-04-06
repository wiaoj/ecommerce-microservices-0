namespace BuildingBlocks.Abstractions.Domain;
public interface IHaveCreator {
	public DateTime Created { get; }
	public Int32? CreatedBy { get; }
}