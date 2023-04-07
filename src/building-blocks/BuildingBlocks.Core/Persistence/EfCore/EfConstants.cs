namespace BuildingBlocks.Core.Persistence.EfCore;
public static class EfConstants {
	public const String UuidGenerator = "uuid-ossp";
	public const String UuidAlgorithm = "uuid_generate_v4()";
	public const String DateAlgorithm = "now() at time zone('utc')";

	public static class ColumnTypes {
		public const String PriceDecimal = "decimal(18,2)";
		public const String TinyText = "varchar(15)";
		public const String ShortText = "varchar(25)";
		public const String NormalText = "varchar(50)";
		public const String MediumText = "varchar(100)";
		public const String LongText = "varchar(250)";
		public const String ExtraLongText = "varchar(500)";
	}

	public static class Lenght {
		public const Int32 Tiny = 15;
		public const Int32 Short = 25;
		public const Int32 Normal = 50;
		public const Int32 Medium = 50;
		public const Int32 Long = 250;
		public const Int32 ExtraLong = 500;
	}
}