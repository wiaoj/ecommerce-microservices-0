namespace BuildingBlocks.Abstractions.Persistence.EfCore;
public interface IRetryDbContextExecution {
	public Task RetryOnExceptionAsync(Func<Task> operation);
	public Task<TypeResult> RetryOnExceptionAsync<TypeResult>(Func<Task<TypeResult>> operation);
}