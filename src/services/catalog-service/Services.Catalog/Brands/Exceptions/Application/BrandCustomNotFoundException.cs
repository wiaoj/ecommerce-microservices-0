using BuildingBlocks.Core.Exception.Types;

namespace Services.Catalog.Brands.Exceptions.Application;
public class BrandCustomNotFoundException : CustomNotFoundException {
	public BrandCustomNotFoundException(Guid id) : base($"Brand with id '{id}' not found") {
	}

	public BrandCustomNotFoundException(string message) : base(message) {
	}
}
