using BuildingBlocks.Core.Exception.Types;

namespace Services.Catalog.Products.Exceptions.Application;

public class ProductCustomNotFoundException : CustomNotFoundException {
	public ProductCustomNotFoundException(long id) : base($"Product with id '{id}' not found") {
	}

	public ProductCustomNotFoundException(string message) : base(message) {
	}
}
