using Azure.Messaging.ServiceBus;

namespace ServiceBusAPI.Messages.Sender;

public class ServicebusSenderService : IServicebusSenderService
{
    private IConfiguration _configuration;

    public ServicebusSenderService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMessage(string queueName, string message)
    {
        await using ServiceBusClient client = new ServiceBusClient(_configuration["ConnectionStrings:ServiceBus"]);

        ServiceBusSender sender = client.CreateSender(queueName);
        ServiceBusMessage serializedContents = new ServiceBusMessage(message);
        await sender.SendMessageAsync(serializedContents);
    }
}