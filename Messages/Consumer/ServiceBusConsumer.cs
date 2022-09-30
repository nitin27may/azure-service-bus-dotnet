using Azure.Messaging.ServiceBus;
using ServiceBusAPI.Models;
using ServiceBusAPI.Services;

namespace ServiceBusAPI.Messages.Consumer
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _configuration;
        private readonly ServiceBusClient _client;
        private readonly ILogger _logger;
        private ServiceBusProcessor _processor;

        public ServiceBusConsumer(INotificationService notificationService,
            IConfiguration configuration,
            ILogger<ServiceBusConsumer> logger)
        {
            _notificationService = notificationService;
            _configuration = configuration;
            _logger = logger;

            var connectionString = _configuration["ConnectionStrings:ServiceBus"];
            _client = new ServiceBusClient(connectionString);
        }

        public async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,

            };

            _processor = _client.CreateProcessor(_configuration["ConnectionStrings:QueueName"], _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;
            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<Notification>();
            await _notificationService.SendNotficaiton(myPayload).ConfigureAwait(false);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor != null)
            {
                await _processor.DisposeAsync().ConfigureAwait(false);
            }

            if (_client != null)
            {
                await _client.DisposeAsync().ConfigureAwait(false);
            }
        }

        public async Task CloseQueueAsync()
        {
            await _processor.CloseAsync().ConfigureAwait(false);
        }
    }
}
