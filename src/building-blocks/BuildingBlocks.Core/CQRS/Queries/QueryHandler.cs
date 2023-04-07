using BuildingBlocks.Abstractions.CQRS.Queries;

namespace BuildingBlocks.Core.CQRS.Queries;
public abstract class QueryHandler<TypeQuery, TypeResponse> : IQueryHandler<TypeQuery, TypeResponse>
	where TypeQuery : IQuery<TypeResponse>
	where TypeResponse : notnull {
	protected abstract Task<TypeResponse> HandleQueryAsync(TypeQuery query, CancellationToken cancellationToken);

	public Task<TypeResponse> Handle(TypeQuery request, CancellationToken cancellationToken) {
		return this.HandleQueryAsync(request, cancellationToken);
	}
}