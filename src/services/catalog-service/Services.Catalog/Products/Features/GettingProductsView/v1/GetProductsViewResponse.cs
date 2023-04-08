namespace Services.Catalog.Products.Features.GettingProductsView.v1;

public record GetProductsViewResponse(IEnumerable<ProductViewDto> Products);
