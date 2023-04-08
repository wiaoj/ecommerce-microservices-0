using BuildingBlocks.Core.Exceptions;

namespace Services.Catalog.Products.Exceptions.Application;
public class ProductNotFoundException : NotFoundException {
	public ProductNotFoundException(Guid id) : base($"Product with id '{id}' not found") { }
	public ProductNotFoundException(String message) : base(message) { }
}