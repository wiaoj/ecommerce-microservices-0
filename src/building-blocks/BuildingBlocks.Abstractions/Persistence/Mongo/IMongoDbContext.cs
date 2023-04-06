using MongoDB.Driver;

namespace BuildingBlocks.Abstractions.Persistence.Mongo;
public interface IMongoDbContext : IDisposable {
	public IMongoCollection<Type> GetCollection<Type>(String? name);
	public Task<Int32> SaveChangesAsync(CancellationToken cancellationToken);
	public Task BeginTransactionAsync(CancellationToken cancellationToken);
	public Task CommitTransactionAsync(CancellationToken cancellationToken);
	public Task RollbackTransaction(CancellationToken cancellationToken);
	public void AddCommand(Func<Task> func);
}