using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Core.Web.Extenions;
public static class HostEnvironmentExtensions {
	public static Boolean IsTest(this IHostEnvironment env) {
		return env.IsEnvironment("test");
	}

	public static Boolean IsDocker(this IHostEnvironment env) {
		return env.IsEnvironment("docker");
	}
}