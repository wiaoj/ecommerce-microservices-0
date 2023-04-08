using BuildingBlocks.Core.Exceptions;

namespace Services.Catalog.Categories.Exceptions.Application;
public class CategoryNotFoundException : NotFoundException {
	public CategoryNotFoundException(String message) : base(message) { }
	public CategoryNotFoundException(Guid id) : base($"Category with id '{id}' not found.") { }
}