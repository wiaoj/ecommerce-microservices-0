using Bogus;
using BuildingBlocks.Abstractions.Persistence;
using Services.Catalog.Shared.Contracts;

namespace Services.Catalog.Brands.Data;
public class BrandDataSeeder : IDataSeeder {
	private readonly ICatalogDbContext context;

	public Int32 Order => 3;

	public BrandDataSeeder(ICatalogDbContext context) {
		this.context = context;
	}

	public async Task SeedAllAsync() {
		if(await this.context.Brands.AnyAsync()) {
			return;
		}

		// https://github.com/bchavez/Bogus
		// https://www.youtube.com/watch?v=T9pwE1GAr_U
		Faker<Brand> brandFaker = new Faker<Brand>().CustomInstantiator(faker => {
			Brand brand = Brand.Create(Guid.NewGuid(), faker.Company.CompanyName());
			return brand;
		});
		List<Brand> brands = brandFaker.Generate(5);

		await this.context.Brands.AddRangeAsync(brands);
		await this.context.SaveChangesAsync();
	}
}