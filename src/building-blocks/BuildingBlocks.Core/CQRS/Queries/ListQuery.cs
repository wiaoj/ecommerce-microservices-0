using BuildingBlocks.Abstractions.CQRS;
using BuildingBlocks.Abstractions.CQRS.Queries;

namespace BuildingBlocks.Core.CQRS.Queries;
public record ListQuery<TypeResponse> : IListQuery<TypeResponse> where TypeResponse : notnull {
	public IList<String>? Includes { get; init; }
	public IList<FilterModel>? Filters { get; init; }
	public IList<String>? Sorts { get; init; }
	public Int32 Page { get; init; }
	public Int32 PageSize { get; init; }
}