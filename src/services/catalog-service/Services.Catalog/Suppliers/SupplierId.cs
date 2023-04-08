using BuildingBlocks.Abstractions.Domain;

namespace Services.Catalog.Suppliers;
public record SupplierId : AggregateId {
	public SupplierId(Guid value) : base(value) { }

	public static implicit operator Guid(SupplierId id) => id.Value;
	public static implicit operator SupplierId(Guid id) => new(id);
}