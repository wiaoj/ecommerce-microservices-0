namespace BuildingBlocks.Abstractions.CQRS.Queries;
public interface IListQuery<out TypeResponse> : IQuery<TypeResponse> where TypeResponse : notnull {
	public IList<String>? Includes { get; init; }
	public IList<FilterModel>? Filters { get; init; }
	public IList<String>? Sorts { get; init; }
	public Int32 Page { get; init; }
	public Int32 PageSize { get; init; }
}