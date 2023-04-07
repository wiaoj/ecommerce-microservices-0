using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Abstractions.Web.Module;
public interface IModuleConfiguration {
	public WebApplicationBuilder AddModuleServices(WebApplicationBuilder builder);
	public Task<WebApplication> ConfigureModule(WebApplication app);
	public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
}