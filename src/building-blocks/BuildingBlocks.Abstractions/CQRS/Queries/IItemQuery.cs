namespace BuildingBlocks.Abstractions.CQRS.Queries;
public interface IItemQuery<TypeId, out TypeResponse> : IQuery<TypeResponse> where TypeId : struct where TypeResponse : notnull {
	public IList<String> Includes { get; }
	public TypeId Id { get; }
}