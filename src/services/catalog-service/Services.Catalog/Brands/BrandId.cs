using BuildingBlocks.Abstractions.Domain;

namespace Services.Catalog.Brands;

public record BrandId : AggregateId {
	public BrandId(long value) : base(value) {
	}

	public static implicit operator long(BrandId id) => id.Value;

	public static implicit operator BrandId(long id) => new(id);
}
