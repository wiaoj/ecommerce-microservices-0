namespace Services.Catalog.Products.Dtos;
public record ProductImageDto {
	public Guid Id { get; init; }
	public String ImageUrl { get; init; } = default!;
	public Boolean IsMain { get; init; }
	public Guid ProductId { get; init; }
}