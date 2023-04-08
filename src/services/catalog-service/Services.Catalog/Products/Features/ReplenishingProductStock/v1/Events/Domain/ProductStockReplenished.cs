using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.ReplenishingProductStock.v1.Events.Domain;
public record ProductStockReplenished(ProductId ProductId, Stock NewStock, int ReplenishedQuantity) : DomainEvent;