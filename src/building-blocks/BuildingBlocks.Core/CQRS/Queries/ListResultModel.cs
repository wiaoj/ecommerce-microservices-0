namespace BuildingBlocks.Core.CQRS.Queries;
public record ListResultModel<Type>(List<Type> Items, Int64 TotalItems, Int32 Page, Int32 PageSize) where Type : notnull {
	public static ListResultModel<Type> Empty => new(Enumerable.Empty<Type>().ToList(), default, default, default);

	public static ListResultModel<Type> Create(List<Type> items, Int64 totalItems, Int32 page, Int32 pageSize) {
		return new ListResultModel<Type>(items, totalItems, page, pageSize);
	}

	public static ListResultModel<Type> Create(List<Type> items) {
		return Create(items, 0, 1, 20);
	}
}