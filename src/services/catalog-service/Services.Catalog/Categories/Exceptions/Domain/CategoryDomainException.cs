using BuildingBlocks.Core.Domain.Exceptions;

namespace Services.Catalog.Categories.Exceptions.Domain;
public class CategoryDomainException : DomainException {
	public CategoryDomainException(String message) : base(message) { }
	public CategoryDomainException(Guid id) : base($"Category with id: '{id}' not found.") { }
}