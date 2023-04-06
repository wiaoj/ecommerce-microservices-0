namespace BuildingBlocks.Abstractions.Persistence;
public interface ITxDbContextExecution{
    public Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken cancellationToken);
    public Task<Type> ExecuteTransactionalAsync<Type>(Func<Task<Type>> action, CancellationToken cancellationToken);
}