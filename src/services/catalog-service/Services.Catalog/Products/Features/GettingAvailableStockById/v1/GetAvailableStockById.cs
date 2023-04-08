using BuildingBlocks.Abstractions.CQRS.Queries;

namespace Services.Catalog.Products.Features.GettingAvailableStockById.v1;

public record GetAvailableStockById(long ProductId) : IQuery<int>;
