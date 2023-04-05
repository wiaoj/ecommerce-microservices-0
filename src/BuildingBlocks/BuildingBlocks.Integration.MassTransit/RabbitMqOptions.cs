namespace BuildingBlocks.Integration.MassTransit;
public sealed class RabbitMqOptions {
	public String Host { get; init; } = "localhost";
	public String UserName { get; init; } = "guest";
	public String Password { get; init; } = "guest";
	public String ConnectionString => $"amqp://{this.UserName}:{this.Password}@{this.Host}/";
}