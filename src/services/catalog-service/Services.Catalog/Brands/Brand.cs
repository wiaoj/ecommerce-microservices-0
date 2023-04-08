using BuildingBlocks.Core.Domain;
using Services.Catalog.Brands.Exceptions.Domain;

namespace Services.Catalog.Brands;
public sealed class Brand : Aggregate<BrandId> {
	public String Name { get; private set; }

	private Brand(BrandId id, String name) {
		ArgumentNullException.ThrowIfNull(id, nameof(id));

		this.Id = id;
		this.Name = name;
	}

	public static Brand Create(BrandId id, String name) {
		return new(id, name);
	}

	public void ChangeName(String name) {
		if(String.IsNullOrWhiteSpace(name)) {
			throw new BrandDomainException("Name can't be white space or null.");
		}

		this.Name = name;
	}
}