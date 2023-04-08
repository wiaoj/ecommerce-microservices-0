using BuildingBlocks.Core.Domain.Exceptions;

namespace Services.Catalog.Products.Exceptions.Domain;
public class MaxStockThresholdReachedException : DomainException {
	public MaxStockThresholdReachedException(String message) : base(message) { }
}