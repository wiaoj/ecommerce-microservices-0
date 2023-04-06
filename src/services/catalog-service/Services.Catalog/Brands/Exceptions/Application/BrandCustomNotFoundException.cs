using BuildingBlocks.Core.Exceptions;

namespace Services.Catalog.Brands.Exceptions.Application;
public class BrandNotFoundException : NotFoundException {
	public BrandNotFoundException(Guid id) : base($"Brand with id '{id}' not found") { }

	public BrandNotFoundException(String message) : base(message) { }
}