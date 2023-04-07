namespace BuildingBlocks.Persistence.EfCore.Postgres;
public class PostgresOptions {
	public String ConnectionString { get; set; } = default!;
	public Boolean UseInMemory { get; set; }
	public String? MigrationAssembly { get; set; } = null!;
}