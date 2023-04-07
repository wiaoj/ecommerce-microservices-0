namespace BuildingBlocks.Abstractions.Persistence;
public interface IDataSeeder {
	public Task SeedAllAsync();
	public Int32 Order { get; }
}