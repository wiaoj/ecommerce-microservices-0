using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Categories;
using Services.Catalog.Products.ValueObjects;

namespace Services.Catalog.Products.Features.ChangingProductCategory.Events;

public record ProductCategoryChangedNotification(CategoryId CategoryId, ProductId ProductId) : DomainEvent;
