namespace Services.Catalog.Products.Features.CreatingProduct.Requests;

public record CreateProductImageRequest {
	public string ImageUrl { get; init; } = default!;
	public bool IsMain { get; init; }
}
