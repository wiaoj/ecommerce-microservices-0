namespace Services.Catalog.Products.Features.CreatingProduct.v1.Requests;
public record CreateProductImageRequest {
	public String ImageUrl { get; init; } = default!;
	public Boolean IsMain { get; init; }
}