namespace BuildingBlocks.Abstractions.Persistence;
public interface IUnitOfWork : IDisposable {
	public Task BeginTransactionAsync(CancellationToken cancellationToken);
	public Task CommitAsync(CancellationToken cancellationToken);
}

public interface IUnitOfWork<out TypeContext> : IUnitOfWork where TypeContext : class {
	public TypeContext Context { get; }
}