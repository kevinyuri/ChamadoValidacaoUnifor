using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace UserValidacaoUnifor.RabbitMQ
{
    public class RabbitMQService
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQService()
        {
            _factory = new ConnectionFactory
            {
                HostName = "191.235.32.169",  
                Port = 5672,
                UserName = "guest", 
                Password = "guest"
            };
        }

        public async Task InitializeAsync()
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.QueueDeclareAsync(queue: "user_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: "chamado_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            await Task.CompletedTask;
        }

        public async Task SendMessageAsync(string message)
        {
            if (_channel is null)
            {
                return;
            }

            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync("", "user_queue", body: body);
        }

        public async Task SendMessageChamadoAsync(string message)
        {
            if (_channel is null)
            {
                return;
            }

            var body = Encoding.UTF8.GetBytes(message);
            await _channel.BasicPublishAsync("", "chamado_queue", body: body);
        }
    }
}
