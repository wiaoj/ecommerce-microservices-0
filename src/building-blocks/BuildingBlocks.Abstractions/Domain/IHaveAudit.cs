namespace BuildingBlocks.Abstractions.Domain;
public interface IHaveAudit : IHaveCreator {
	public DateTime? LastModified { get; }
	public Int32? LastModifiedBy { get; }
}