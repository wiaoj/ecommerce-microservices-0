using BuildingBlocks.Core.Domain.Exceptions;

namespace Services.Catalog.Products.Exceptions.Domain;

public class InsufficientStockException : DomainException {
	public InsufficientStockException(string message) : base(message) {
	}
}
