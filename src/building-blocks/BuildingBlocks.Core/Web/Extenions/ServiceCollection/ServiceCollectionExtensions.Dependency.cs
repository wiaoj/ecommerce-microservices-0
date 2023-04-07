using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BuildingBlocks.Core.Web.Extenions.ServiceCollection;
public static partial class ServiceCollectionExtensions {
	public static IServiceCollection Replace<TypeService, TypeImplementation>(this IServiceCollection services, ServiceLifetime lifetime
	) {
		return services.Replace(new ServiceDescriptor(typeof(TypeService), typeof(TypeImplementation), lifetime));
	}

	public static IServiceCollection ReplaceScoped<TypeService, TypeImplementation>(this IServiceCollection services)
		where TypeService : class
		where TypeImplementation : class, TypeService {
		return services.Replace(ServiceDescriptor.Scoped<TypeService, TypeImplementation>());
	}

	public static IServiceCollection ReplaceScoped<TypeService>(
		this IServiceCollection services,
		Func<IServiceProvider, TypeService> implementationFactory) where TypeService : class {
		return services.Replace(ServiceDescriptor.Scoped(implementationFactory));
	}

	public static IServiceCollection ReplaceTransient<TypeService, TypeImplementation>(this IServiceCollection services)
		where TypeService : class
		where TypeImplementation : class, TypeService {
		return services.Replace(ServiceDescriptor.Transient<TypeService, TypeImplementation>());
	}

	public static IServiceCollection ReplaceTransient<TypeService>(
		this IServiceCollection services,
		Func<IServiceProvider, TypeService> implementationFactory
	)
		where TypeService : class {
		return services.Replace(ServiceDescriptor.Transient(implementationFactory));
	}

	public static IServiceCollection ReplaceSingleton<TypeService, TypeImplementation>(this IServiceCollection services)
		where TypeService : class
		where TypeImplementation : class, TypeService {
		return services.Replace(ServiceDescriptor.Singleton<TypeService, TypeImplementation>());
	}

	public static IServiceCollection ReplaceSingleton<TypeService>(
		this IServiceCollection services,
		Func<IServiceProvider, TypeService> implementationFactory
	)
		where TypeService : class {
		return services.Replace(ServiceDescriptor.Singleton(implementationFactory));
	}

	/// <summary>
	/// Adds a new transient registration to the service collection only when no existing registration of the same service type and implementation type exists.
	/// In contrast to TyperyAddTransient, which only checks the service type.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="serviceType"></param>
	/// <param name="implementationType"></param>
	public static void TyperyAddTransientExact(this IServiceCollection services, Type serviceType, Type implementationType) {
		if(services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType)) {
			return;
		}

		services.AddTransient(serviceType, implementationType);
	}

	/// <summary>
	/// Adds a new scope registration to the service collection only when no existing registration of the same service type and implementation type exists.
	/// In contrast to TyperyAddScope, which only checks the service type.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="serviceType"></param>
	/// <param name="implementationType"></param>
	public static void TyperyAddScopeExact(this IServiceCollection services, Type serviceType, Type implementationType) {
		if(services.Any(reg => reg.ServiceType == serviceType && reg.ImplementationType == implementationType)) {
			return;
		}

		services.AddScoped(serviceType, implementationType);
	}

	public static IServiceCollection Add<TypeService, TypeImplementation>(
		this IServiceCollection services,
		Func<IServiceProvider, TypeImplementation> implementationFactory,
		ServiceLifetime serviceLifetime = ServiceLifetime.Transient
	)
		where TypeService : class
		where TypeImplementation : class, TypeService {
		return serviceLifetime switch {
			ServiceLifetime.Singleton => services.AddSingleton<TypeService, TypeImplementation>(implementationFactory),
			ServiceLifetime.Scoped => services.AddScoped<TypeService, TypeImplementation>(implementationFactory),
			ServiceLifetime.Transient => services.AddTransient<TypeService, TypeImplementation>(implementationFactory),
			_ => throw new ArgumentNullException(nameof(serviceLifetime)),
		};
	}

	public static IServiceCollection Add<TypeService>(
		this IServiceCollection services,
		Func<IServiceProvider, TypeService> implementationFactory,
		ServiceLifetime serviceLifetime = ServiceLifetime.Transient
	)
		where TypeService : class {
		return serviceLifetime switch {
			ServiceLifetime.Singleton => services.AddSingleton(implementationFactory),
			ServiceLifetime.Scoped => services.AddScoped(implementationFactory),
			ServiceLifetime.Transient => services.AddTransient(implementationFactory),
			_ => throw new ArgumentNullException(nameof(serviceLifetime)),
		};
	}

	public static IServiceCollection Add<TypeService>(
		this IServiceCollection services,
		ServiceLifetime serviceLifetime = ServiceLifetime.Transient
	)
		where TypeService : class {
		return serviceLifetime switch {
			ServiceLifetime.Singleton => services.AddSingleton<TypeService>(),
			ServiceLifetime.Scoped => services.AddScoped<TypeService>(),
			ServiceLifetime.Transient => services.AddTransient<TypeService>(),
			_ => throw new ArgumentNullException(nameof(serviceLifetime)),
		};
	}

	public static IServiceCollection Add(
		this IServiceCollection services,
		Type serviceType,
		ServiceLifetime serviceLifetime = ServiceLifetime.Transient
	) {
		return serviceLifetime switch {
			ServiceLifetime.Singleton => services.AddSingleton(serviceType),
			ServiceLifetime.Scoped => services.AddScoped(serviceType),
			ServiceLifetime.Transient => services.AddTransient(serviceType),
			_ => throw new ArgumentNullException(nameof(serviceLifetime)),
		};
	}

	public static IServiceCollection Add<TypeService, TypeImplementation>(
		this IServiceCollection services,
		ServiceLifetime serviceLifetime = ServiceLifetime.Transient
	)
		where TypeService : class
		where TypeImplementation : class, TypeService {
		return serviceLifetime switch {
			ServiceLifetime.Singleton => services.AddSingleton<TypeService, TypeImplementation>(),
			ServiceLifetime.Scoped => services.AddScoped<TypeService, TypeImplementation>(),
			ServiceLifetime.Transient => services.AddTransient<TypeService, TypeImplementation>(),
			_ => throw new ArgumentNullException(nameof(serviceLifetime)),
		};
	}

	public static IServiceCollection Add(
		this IServiceCollection services,
		Type serviceType,
		Func<IServiceProvider, Object> implementationFactory,
		ServiceLifetime serviceLifetime = ServiceLifetime.Transient
	) {
		return serviceLifetime switch {
			ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationFactory),
			ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationFactory),
			ServiceLifetime.Transient => services.AddTransient(serviceType, implementationFactory),
			_ => throw new ArgumentNullException(nameof(serviceLifetime)),
		};
	}

	public static IServiceCollection Add(
		this IServiceCollection services,
		Type serviceType,
		Type implementationType,
		ServiceLifetime serviceLifetime = ServiceLifetime.Transient
	) {
		return serviceLifetime switch {
			ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
			ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
			ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
			_ => throw new ArgumentNullException(nameof(serviceLifetime)),
		};
	}
}