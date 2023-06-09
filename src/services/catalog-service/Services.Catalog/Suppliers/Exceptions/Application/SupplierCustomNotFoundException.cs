using BuildingBlocks.Core.Exception.Types;

namespace Services.Catalog.Suppliers.Exceptions.Application;

public class SupplierCustomNotFoundException : CustomNotFoundException {
	public SupplierCustomNotFoundException(long id) : base($"Supplier with id '{id}' not found") {
	}

	public SupplierCustomNotFoundException(string message) : base(message) {
	}
}
