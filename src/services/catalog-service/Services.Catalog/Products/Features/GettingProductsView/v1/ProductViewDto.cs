namespace Services.Catalog.Products.Features.GettingProductsView.v1;

public record struct ProductViewDto(long Id, string Name, string CategoryName, string SupplierName, long ItemCount);
