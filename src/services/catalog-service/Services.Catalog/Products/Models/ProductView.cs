namespace Services.Catalog.Products.Models;
public class ProductView {
	public Guid ProductId { get; set; }
	public String ProductName { get; set; } = default!;
	public Guid CategoryId { get; set; }
	public String CategoryName { get; set; } = default!;
	public Guid SupplierId { get; set; }
	public String SupplierName { get; set; } = default!;
	public Guid BrandId { get; set; }
	public String BrandName { get; set; } = default!;
}