using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Products.ValueObjects;
using Services.Catalog.Suppliers;

namespace Services.Catalog.Products.Features.ChangingProductSupplier.v1.Events;
public record ProductSupplierChanged(SupplierId SupplierId, ProductId ProductId) : DomainEvent;