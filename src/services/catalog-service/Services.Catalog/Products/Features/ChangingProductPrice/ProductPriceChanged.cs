using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.ChangingProductPrice;

public record ProductPriceChanged(Price Price) : DomainEvent;
