using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace BuildingBlocks.Abstractions.Web.Module;
public interface ISharedModulesConfiguration {
	public WebApplicationBuilder AddSharedModuleServices(WebApplicationBuilder builder);
	public Task<WebApplication> ConfigureSharedModule(WebApplication app);
	public IEndpointRouteBuilder MapSharedModuleEndpoints(IEndpointRouteBuilder endpoints);
}