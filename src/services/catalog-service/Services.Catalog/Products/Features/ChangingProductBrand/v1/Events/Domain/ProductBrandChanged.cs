using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Brands;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.ChangingProductBrand.v1.Events.Domain;
internal record ProductBrandChanged(BrandId BrandId, ProductId ProductId) : DomainEvent;