using Services.Catalog.Products.Models;

namespace Services.Catalog.Products.Dtos;
public record ProductDto {
	public Guid Id { get; init; }
	public String Name { get; init; } = default!;
	public String? Description { get; init; }
	public Decimal Price { get; init; }
	public Guid CategoryId { get; init; }
	public String CategoryName { get; init; } = default!;
	public Guid SupplierId { get; init; }
	public String SupplierName { get; init; } = default!;
	public Guid BrandId { get; init; }
	public String BrandName { get; init; } = default!;
	public Int32 AvailableStock { get; init; }
	public Int32 RestockThreshold { get; init; }
	public Int32 MaxStockThreshold { get; init; }
	public ProductStatus ProductStatus { get; init; }
	public ProductColor ProductColor { get; init; }
	public String Size { get; init; } = default!;
	public Int32 Height { get; init; }
	public Int32 Width { get; init; }
	public Int32 Depth { get; init; }
	public IEnumerable<ProductImageDto>? Images { get; init; }
}