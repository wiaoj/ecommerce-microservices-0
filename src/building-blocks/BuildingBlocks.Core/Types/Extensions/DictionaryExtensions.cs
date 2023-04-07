namespace BuildingBlocks.Core.Types.Extensions;
public static class DictionaryExtensions {
	public static Boolean TryAdd<Type, TypeValue>(this IDictionary<Type, TypeValue> dictionary, Type key, TypeValue value) {
		if(dictionary.ContainsKey(key)) {
			return false;
		}

		dictionary.Add(key, value);
		return true;
	}

	public static Boolean AddOrReplace<TypeKey, TypeValue>(this IDictionary<TypeKey, TypeValue> dictionary, TypeKey key, TypeValue value) {
		if(dictionary.ContainsKey(key)) {
			dictionary.Remove(key);
		}

		return dictionary.TryAdd(key, value);
	}

	public static Object? Get(this IDictionary<String, Object?> dictionary, String key) {
		dictionary.TryGetValue(key, out Object? val);

		return val;
	}

	public static TypeValue? Get<TypeValue>(this IDictionary<String, Object?> dictionary, String key) where TypeValue : class {
		dictionary.TryGetValue(key, out Object? value);

		return value as TypeValue;
	}
}