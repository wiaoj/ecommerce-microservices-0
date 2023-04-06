using BuildingBlocks.Core.CQRS.Events.Internal;
using Services.Catalog.Brands;
using Services.Catalog.Categories;
using Services.Catalog.Products.Models;
using Services.Catalog.Products.ValueObjects;
using Services.Catalog.Suppliers;

namespace Services.Catalog.Products.Features.CreatingProduct.Events.Domain;

public record CreatingProduct(
	ProductId Id,
	Name Name,
	Price Price,
	Stock Stock,
	ProductStatus Status,
	Dimensions Dimensions,
	Category? Category,
	Supplier? Supplier,
	Brand? Brand,
	string? Description = null) : DomainEvent;
