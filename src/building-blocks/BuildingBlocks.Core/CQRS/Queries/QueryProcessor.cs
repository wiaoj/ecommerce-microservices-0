using BuildingBlocks.Abstractions.CQRS.Queries;
using MediatR;

namespace BuildingBlocks.Core.CQRS.Queries;
public class QueryProcessor : IQueryProcessor {
	private readonly ISender sender;

	public QueryProcessor(ISender sender) {
		this.sender = sender;
	}

	public Task<TypeResponse> SendAsync<TypeResponse>(IQuery<TypeResponse> query, CancellationToken cancellationToken) 
		where TypeResponse : notnull {
		return this.sender.Send(query, cancellationToken);
	}

	public IAsyncEnumerable<TypeResponse> SendAsync<TypeResponse>(IStreamQuery<TypeResponse> query, CancellationToken cancellationToken) 
		where TypeResponse : notnull {
		return this.sender.CreateStream(query, cancellationToken);
	}
}