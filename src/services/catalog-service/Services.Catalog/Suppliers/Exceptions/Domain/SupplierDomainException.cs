using BuildingBlocks.Core.Domain.Exceptions;

namespace Services.Catalog.Suppliers.Exceptions.Domain;

public class SupplierDomainException : DomainException {
	public SupplierDomainException(string message) : base(message) {
	}
}
