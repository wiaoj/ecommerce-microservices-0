using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.ChangingProductPrice.v1;
public record ProductPriceChanged(Price Price) : DomainEvent;