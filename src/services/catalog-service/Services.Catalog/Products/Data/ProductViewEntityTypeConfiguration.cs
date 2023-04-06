using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Catalog.Products.Models;
using Services.Catalog.Shared.Data;

namespace Services.Catalog.Products.Data;

public class ProductViewEntityTypeConfiguration : IEntityTypeConfiguration<ProductView> {
	public void Configure(EntityTypeBuilder<ProductView> builder) {
		builder.ToTable("product_views", CatalogDbContext.DefaultSchema);
		builder.HasKey(x => x.ProductId);
		builder.HasIndex(x => x.ProductId).IsUnique();
	}
}
