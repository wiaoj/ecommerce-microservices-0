using MediatR;

namespace BuildingBlocks.Abstractions.CQRS.Commands;
public interface ICommandHandler<in TypeCommand> : ICommandHandler<TypeCommand, Unit> where TypeCommand : ICommand<Unit> { }

public interface ICommandHandler<in TypeCommand, TypeResponse> : IRequestHandler<TypeCommand, TypeResponse>
	where TypeCommand : ICommand<TypeResponse>
	where TypeResponse : notnull { }