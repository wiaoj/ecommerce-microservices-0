using Services.Catalog.Products.Models;

namespace Services.Catalog.Products.Features.CreatingProduct.v1.Requests;
public record CreateProductRequest {
	public String Name { get; init; } = null!;
	public Decimal Price { get; init; }
	public Int32 Stock { get; init; }
	public Int32 RestockThreshold { get; init; }
	public Int32 MaxStockThreshold { get; init; }
	public ProductStatus Status { get; init; } = ProductStatus.Available;
	public Int32 Height { get; init; }
	public Int32 Width { get; init; }
	public Int32 Depth { get; init; }
	public String Size { get; init; } = null!;
	public ProductColor Color { get; init; }
	public Guid CategoryId { get; init; }
	public Guid SupplierId { get; init; }
	public Guid BrandId { get; init; }
	public String? Description { get; init; }
	public IEnumerable<CreateProductImageRequest>? Images { get; init; }
}