using System.Text.Json;
using Confluent.Kafka;

public class ConsumeEvents : IHostedService
{
    private readonly string topic="test";
    private readonly string groupId="test_group";
    private readonly string bootstrapServers="localhost:9092";
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var config=new ConsumerConfig{
            GroupId=groupId,
            BootstrapServers=bootstrapServers,
            AutoOffsetReset=AutoOffsetReset.Earliest
        };
        try{
           using(var consumerBuilder = new ConsumerBuilder<Ignore,string>(config).Build()){
            consumerBuilder.Subscribe(topic);
            var cancelToken=new CancellationTokenSource();
            try{
                  while(true)
                  {
                    var consumer=consumerBuilder.Consume(cancelToken.Token);
                    var orderRequest=JsonSerializer.Deserialize<Order>(consumer.Message.Value);
                    Console.WriteLine(orderRequest.OrderID);
                    Console.ReadKey();
                  }
            }
            catch(Exception e)
            {
              consumerBuilder.Close();
            }
           }
        }catch(Exception e){

        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}