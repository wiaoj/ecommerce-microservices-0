using MediatR;

namespace BuildingBlocks.Abstractions.CQRS.Queries;
public interface IQueryHandler<in TypeQuery, TypeResponse> : IRequestHandler<TypeQuery, TypeResponse>
	where TypeQuery : IQuery<TypeResponse>
	where TypeResponse : notnull {
}

// https://jimmybogard.com/mediatr-10-0-released/
public interface IStreamQueryHandler<in TypeQuery, TypeResponse> : IStreamRequestHandler<TypeQuery, TypeResponse>
	where TypeQuery : IStreamQuery<TypeResponse>
	where TypeResponse : notnull {
}