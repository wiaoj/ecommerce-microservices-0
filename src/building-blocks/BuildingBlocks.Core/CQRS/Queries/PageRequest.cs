using BuildingBlocks.Abstractions.CQRS;
using BuildingBlocks.Abstractions.CQRS.Queries;

namespace BuildingBlocks.Core.CQRS.Queries;
public record PageRequest : IPageRequest {
	public Int32 Page { get; init; } = 1;
	public Int32 PageSize { get; init; } = 20;
	public IList<String>? Includes { get; init; }
	public IList<FilterModel>? Filters { get; init; }
	public IList<String>? Sorts { get; init; }
}