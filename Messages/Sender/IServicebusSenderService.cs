namespace ServiceBusAPI.Messages.Sender;

public interface IServicebusSenderService
{
    Task SendMessage(string queueName, string message);
}