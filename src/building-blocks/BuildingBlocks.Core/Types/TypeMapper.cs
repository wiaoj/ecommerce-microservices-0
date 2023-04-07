using BuildingBlocks.Core.Reflection;
using System.Collections.Concurrent;

namespace BuildingBlocks.Core.Types;
public static class TypeMapper {
	private static readonly ConcurrentDictionary<Type, String> TypeNameMap = new();
	private static readonly ConcurrentDictionary<String, Type> TypeMap = new();

	/// <summary>
	/// Gets the full type name from a generic Type class.
	/// </summary>
	/// <typeparam name="Type"></typeparam>
	/// <returns></returns>
	public static String GetFullTypeName<Type>() {
		return ToName(typeof(Type));
	}

	/// <summary>
	/// Gets the type name from a generic Type class without namespace.
	/// </summary>
	/// <typeparam name="Type"></typeparam>
	/// <returns></returns>
	public static String GetTypeName<Type>() {
		return ToName(typeof(Type), false);
	}

	/// <summary>
	/// Gets the type name from a Type class without namespace.
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static String GetTypeName(Type type) {
		return ToName(type, false);
	}

	/// <summary>
	/// Gets the full type name from a Type class.
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public static String GetFullTypeName(Type type) {
		return ToName(type);
	}

	/// <summary>
	/// Gets the type name from a instance object without namespace.
	/// </summary>
	/// <param name="object"></param>
	/// <returns></returns>
	public static String GetTypeNameByObject(Object @object) {
		return ToName(@object.GetType(), false);
	}

	/// <summary>
	/// Gets the full type name from a instance object.
	/// </summary>
	/// <param name="object"></param>
	/// <returns></returns>
	public static String GetFullTypeNameByObject(Object @object) {
		return ToName(@object.GetType());
	}

	/// <summary>
	/// Gets the type class from a type name.
	/// </summary>
	/// <param name="typeName"></param>
	/// <returns></returns>
	public static Type GetType(String typeName) {
		return ToType(typeName);
	}

	public static void AddType<T>(String name) {
		AddType(typeof(T), name);
	}

	private static void AddType(Type type, String name) {
		ToName(type);
		ToType(name);
	}

	public static Boolean IsTypeRegistered<T>() {
		return TypeNameMap.ContainsKey(typeof(T));
	}

	private static String ToName(Type type) => ToName(type, true);

	private static String ToName(Type type, Boolean fullName) {
		ArgumentNullException.ThrowIfNull(type);

		return TypeNameMap.GetOrAdd(type, _ => {
			String eventTypeName = fullName ? type.FullName! : type.Name;

			TypeMap.GetOrAdd(eventTypeName, type);

			return eventTypeName;
		});
	}

	private static Type ToType(String typeName) {
		ArgumentException.ThrowIfNullOrEmpty(typeName, nameof(typeName));

		return TypeMap.GetOrAdd(typeName, _ => {
			Type? type = ReflectionUtilities.GetFirstMatchingTypeFromCurrentDomainAssemblies(typeName);

			return type is null ? throw new Exception($"Type map for '{typeName}' wasn't found!") : type;
		});
	}
}
