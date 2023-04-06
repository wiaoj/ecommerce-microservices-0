using BuildingBlocks.Core.CQRS.Queries;
using Services.Catalog.Products.Dtos;

namespace Services.Catalog.Products.Features.GettingProducts;

public record GetProductsResponse(ListResultModel<ProductDto> Products);
