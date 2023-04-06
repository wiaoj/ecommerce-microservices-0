using Services.Catalog.Brands;
using Services.Catalog.Categories;
using Services.Catalog.Products.Models;
using Services.Catalog.Suppliers;

namespace Services.Catalog.Shared.Contracts;

public interface ICatalogDbContext {
	DbSet<Product> Products { get; }
	DbSet<Category> Categories { get; }
	DbSet<Brand> Brands { get; }
	DbSet<Supplier> Suppliers { get; }
	DbSet<ProductView> ProductsView { get; }

	DbSet<TEntity> Set<TEntity>()
		where TEntity : class;

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
