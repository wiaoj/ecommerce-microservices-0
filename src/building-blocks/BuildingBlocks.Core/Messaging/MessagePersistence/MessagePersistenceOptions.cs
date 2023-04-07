namespace BuildingBlocks.Core.Messaging.MessagePersistence;
public class MessagePersistenceOptions {
	public Int32? Interval { get; set; } = 30;
	public String ConnectionString { get; set; } = default!;
	public Boolean Enabled { get; set; } = true;
	public String? MigrationAssembly { get; set; } = null!;
}