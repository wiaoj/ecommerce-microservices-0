namespace BuildingBlocks.Abstractions.CQRS.Commands;
public interface ICommandProcessor {
	public Task<TypeResult> SendAsync<TypeResult>(ICommand<TypeResult> command, CancellationToken cancellationToken) where TypeResult : notnull;
	public Task ScheduleAsync(IInternalCommand internalCommandCommand, CancellationToken cancellationToken);
	public Task ScheduleAsync(IInternalCommand[] internalCommandCommands, CancellationToken cancellationToken);
}