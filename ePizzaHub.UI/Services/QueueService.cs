using Azure.Messaging.ServiceBus;
using ePizzaHub.UI.Interfaces;
using System.Text.Json;

namespace ePizzaHub.UI.Services
{
    public class QueueService : IQueueService
    {
        private ServiceBusClient _serviceBusClient;
        private IConfiguration _configuration;
        private ServiceBusSender _sender;
        private IKeyVaultService _keyVaultService;
        private IWebHostEnvironment _env;
        public QueueService(IConfiguration configuration, IKeyVaultService keyVaultService, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _keyVaultService = keyVaultService;
            _env = env;

            //for appsettings.json
            if (_env.IsDevelopment())
            {
                _serviceBusClient = new ServiceBusClient(_configuration["ConnectionStrings:ServiceBus"]);
            }
            else
            {
                var ServiceBus = _keyVaultService.GetSecret("ServiceBusConnection").Result;
                _serviceBusClient = new ServiceBusClient(ServiceBus);
            }
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            _sender = _serviceBusClient.CreateSender(queueName);
            string messageBody = JsonSerializer.Serialize(serviceBusMessage);
            var message = new ServiceBusMessage(messageBody);

            await _sender.SendMessageAsync(message);
        }
    }
}