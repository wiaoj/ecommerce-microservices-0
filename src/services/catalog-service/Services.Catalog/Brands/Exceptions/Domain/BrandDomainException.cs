using BuildingBlocks.Core.Domain.Exceptions;

namespace Services.Catalog.Brands.Exceptions.Domain;
public class BrandDomainException : DomainException {
	public BrandDomainException(String message) : base(message) { }
}