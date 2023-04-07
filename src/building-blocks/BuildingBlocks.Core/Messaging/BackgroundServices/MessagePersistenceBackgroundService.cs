using BuildingBlocks.Abstractions.Messaging.PersistMessage;
using BuildingBlocks.Abstractions.Types;
using BuildingBlocks.Core.Messaging.MessagePersistence;
using BuildingBlocks.Core.Web.Extenions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Core.Messaging.BackgroundServices;

// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services
public class MessagePersistenceBackgroundService : BackgroundService {
	private readonly ILogger<MessagePersistenceBackgroundService> logger;
	private readonly IServiceProvider serviceProvider;
	private readonly IHostApplicationLifetime lifetime;
	private readonly MessagePersistenceOptions options;
	private readonly IMachineInstanceInfo machineInstanceInfo;

	public MessagePersistenceBackgroundService(
		ILogger<MessagePersistenceBackgroundService> logger,
		IOptions<MessagePersistenceOptions> options,
		IServiceProvider serviceProvider,
		IHostApplicationLifetime lifetime,
		IMachineInstanceInfo machineInstanceInfo
	) {
		this.logger = logger;
		this.serviceProvider = serviceProvider;
		this.lifetime = lifetime;
		this.options = options.Value;
		this.machineInstanceInfo = machineInstanceInfo;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		if(!await this.lifetime.WaitForAppStartup(stoppingToken)) {
			return;
		}

		this.logger.LogInformation(
			"MessagePersistence Background Service is starting on client '{@ClientId}' and group '{@ClientGroup}'",
			this.machineInstanceInfo.ClientId,
			this.machineInstanceInfo.ClientGroup
		);

		await this.ProcessAsync(stoppingToken);
	}

	public override Task StopAsync(CancellationToken cancellationToken) {
		this.logger.LogInformation(
			"MessagePersistence Background Service is stopping on client '{@ClientId}' and group '{@ClientGroup}'",
			this.machineInstanceInfo.ClientId,
			this.machineInstanceInfo.ClientGroup
		);

		return base.StopAsync(cancellationToken);
	}

	private async Task ProcessAsync(CancellationToken stoppingToken) {
		while(!stoppingToken.IsCancellationRequested) {
			await using(AsyncServiceScope scope = this.serviceProvider.CreateAsyncScope()) {
				IMessagePersistenceService service = scope.ServiceProvider.GetRequiredService<IMessagePersistenceService>();
				await service.ProcessAllAsync(stoppingToken);
			}

			TimeSpan delay = this.options.Interval is { }
				? TimeSpan.FromSeconds((Int32)this.options.Interval)
				: TimeSpan.FromSeconds(30);

			await Task.Delay(delay, stoppingToken);
		}
	}
}