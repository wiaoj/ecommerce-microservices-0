using BuildingBlocks.Abstractions.Domain;

namespace Services.Catalog.Products.ValueObjects;
public record ProductId : AggregateId {
	public ProductId(Guid value) : base(value) { }

	public static implicit operator Guid(ProductId id) => id.Value;
	public static implicit operator ProductId(Guid id) => new(id);
}