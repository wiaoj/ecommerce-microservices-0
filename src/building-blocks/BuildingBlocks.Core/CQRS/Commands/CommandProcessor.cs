using BuildingBlocks.Abstractions.CQRS.Commands;
using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using MediatR;

namespace BuildingBlocks.Core.CQRS.Commands;
public class CommandProcessor : ICommandProcessor {
	private readonly ISender sender;
	private readonly IMessagePersistenceService messagePersistenceService;

	public CommandProcessor(ISender sender, IMessagePersistenceService messagePersistenceService) {
		this.sender = sender;
		this.messagePersistenceService = messagePersistenceService;
	}

	public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken) where TResult : notnull {
		return this.sender.Send(command, cancellationToken);
	}

	public async Task ScheduleAsync(IInternalCommand internalCommandCommand, CancellationToken cancellationToken) {
		await this.messagePersistenceService.AddInternalMessageAsync(internalCommandCommand, cancellationToken);
	}

	public async Task ScheduleAsync(IInternalCommand[] internalCommandCommands, CancellationToken cancellationToken) {
		foreach(IInternalCommand internalCommandCommand in internalCommandCommands) {
			await this.ScheduleAsync(internalCommandCommand, cancellationToken);
		}
	}
}