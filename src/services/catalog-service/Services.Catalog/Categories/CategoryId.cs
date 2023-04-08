using BuildingBlocks.Abstractions.Domain;

namespace Services.Catalog.Categories;
public record CategoryId : AggregateId {
	public CategoryId(Guid value) : base(value) { }

	public static implicit operator Guid(CategoryId id) => id.Value;
	public static implicit operator CategoryId(Guid id) => new(id);
}