using System.Net;
using Confluent.Kafka;

public class ProduceEvents
{
   private readonly string bootstrapServers="localhost:9092";
   private readonly string topic="test";
   public async Task<bool> SendOrderRequest(string topic,string message)
   {
    ProducerConfig config=new ProducerConfig{
        BootstrapServers=bootstrapServers,
        ClientId=Dns.GetHostName()
    };
    try{
        using (var producer =new ProducerBuilder<Null,string>(config).Build()){
            var result= await producer.ProduceAsync(topic,new Message<Null, string>{
                Value=message
            });
            return await Task.FromResult(true);
        }
    }catch(Exception e){
       Console.WriteLine("Error occured");
       Console.ReadKey();
    }
    return await Task.FromResult(false);
   }
}