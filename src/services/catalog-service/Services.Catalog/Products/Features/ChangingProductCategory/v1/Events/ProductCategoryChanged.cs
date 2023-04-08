using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Categories;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.ChangingProductCategory.v1.Events;
public record ProductCategoryChanged(CategoryId CategoryId, ProductId ProductId) : DomainEvent;