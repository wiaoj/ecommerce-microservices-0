using Services.Catalog.Brands;
using Services.Catalog.Categories;
using Services.Catalog.Products.Models;
using Services.Catalog.Suppliers;

namespace Services.Catalog.Shared.Contracts;
public interface ICatalogDbContext {
	public DbSet<Product> Products { get; }
	public DbSet<Category> Categories { get; }
	public DbSet<Brand> Brands { get; }
	public DbSet<Supplier> Suppliers { get; }
	public DbSet<ProductView> ProductsView { get; }

	public DbSet<TEntity> Set<TEntity>() where TEntity : class;

	public Task<Int32> SaveChangesAsync(CancellationToken cancellationToken);
}