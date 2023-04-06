using Humanizer;
using MassTransit;

namespace BuildingBlocks.Integration.MassTransit;
public sealed class CustomEntityNameFormatter : IEntityNameFormatter {
	public String FormatEntityName<Type>() {
		return typeof(Type).Name.Underscore();
	}
}