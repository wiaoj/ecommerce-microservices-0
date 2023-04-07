using MediatR;

namespace BuildingBlocks.Abstractions.CQRS.Queries;
public interface IQuery<out Type> : IRequest<Type> where Type : notnull { }

// https://jimmybogard.com/mediatr-10-0-released/
public interface IStreamQuery<out Type> : IStreamRequest<Type> where Type : notnull { }