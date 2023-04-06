using BuildingBlocks.Abstractions.Domain;

namespace Services.Catalog.Brands;

public sealed record BrandId : AggregateId {
	private BrandId(Guid value) : base(value) { }

	public static BrandId Generate => new(Guid.NewGuid());


	public static implicit operator Guid(BrandId id) => id.Value;
	public static implicit operator BrandId(long id) => new(id);
}