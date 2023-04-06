using MediatR;

namespace BuildingBlocks.Abstractions.CQRS.Commands;
public interface ICommand : ICommand<Unit> { }

public interface ICommand<out TypeRequest> : IRequest<TypeRequest> where TypeRequest : notnull { }