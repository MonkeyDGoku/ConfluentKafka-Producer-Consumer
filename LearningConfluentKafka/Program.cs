// See https://aka.ms/new-console-template for more information
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;


CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, collection) =>
                {
                    collection.AddHostedService<KafkaProducerHostedService>();
                    collection.AddHostedService<KafkaConsumerHostedService>();

                });
                
}
    
public class KafkaProducerHostedService : IHostedService
{
    private readonly ILogger<KafkaProducerHostedService> _logger;
    private IProducer<Null, string> _producer;

    public KafkaProducerHostedService(ILogger<KafkaProducerHostedService> logger)
    {
        _logger = logger;
        var config = new ProducerConfig()
        {
            BootstrapServers = "localhost:9092"
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        for(var i=0; i < 10; i++)
        {
            var value = $"Hello World {i}";
            _logger.LogInformation(value);
            await _producer.ProduceAsync("users", new Message<Null, string>()
            {
                Value = value
            }, cancellationToken);
            _producer.Flush(TimeSpan.FromSeconds(10));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _producer?.Dispose();
        return Task.CompletedTask;
    }
}

public class KafkaConsumerHostedService : IHostedService
{
    private readonly ILogger<KafkaConsumerHostedService> _logger;
    private IConsumer<Null, string> _consumer;

    public KafkaConsumerHostedService(ILogger<KafkaConsumerHostedService> logger)
    {
        _logger = logger;
        var config = new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId ="foo"
            
        };
        _consumer = new ConsumerBuilder<Null, string>(config).Build();
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe("users");
        while (true)
        {
            var cr = _consumer.Consume(cancellationToken);
            _logger.LogInformation("Received: " + cr.Message.Value);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer?.Dispose();
        return Task.CompletedTask;
    }
}