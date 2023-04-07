using BuildingBlocks.Abstractions.CQRS.Events;
using BuildingBlocks.Abstractions.CQRS.Events.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BuildingBlocks.Core.Reflection.Extensions;

public static class TypeExtensions {
	private static readonly ConcurrentDictionary<Type, String> _typeCacheKeys = new();
	private static readonly ConcurrentDictionary<Type, String> _prettyPrintCache = new();

	private const BindingFlags PublicInstanceMembersFlag = BindingFlags.Public | BindingFlags.Instance;

	private const BindingFlags AllInstanceMembersFlag =
		BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

	private const BindingFlags AllStaticAndInstanceMembersFlag =
		PublicInstanceMembersFlag | BindingFlags.NonPublic | BindingFlags.Static;

	/// <summary>
	/// Invoke a static generic method
	/// </summary>
	/// <param name="type"></param>
	/// <param name="methodName"></param>
	/// <param name="genericTypes"></param>
	/// <param name="returnType"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	public static dynamic? InvokeGenericMethod(
		this Type type,
		String methodName,
		Type[] genericTypes,
		Type? returnType = null,
		params Object[] parameters
	) {
		MethodInfo? method = GetGenericMethod(
			type,
			methodName,
			genericTypes,
			parameters.Select(y => y.GetType()).ToArray(),
			returnType
		);

		if(method == null) {
			return null;
		}

		MethodInfo genericMethod = method.MakeGenericMethod(genericTypes);
		return genericMethod.Invoke(null, parameters);
	}

	// Ref: https://stackoverflow.com/a/588596/581476
	public static MethodInfo? GetGenericMethod(
		this Type t,
		String name,
		Type[] genericArgTypes,
		Type[] argTypes,
		Type? returnType = null
	) {
		MethodInfo? res = (
			from m in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
			where
				m.Name == name
				&& m.GetGenericArguments().Length == genericArgTypes.Length
				&& m.GetParameters().Select(pi => pi.ParameterType).All(d => argTypes.Any(a => a.IsAssignableTo(d)))
				&& (m.ReturnType == returnType || returnType == null)
			select m
		).FirstOrDefault();

		return res;
	}

	/// Ref: https://stackoverflow.com/a/39679855/581476
	/// <summary>
	/// Invoke an async static generic method
	/// </summary>
	/// <param name="type"></param>
	/// <param name="methodName"></param>
	/// <param name="genericTypes"></param>
	/// <param name="returnType"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	public static Task<dynamic>? InvokeGenericMethodAsync(
		this Type type,
		String methodName,
		Type[] genericTypes,
		Type? returnType = null,
		params Object[] parameters
	) {
		dynamic? awaitable = InvokeGenericMethod(type, methodName, genericTypes, returnType, parameters);

		return awaitable;
	}

	/// <summary>
	/// Invoke a static method.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="methodName"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	public static dynamic InvokeMethod(this Type type, String methodName, params Object[] parameters) {
		MethodInfo? method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
			.Where(x => x.Name == methodName)
			.FirstOrDefault(
				x => x.GetParameters().Select(p => p.ParameterType).All(parameters.Select(p => p.GetType()).Contains)
			);

		return method is null ? null : (dynamic)method.Invoke(null, parameters);
	}

	/// <summary>
	/// Invoke a async static method.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="methodName"></param>
	/// <param name="parameters"></param>
	/// <returns></returns>
	public static Task<dynamic> InvokeMethodAsync(this Type type, String methodName, params Object[] parameters) {
		dynamic awaitable = InvokeMethod(type, methodName, parameters);

		return awaitable;
	}

	public static String GetCacheKey(this Type type) {
		return _typeCacheKeys.GetOrAdd(type, t => $"{t.PrettyPrint()}");
	}

	/// <summary>
	/// This Methode extends the System.Type-type to get all extended methods. It searches hereby in all assemblies which are known by the current AppDomain.
	/// </summary>
	/// <remarks>
	/// Inspired by Jon Skeet from his answer on http://stackoverflow.com/questions/299515/c-sharp-reflection-to-identify-extension-methods
	/// </remarks>
	/// <returns>returns MethodInfo[] with the extended Method</returns>
	public static MethodInfo[] GetExtensionMethods(this Type t) {
		List<Type> AssTypes = new();

		foreach(Assembly item in AppDomain.CurrentDomain.GetAssemblies()) {
			AssTypes.AddRange(item.GetTypes());
		}

		IEnumerable<MethodInfo> query =
			from type in AssTypes
			where type.IsSealed && !type.IsGenericType && !type.IsNested
			from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
			where method.IsDefined(typeof(ExtensionAttribute), false)
			where method.GetParameters()[0].ParameterType == t
			select method;
		return query.ToArray<MethodInfo>();
	}

	public static MethodInfo GetExtensionMethod(this Type t, String methodeName) {
		IEnumerable<MethodInfo> mi = from methode in t.GetExtensionMethods() where methode.Name == methodeName select methode;
		return !mi.Any() ? null : mi.First<MethodInfo>();
	}

	private static String PrettyPrintRecursive(Type type, Int32 depth) {
		if(depth > 3) {
			return type.Name;
		}

		String[] nameParts = type.Name.Split('`');
		if(nameParts.Length == 1) {
			return nameParts[0];
		}

		Type[] genericArguments = type.GetTypeInfo().GetGenericArguments();
		return !type.IsConstructedGenericType
			? $"{nameParts[0]}<{new String(',', genericArguments.Length - 1)}>"
			: $"{nameParts[0]}<{String.Join(",", genericArguments.Select(t => PrettyPrintRecursive(t, depth + 1)))}>";
	}

	public static IEnumerable<Type> GetAllInterfacesImplementingOpenGenericInterface(
		this Type type,
		Type openGenericType
	) {
		Type[] interfaces = type.GetInterfaces();
		return interfaces.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == openGenericType);
	}

	// https://stackoverflow.com/questions/42245011/get-all-implementations-types-of-a-generic-interface
	public static IEnumerable<Type> GetAllTypesImplementingOpenGenericInterface(
		this Type openGenericType,
		params Assembly[] assemblies
	) {
		Assembly[] inputAssemblies = assemblies.Any() ? assemblies : AppDomain.CurrentDomain.GetAssemblies();
		return inputAssemblies.SelectMany(
			assembly => GetAllTypesImplementingOpenGenericInterface(openGenericType, assembly)
		);
	}

	public static IEnumerable<Type> GetAllTypesImplementingOpenGenericInterface(
		this Type openGenericType,
		Assembly assembly
	) {
		try {
			return GetAllTypesImplementingOpenGenericInterface(openGenericType, assembly.GetTypes());
		} catch(ReflectionTypeLoadException) {
			// It's expected to not being able to load all assemblies
			return new List<Type>();
		}
	}

	public static IEnumerable<Type> GetAllTypesImplementingOpenGenericInterface(
		this Type openGenericType,
		IEnumerable<Type> types
	) {
		return from type in types
			   from interfaceType in type.GetInterfaces()
			   where
				   interfaceType.IsGenericType
				   && openGenericType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition())
				   && type.IsClass
				   && !type.IsAbstract
			   select type;
	}

	// https://stackoverflow.com/questions/26733/getting-all-types-that-implement-an-interface
	public static IEnumerable<Type> GetAllTypesImplementingInterface(
		this Type interfaceType,
		params Assembly[] assemblies
	) {
		Assembly[] inputAssemblies = assemblies.Any() ? assemblies : AppDomain.CurrentDomain.GetAssemblies();
		return inputAssemblies.SelectMany(assembly => GetAllTypesImplementingInterface(interfaceType, assembly));
	}

	private static IEnumerable<Type> GetAllTypesImplementingInterface(this Type interfaceType, Assembly assembly) {
		return assembly
			.GetTypes()
			.Where(
				type => interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract && type.IsClass
			);
	}

	public static PropertyInfo[] FindPropertiesWithAttribute(this Type type, Type attribute) {
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		return properties.Where(x => x.GetCustomAttributes(attribute, true).Any()).ToArray();
	}

	public static Type[] GetTypeInheritanceChainTo(this Type type, Type toBaseType) {
		List<Type> retVal = new() {
			type
		};
		Type? baseType = type.BaseType;
		while(baseType != toBaseType && baseType != typeof(Object)) {
			retVal.Add(baseType);
			baseType = baseType?.BaseType;
		}

		return retVal.ToArray();
	}

	public static Boolean IsDerivativeOf(this Type type, Type typeToCompare) {
		if(type == null) {
			throw new ArgumentNullException(nameof(type));
		}

		Boolean retVal = type.BaseType != null;
		if(retVal) {
			retVal = type.BaseType == typeToCompare;
		}

		if(!retVal && type.BaseType != null) {
			retVal = type.BaseType.IsDerivativeOf(typeToCompare);
		}

		return retVal;
	}

	public static T CreateInstanceFromType<T>(this Type type, params Object[] parameters) {
		T? instance = (T)Activator.CreateInstance(type, parameters);

		return instance;
	}

	public static Boolean IsDictionary(this Type type) {
		if(type == null) {
			throw new ArgumentNullException("type");
		}

		Boolean retVal = typeof(IDictionary).IsAssignableFrom(type);
		if(!retVal) {
			retVal = type.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(type.GetGenericTypeDefinition());
		}

		return retVal;
	}

	public static Boolean IsAssignableFromGenericList(this Type type) {
		foreach(Type intType in type.GetInterfaces()) {
			if(intType.IsGenericType && intType.GetGenericTypeDefinition() == typeof(IList<>)) {
				return true;
			}
		}

		return false;
	}

	public static Boolean IsNonAbstractClass(this Type type, Boolean publicOnly) {
		TypeInfo typeInfo = type.GetTypeInfo();

		if(typeInfo.IsSpecialName) {
			return false;
		}

		if(typeInfo.IsClass && !typeInfo.IsAbstract) {
			if(typeInfo.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true)) {
				return false;
			}

			return !publicOnly || typeInfo.IsPublic || typeInfo.IsNestedPublic;
		}

		return false;
	}

	public static IEnumerable<Type> GetBaseTypes(this Type type) {
		TypeInfo typeInfo = type.GetTypeInfo();

		foreach(Type implementedInterface in typeInfo.ImplementedInterfaces) {
			yield return implementedInterface;
		}

		Type? baseType = typeInfo.BaseType;

		while(baseType != null) {
			TypeInfo baseTypeInfo = baseType.GetTypeInfo();

			yield return baseType;

			baseType = baseTypeInfo.BaseType;
		}
	}

	public static Boolean IsInNamespace(this Type type, String @namespace) {
		String typeNamespace = type.Namespace ?? String.Empty;

		if(@namespace.Length > typeNamespace.Length) {
			return false;
		}

		String typeSubNamespace = typeNamespace[..@namespace.Length];

		if(typeSubNamespace.Equals(@namespace, StringComparison.Ordinal)) {
			if(typeNamespace.Length == @namespace.Length) {
				//exactly the same
				return true;
			}

			//is a subnamespace?
			return typeNamespace[@namespace.Length] == '.';
		}

		return false;
	}

	public static Boolean IsInExactNamespace(this Type type, String @namespace) {
		return String.Equals(type.Namespace, @namespace, StringComparison.Ordinal);
	}

	public static Boolean HasAttribute(this Type type, Type attributeType) {
		return type.GetTypeInfo().IsDefined(attributeType, inherit: true);
	}

	public static Boolean HasAttribute<T>(this Type type, Func<T, Boolean> predicate)
		where T : Attribute {
		return type.GetTypeInfo().GetCustomAttributes<T>(inherit: true).Any(predicate);
	}

	public static Boolean IsAssignableTo(this Type type, Type otherType) {
		TypeInfo typeInfo = type.GetTypeInfo();
		TypeInfo otherTypeInfo = otherType.GetTypeInfo();

		return otherTypeInfo.IsGenericTypeDefinition
			? typeInfo.IsAssignableToGenericTypeDefinition(otherTypeInfo)
			: otherTypeInfo.IsAssignableFrom(typeInfo);
	}

	private static Boolean IsAssignableToGenericTypeDefinition(this TypeInfo typeInfo, TypeInfo genericTypeInfo) {
		IEnumerable<TypeInfo> interfaceTypes = typeInfo.ImplementedInterfaces.Select(t => t.GetTypeInfo());

		foreach(TypeInfo? interfaceType in interfaceTypes) {
			if(interfaceType.IsGenericType) {
				TypeInfo typeDefinitionTypeInfo = interfaceType.GetGenericTypeDefinition().GetTypeInfo();

				if(typeDefinitionTypeInfo.Equals(genericTypeInfo)) {
					return true;
				}
			}
		}

		if(typeInfo.IsGenericType) {
			TypeInfo typeDefinitionTypeInfo = typeInfo.GetGenericTypeDefinition().GetTypeInfo();

			if(typeDefinitionTypeInfo.Equals(genericTypeInfo)) {
				return true;
			}
		}

		TypeInfo? baseTypeInfo = typeInfo.BaseType?.GetTypeInfo();

		return baseTypeInfo is not null && baseTypeInfo.IsAssignableToGenericTypeDefinition(genericTypeInfo);
	}

	private static IEnumerable<Type> GetImplementedInterfacesToMap(TypeInfo typeInfo) {
		if(!typeInfo.IsGenericType) {
			return typeInfo.ImplementedInterfaces;
		}

		return !typeInfo.IsGenericTypeDefinition ? typeInfo.ImplementedInterfaces : FilterMatchingGenericInterfaces(typeInfo);
	}

	private static IEnumerable<Type> FilterMatchingGenericInterfaces(TypeInfo typeInfo) {
		Type[] genericTypeParameters = typeInfo.GenericTypeParameters;

		foreach(Type current in typeInfo.ImplementedInterfaces) {
			TypeInfo currentTypeInfo = current.GetTypeInfo();

			if(
				currentTypeInfo.IsGenericType
				&& currentTypeInfo.ContainsGenericParameters
				&& GenericParametersMatch(genericTypeParameters, currentTypeInfo.GenericTypeArguments)
			) {
				yield return currentTypeInfo.GetGenericTypeDefinition();
			}
		}
	}

	private static Boolean GenericParametersMatch(IReadOnlyList<Type> parameters, IReadOnlyList<Type> interfaceArguments) {
		if(parameters.Count != interfaceArguments.Count) {
			return false;
		}

		for(Int32 i = 0; i < parameters.Count; i++) {
			if(parameters[i] != interfaceArguments[i]) {
				return false;
			}
		}

		return true;
	}

	public static Boolean IsOpenGeneric(this Type type) {
		return type.GetTypeInfo().IsGenericTypeDefinition;
	}

	public static Boolean HasMatchingGenericArity(this Type interfaceType, TypeInfo typeInfo) {
		if(typeInfo.IsGenericType) {
			TypeInfo interfaceTypeInfo = interfaceType.GetTypeInfo();

			if(interfaceTypeInfo.IsGenericType) {
				Int32 argumentCount = interfaceType.GenericTypeArguments.Length;
				Int32 parameterCount = typeInfo.GenericTypeParameters.Length;

				return argumentCount == parameterCount;
			}

			return false;
		}

		return true;
	}

	/// <summary>
	/// Check if type is a value-type, primitive type or string
	/// </summary>
	private static Boolean IsPrimitive(this Type type) {
		return type == typeof(String) || type.IsValueType || type.IsPrimitive;
	}

	public static Type UnwrapNullableType(this Type type) {
		return Nullable.GetUnderlyingType(type) ?? type;
	}

	public static Boolean IsNullableType(this Type type) {
		TypeInfo typeInfo = type.GetTypeInfo();

		return !typeInfo.IsValueType
			|| (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>));
	}

	public static Type UnwrapEnumType(this Type type) {
		Boolean isNullable = type.IsNullableType();
		Type underlyingNonNullableType = isNullable ? type.UnwrapNullableType() : type;
		if(!underlyingNonNullableType.GetTypeInfo().IsEnum) {
			return type;
		}

		Type underlyingEnumType = Enum.GetUnderlyingType(underlyingNonNullableType);
		return isNullable ? MakeNullable(underlyingEnumType) : underlyingEnumType;
	}

	public static Type MakeNullable(this Type type, Boolean nullable = true) {
		return type.IsNullableType() == nullable
			? type
			: nullable
				? typeof(Nullable<>).MakeGenericType(type)
				: type.UnwrapNullableType();
	}

	/// <summary>
	/// Helper method use to differentiate behavior between command/query/event handlers.
	/// Command/Query handlers should only be added once (so set addIfAlreadyExists to false)
	/// Event handlers should all be added (set addIfAlreadyExists to true)
	/// </summary>
	public static void AddImplementationsAsTransient(
		this Type[] openMessageInterfaces,
		IServiceCollection services,
		IEnumerable<Assembly> assembliesToScan,
		Boolean addIfAlreadyExists
	) {
		foreach(Type openInterface in openMessageInterfaces) {
			List<Type> concretions = new();
			List<Type> interfaces = new();

			foreach(TypeInfo? type in assembliesToScan.SelectMany(a => a.DefinedTypes)) {
				IEnumerable<Type> interfaceTypes = type.FindInterfacesThatClose(openInterface).ToArray();
				if(!interfaceTypes.Any()) {
					continue;
				}

				if(type.IsConcrete()) {
					concretions.Add(type);
				}

				foreach(Type interfaceType in interfaceTypes) {
					if(interfaceType.GetInterfaces().Any()) {
						// Register the IRequestHandler instead of ICommand/Query/EventHandler
						interfaces.AddRange(interfaceType.GetInterfaces());
					} else {
						interfaces.Fill(interfaceType);
					}
				}
			}

			foreach(Type? @interface in interfaces.Distinct()) {
				List<Type> matches = concretions.Where(t => t.CanBeCastTo(@interface)).ToList();

				if(addIfAlreadyExists) {
					matches.ForEach(match => services.AddTransient(@interface, match));
				} else {
					if(matches.Count() > 1) {
						matches.RemoveAll(m => !IsMatchingWithInterface(m, @interface));
					}

					matches.ForEach(match => services.TryAddTransient(@interface, match));
				}

				if(!@interface.IsOpenGeneric()) {
					AddConcretionsThatCouldBeClosed(@interface, concretions, services);
				}
			}
		}
	}

	private static void Fill<T>(this IList<T> list, T value) {
		if(list.Contains(value)) {
			return;
		}

		list.Add(value);
	}

	// https://tmont.com/blargh/2011/3/determining-if-an-open-generic-type-isassignablefrom-a-type

	/// <summary>
	/// Determines whether the <paramref name="genericType"/> is assignable from
	/// <paramref name="givenType"/> taking into account generic definitions
	/// </summary>
	public static Boolean IsAssignableToGenericType(this Type givenType, Type genericType) {
		return givenType != null && genericType != null
&& (givenType == genericType
			|| givenType.MapsToGenericTypeDefinition(genericType)
			|| givenType.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
			|| givenType.BaseType.IsAssignableToGenericType(genericType));
	}

	private static Boolean HasInterfaceThatMapsToGenericTypeDefinition(this Type givenType, Type genericType) {
		return givenType
			.GetInterfaces()
			.Where(it => it.IsGenericType)
			.Any(it => it.GetGenericTypeDefinition() == genericType);
	}

	private static Boolean MapsToGenericTypeDefinition(this Type givenType, Type genericType) {
		return genericType.IsGenericTypeDefinition
			&& givenType.IsGenericType
			&& givenType.GetGenericTypeDefinition() == genericType;
	}

	public static Boolean IsEvent(this Type type) {
		return type.IsAssignableTo(typeof(IEvent));
	}

	private static Boolean IsRecord(this Type objectType) {
		return objectType.GetMethod("<Clone>$") != null
			|| ((TypeInfo)objectType).DeclaredProperties
				.FirstOrDefault(x => x.Name == "EqualityContract")
				?.GetMethod?.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) != null;
	}

	private static Boolean IsMatchingWithInterface(Type handlerType, Type handlerInterface) {
		if(handlerType == null || handlerInterface == null) {
			return false;
		}

		if(handlerType.IsInterface) {
			if(handlerType.GenericTypeArguments.SequenceEqual(handlerInterface.GenericTypeArguments)) {
				return true;
			}
		} else {
			return IsMatchingWithInterface(handlerType.GetInterface(handlerInterface.Name), handlerInterface);
		}

		return false;
	}

	private static void AddConcretionsThatCouldBeClosed(
		Type @interface,
		List<Type> concretions,
		IServiceCollection services
	) {
		foreach(Type? type in concretions.Where(x => x.IsOpenGeneric() && x.CouldCloseTo(@interface))) {
			services.TryAddTransient(@interface, type.MakeGenericType(@interface.GenericTypeArguments));
		}
	}

	public static Boolean CouldCloseTo(this Type openConcretion, Type closedInterface) {
		Type openInterface = closedInterface.GetGenericTypeDefinition();
		Type[] arguments = closedInterface.GenericTypeArguments;

		Type[] concreteArguments = openConcretion.GenericTypeArguments;
		return arguments.Length == concreteArguments.Length && openConcretion.CanBeCastTo(openInterface);
	}

	public static Boolean CanBeCastTo(this Type pluggedType, Type pluginType) {
		if(pluggedType == null) {
			return false;
		}

		return pluggedType == pluginType || pluginType.GetTypeInfo().IsAssignableFrom(pluggedType.GetTypeInfo());
	}

	public static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType) {
		if(!pluggedType.IsConcrete()) {
			yield break;
		}

		if(templateType.GetTypeInfo().IsInterface) {
			foreach(
				Type? interfaceType in pluggedType
					.GetTypeInfo()
					.ImplementedInterfaces.Where(
						type => type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == templateType)
					)
			) {
				yield return interfaceType;
			}
		} else if(
			  pluggedType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType
			  && (pluggedType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == templateType)
		  ) {
			yield return pluggedType.GetTypeInfo().BaseType;
		}

		if(pluggedType == typeof(Object)) {
			yield break;
		}

		if(pluggedType.GetTypeInfo().BaseType == typeof(Object)) {
			yield break;
		}

		foreach(Type interfaceType in FindInterfacesThatClose(pluggedType.GetTypeInfo().BaseType, templateType)) {
			yield return interfaceType;
		}
	}

	public static Boolean IsConcrete(this Type type) {
		return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
	}

	public static Type GetRegistrationType(this Type interfaceType, TypeInfo typeInfo) {
		if(typeInfo.IsGenericTypeDefinition) {
			TypeInfo interfaceTypeInfo = interfaceType.GetTypeInfo();

			if(interfaceTypeInfo.IsGenericType) {
				return interfaceType.GetGenericTypeDefinition();
			}
		}

		return interfaceType;
	}

	public static String GetModuleName(this Type type) {
		if(type?.Namespace is null) {
			return String.Empty;
		}

		String? moduleName = type.Assembly.GetName().Name;
		return type.Namespace.StartsWith(moduleName!, StringComparison.Ordinal)
			? type.Namespace.Split(".")[2].ToLowerInvariant()
			: String.Empty;
	}

	public static String PrettyPrint(this Type type) {
		return _prettyPrintCache.GetOrAdd(
			type,
			t => {
				try {
					return PrettyPrintRecursive(t, 0);
				} catch(System.Exception) {
					return t.Name;
				}
			}
		);
	}

	public static Boolean HasAggregateApplyMethod<TDomainEvent>(this Type type) {
		return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.Any(
				mi =>
					mi.Name == "Apply"
					&& mi.GetParameters().Length == 1
					&& typeof(TDomainEvent).GetTypeInfo().IsAssignableFrom(mi.GetParameters()[0].ParameterType)
			);
	}

	public static Boolean HasAggregateApplyMethod(this Type type, Type eventType) {
		return type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.Any(
				mi =>
					mi.Name == "Apply"
					&& mi.GetParameters().Length == 1
					&& eventType.GetTypeInfo().IsAssignableFrom(mi.GetParameters()[0].ParameterType)
			);
	}

	public static IReadOnlyDictionary<Type, Action<TDomainEvent>> GetAggregateApplyMethods<TDomainEvent>(this Type type)
		where TDomainEvent : IDomainEvent {
		Type aggregateEventType = typeof(TDomainEvent);

		return type.GetTypeInfo()
			.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.Where(mi => {
				if(
					!String.Equals(mi.Name, "Apply", StringComparison.Ordinal)
					&& !mi.Name.EndsWith(".Apply", StringComparison.Ordinal)
				) {
					return false;
				}

				ParameterInfo[] parameters = mi.GetParameters();
				return parameters.Length == 1
					&& aggregateEventType.GetTypeInfo().IsAssignableFrom(parameters[0].ParameterType);
			})
			.ToDictionary(
				mi => mi.GetParameters()[0].ParameterType,
				mi => type.CompileMethodInvocation<Action<TDomainEvent>>(mi.Name, mi.GetParameters()[0].ParameterType)
			);
	}

	/// <summary>
	/// Handles correct upcast. If no upcast was needed, then this could be exchanged to an <c>Expression.Call</c>
	/// and an <c>Expression.Lambda</c>.
	/// </summary>
	public static TResult CompileMethodInvocation<TResult>(
		this Type type,
		String methodName,
		params Type[] methodSignature
	) {
		TypeInfo typeInfo = type.GetTypeInfo();
		IEnumerable<MethodInfo> methods = typeInfo
			.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.Where(m => m.Name == methodName);

		MethodInfo? methodInfo =
			methodSignature == null || !methodSignature.Any()
				? methods.SingleOrDefault()
				: methods.SingleOrDefault(
					m => m.GetParameters().Select(mp => mp.ParameterType).SequenceEqual(methodSignature)
				);

		return methodInfo == null
			? throw new ArgumentException($"Type '{type.PrettyPrint()}' doesn't have a method called '{methodName}'")
			: ReflectionUtilities.CompileMethodInvocation<TResult>(methodInfo);
	}

	public static PropertyInfo? FindProperty(this Type type, String propertyName) {
		PropertyInfo? res = null;
		foreach(String prop in propertyName.Split('.')) {
			res = res == null ? type.GetProperty(prop) : res.PropertyType.GetProperty(prop);
		}

		return res;
	}
}
