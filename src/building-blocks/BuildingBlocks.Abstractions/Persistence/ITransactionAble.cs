namespace BuildingBlocks.Abstractions.Persistence;
public interface ITransactionAble {
	public Task BeginTransactionAsync(CancellationToken cancellationToken);
	public Task RollbackTransactionAsync(CancellationToken cancellationToken);
	public Task CommitTransactionAsync(CancellationToken cancellationToken);
}