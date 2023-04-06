using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.ReplenishingProductStock.Events.Domain;

public record ProductStockReplenished(ProductId ProductId, Stock NewStock, int ReplenishedQuantity) : DomainEvent;
