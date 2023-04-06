using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.DebitingProductStock.Events.Domain;

public record ProductStockDebited(ProductId ProductId, Stock NewStock, int DebitedQuantity) : DomainEvent;
