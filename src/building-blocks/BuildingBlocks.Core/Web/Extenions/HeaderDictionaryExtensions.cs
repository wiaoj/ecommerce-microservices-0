using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Core.Web.Extenions;

// https://khalidabuhakmeh.com/read-and-convert-querycollection-values-in-aspnet
public static class HeaderDictionaryExtensions {
	public static IEnumerable<Type> All<Type>(this IHeaderDictionary collection, String key) {
		List<Type> values = new();

		if(collection.TryGetValue(key, out Microsoft.Extensions.Primitives.StringValues results)) {
			foreach(String? s in results) {
				try {
					Type? result = (Type)Convert.ChangeType(s, typeof(Type));
					values.Add(result);
				} catch {
					// conversion failed
					// skip value
				}
			}
		}

		// return an array with at least one
		return values;
	}

	public static Type Get<Type>(this IHeaderDictionary collection, String key) {
		return Get<Type>(collection, key, default, ParameterPick.First);
	}

	public static Type Get<Type>(this IHeaderDictionary collection, String key, Type @default, ParameterPick option) {
		List<Type> values = All<Type>(collection, key).ToList();
		Type? value = @default;

		if(values.Any()) {
			value = option switch {
				ParameterPick.First => values.FirstOrDefault(),
				ParameterPick.Last => values.LastOrDefault(),
				_ => value
			};
		}

		return value ?? @default;
	}
}

public enum ParameterPick {
	First,
	Last
}