namespace Services.Catalog.Products.Features.GettingProductsView;

public record struct ProductViewDto(long Id, string Name, string CategoryName, string SupplierName, long ItemCount);
