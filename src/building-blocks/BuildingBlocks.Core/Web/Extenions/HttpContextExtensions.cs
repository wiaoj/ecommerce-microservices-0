using BuildingBlocks.Abstractions.CQRS.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace BuildingBlocks.Core.Web.Extenions;
public static class HttpContextExtensions {
	public static String? GetTraceId(this IHttpContextAccessor httpContextAccessor) {
		return Activity.Current?.TraceId.ToString() ?? httpContextAccessor?.HttpContext?.TraceIdentifier;
	}

	public static String GetCorrelationId(this HttpContext httpContext) {
		httpContext.Request.Headers.TryGetValue("Cko-Correlation-InternalCommandId", out StringValues correlationId);
		return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
	}

	public static TResult? ExtractXQueryObjectFromHeader<TResult>(this HttpContext httpContext, String query)
		where TResult : IPageRequest, new() {
		TResult? queryModel = new();
		if(!(String.IsNullOrEmpty(query) || query == "{}")) {
			queryModel = JsonConvert.DeserializeObject<TResult>(query);
		}

		httpContext?.Response.Headers.Add(
			"x-query",
			JsonConvert.SerializeObject(
				queryModel,
				new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
			)
		);

		return queryModel;
	}

	public static String? GetUserHostAddress(this HttpContext context) {
		return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
			?? context.Connection.RemoteIpAddress?.ToString();
	}
}