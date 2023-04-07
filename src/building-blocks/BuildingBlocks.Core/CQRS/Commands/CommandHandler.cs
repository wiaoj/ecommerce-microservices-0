using BuildingBlocks.Abstractions.CQRS.Commands;
using MediatR;

namespace BuildingBlocks.Core.CQRS.Commands;
public abstract class CommandHandler<TypeCommand> : ICommandHandler<TypeCommand> where TypeCommand : ICommand {
	protected abstract Task<Unit> HandleCommandAsync(TypeCommand command, CancellationToken cancellationToken);

	public Task<Unit> Handle(TypeCommand request, CancellationToken cancellationToken) {
		return this.HandleCommandAsync(request, cancellationToken);
	}
}

public abstract class CommandHandler<TypeCommand, TypeResponse> : ICommandHandler<TypeCommand, TypeResponse>
	where TypeCommand : ICommand<TypeResponse>
	where TypeResponse : notnull {
	protected abstract Task<TypeResponse> HandleCommandAsync(TypeCommand command, CancellationToken cancellationToken);

	public Task<TypeResponse> Handle(TypeCommand request, CancellationToken cancellationToken) {
		return this.HandleCommandAsync(request, cancellationToken);
	}
}
