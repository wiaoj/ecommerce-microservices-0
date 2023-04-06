using BuildingBlocks.Core.Exception.Types;

namespace Services.Catalog.Categories.Exceptions.Application;

public class CategoryCustomNotFoundException : CustomNotFoundException {
	public CategoryCustomNotFoundException(long id) : base($"Category with id '{id}' not found.") {
	}

	public CategoryCustomNotFoundException(string message) : base(message) {
	}
}
