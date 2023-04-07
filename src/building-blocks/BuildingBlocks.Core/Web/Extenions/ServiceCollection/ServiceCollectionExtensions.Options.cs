using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BuildingBlocks.Core.Web.Extenions.ServiceCollection;
public static partial class ServiceCollectionExtensions {
	public static IServiceCollection AddConfigurationOptions<Type>(this IServiceCollection services) where Type : class {
		return services.AddConfigurationOptions<Type>(typeof(Type).Name);
	}

	public static IServiceCollection AddConfigurationOptions<Type>(this IServiceCollection services, String key)
		where Type : class {
		services.AddOptions<Type>().BindConfiguration(key);

		return services.AddSingleton(x => x.GetRequiredService<IOptions<Type>>().Value);
	}

	public static IServiceCollection AddValidatedOptions<Type>(this IServiceCollection services)
		where Type : class {
		return AddValidatedOptions<Type>(services, typeof(Type).Name, RequiredConfigurationValidator.Validate);
	}

	public static IServiceCollection AddValidatedOptions<Type>(this IServiceCollection services, String key)
		where Type : class {
		return AddValidatedOptions<Type>(services, key, RequiredConfigurationValidator.Validate);
	}

	public static IServiceCollection AddValidatedOptions<Type>(
		this IServiceCollection services,
		String key,
		Func<Type, Boolean> validator
	)
		where Type : class {
		// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options
		// https://thecodeblogger.com/2021/04/21/options-pattern-in-net-ioptions-ioptionssnapshot-ioptionsmonitor/
		// https://code-maze.com/aspnet-configuration-options/
		// https://code-maze.com/aspnet-configuration-options-validation/
		// https://dotnetdocs.ir/Post/42/difference-between-ioptions-ioptionssnapshot-and-ioptionsmonitor
		// https://andrewlock.net/adding-validation-to-strongly-typed-configuration-objects-in-dotnet-6/
		services.AddOptions<Type>().BindConfiguration(key).Validate(validator);

		// IOptions itself registered as singleton
		return services.AddSingleton(x => x.GetRequiredService<IOptions<Type>>().Value);
	}
}

public static class RequiredConfigurationValidator {
	public static Boolean Validate<Type>(Type arg)
		where Type : class {
		IEnumerable<PropertyInfo> requiredProperties = typeof(Type)
			.GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(x => Attribute.IsDefined(x, typeof(RequiredMemberAttribute)));

		foreach(PropertyInfo? requiredProperty in requiredProperties) {
			Object? propertyValue = requiredProperty.GetValue(arg);
			if(propertyValue is null) {
				throw new System.Exception($"Required property '{requiredProperty.Name}' was null");
			}
		}

		return true;
	}
}
