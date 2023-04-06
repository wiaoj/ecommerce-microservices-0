using BuildingBlocks.Core.Domain.Exceptions;

namespace Services.Catalog.Products.Exceptions.Domain;

public class ProductDomainException : DomainException {
	public ProductDomainException(string message) : base(message) {
	}
}
