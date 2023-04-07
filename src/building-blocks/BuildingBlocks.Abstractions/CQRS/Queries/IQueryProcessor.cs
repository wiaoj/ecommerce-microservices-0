namespace BuildingBlocks.Abstractions.CQRS.Queries;
public interface IQueryProcessor {
	public Task<TypeResponse> SendAsync<TypeResponse>(IQuery<TypeResponse> query, CancellationToken cancellationToken)
		where TypeResponse : notnull;

	public IAsyncEnumerable<TypeResponse> SendAsync<TypeResponse>(IStreamQuery<TypeResponse> query, CancellationToken cancellationToken) 
		where TypeResponse : notnull;
}