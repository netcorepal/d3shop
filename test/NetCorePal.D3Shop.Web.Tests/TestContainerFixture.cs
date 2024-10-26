using Testcontainers.MySql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace NetCorePal.D3Shop.Web.Tests;

public class TestContainerFixture : IDisposable
{
    public RedisContainer RedisContainer { get; } =
        new RedisBuilder().Build();

    public RabbitMqContainer RabbitMqContainer { get; } = new RabbitMqBuilder()
        .WithUsername("guest").WithPassword("guest").Build();

    public MySqlContainer MySqlContainer { get; } = new MySqlBuilder()
        .WithUsername("root").WithPassword("123456")
        .WithEnvironment("TZ", "Asia/Shanghai")
        .WithDatabase("demo").Build();

    public TestContainerFixture()
    {
        Task.WhenAll(RedisContainer.StartAsync(),
            RabbitMqContainer.StartAsync(),
            MySqlContainer.StartAsync()).Wait();
    }

    public void Dispose()
    {
        Task.WhenAll(RedisContainer.StopAsync(),
            RabbitMqContainer.StopAsync(),
            MySqlContainer.StopAsync()).Wait();
    }
}