using System.ComponentModel;
using System.Reflection;

namespace BuildingBlocks.Core.Types.Extensions;
// https://stackoverflow.com/a/19621488/581476
public static class EnumExtensions {
	// This extension method is broken out so you can use a similar pattern with
	// other MetaData elements in the future. This is your base method for each.
	public static Type? GetAttribute<Type>(this Enum value) where Type : Attribute {
		System.Type type = value.GetType();
		MemberInfo[] memberInfo = type.GetMember(value.ToString());
		Object[] attributes = memberInfo[default].GetCustomAttributes(typeof(Type), false);
		return attributes.Length > default(Int32) ? (Type)attributes[default] : null;
	}

	// This method creates a specific call to the above method, requesting the
	// Description MetaData attribute.
	public static String ToName(this Enum value) {
		DescriptionAttribute? attribute = value.GetAttribute<DescriptionAttribute>();
		return attribute is null ? value.ToString() : attribute.Description;
	}
}