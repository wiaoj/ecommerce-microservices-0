namespace BuildingBlocks.Abstractions.Serialization;
public interface ISerializer {
	public String ContentType { get; }

	/// <summary>
	/// Serializes the given object into a string
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="camelCase"></param>
	/// <param name="indented"></param>
	/// <returns></returns>
	public String Serialize(Object obj, Boolean camelCase = true, Boolean indented = true);

	/// <summary>
	/// Deserialize the given string into a <see cref="Type"/>
	/// </summary>
	/// <param name="payload"></param>
	/// <param name="camelCase"></param>
	/// <typeparam name="Type"></typeparam>
	/// <returns></returns>
	public Type? Deserialize<Type>(String payload, Boolean camelCase = true);

	/// <summary>
	/// Deserialize the given string into a object.
	/// </summary>
	/// <param name="payload"></param>
	/// <param name="type"></param>
	/// <param name="camelCase"></param>
	/// <returns></returns>
	public Object? Deserialize(String payload, Type type, Boolean camelCase = true);
}