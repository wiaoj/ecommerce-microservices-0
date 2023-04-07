using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Core.Web.Extenions;
public static class HostApplicationLifetimeExtensions {
	// ref: https://andrewlock.net/finding-the-urls-of-an-aspnetcore-app-from-a-hosted-service-in-dotnet-6/
	public static async Task<Boolean> WaitForAppStartup(
		this IHostApplicationLifetime lifetime,
		CancellationToken stoppingToken
	) {
		TaskCompletionSource startedSource = new();
		TaskCompletionSource cancelledSource = new();

		using CancellationTokenRegistration reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());
		using CancellationTokenRegistration reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

		Task completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

		// If the completed tasks was the "app started" task, return true, otherwise false
		return completedTask == startedSource.Task;
	}
}