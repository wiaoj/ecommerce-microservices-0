namespace BuildingBlocks.Core.Types.Extensions;
public static class DateTimeExtensions {
	private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	public static Int64 ToUnixTimeSecond(this DateTime datetime) {
		Double unixTime = (datetime.ToUniversalTime() - Epoch).TotalSeconds;
		return (Int64)unixTime;
	}

	public static DateTime ToDateTime(this Int64? unixTime) {
		return Epoch.AddSeconds(unixTime ?? ToUnixTimeSecond(DateTime.Now)).ToLocalTime();
	}

	public static Int64 ToUnixTimeMilliseconds(this DateTime dateTime) {
		return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
	}
}