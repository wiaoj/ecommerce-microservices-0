using BuildingBlocks.Abstractions.CQRS.Queries;

namespace Services.Catalog.Products.Features.GettingAvailableStockById;

public record GetAvailableStockById(long ProductId) : IQuery<int>;

