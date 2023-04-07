using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Persistence.EfCore.Postgres;
public abstract class DbContextDesignFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
	where TDbContext : DbContext {
	private readonly String connectionStringSection;
	private readonly String? environment;

	protected DbContextDesignFactoryBase(String connectionStringSection, String? environment = null) {
		this.connectionStringSection = connectionStringSection;
		this.environment = environment;
	}

	public TDbContext CreateDbContext(String[] args) {
		Console.WriteLine($"BaseDirectory: {AppContext.BaseDirectory}");

		String environmentName = this.environment ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "test";

		IConfigurationBuilder builder = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory ?? "")
			.AddJsonFile("appsettings.json")
			.AddJsonFile($"appsettings.{environmentName}.json", true) // it is optional
			.AddEnvironmentVariables();

		IConfigurationRoot configuration = builder.Build();

		String? connectionStringSectionValue = configuration.GetValue<String>(this.connectionStringSection);

		if(String.IsNullOrWhiteSpace(connectionStringSectionValue)) {
			throw new InvalidOperationException($"Could not find a value for {this.connectionStringSection} section.");
		}

		Console.WriteLine($"ConnectionString  section value is : {connectionStringSectionValue}");

		var optionsBuilder = new DbContextOptionsBuilder<TDbContext>()
			.UseNpgsql(
				connectionStringSectionValue,
				sqlOptions => {
					sqlOptions.MigrationsAssembly(this.GetType().Assembly.FullName);
					sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
				}
			)
			.UseSnakeCaseNamingConvention()
			.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector<Int64>>();

		return (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options);
	}
}