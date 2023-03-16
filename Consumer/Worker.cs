using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int _intervaloMensagemWorkerAtivo;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration)
        {

            _logger = logger;
            _intervaloMensagemWorkerAtivo =
                Convert.ToInt32(configuration["IntervaloMensagemWorkerAtivo"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"{DateTime.Now} - Aguardando mensagens...");

            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "queue",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Mensagem recebida: {message}");
            };

            channel.BasicConsume(queue: "queue",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Pressione ENTER to exit.");
            Console.ReadLine();

            //while (!stoppingToken.IsCancellationRequested)
            //{
            //_logger.LogInformation(
            //    $"Worker ativo em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            //await Task.Delay(_intervaloMensagemWorkerAtivo, stoppingToken);
            //}
        }

        private void Consumer_Received(
            object sender, BasicDeliverEventArgs e)
        {
            _logger.LogInformation(
                $"[Nova mensagem | {DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                Encoding.UTF8.GetString(e.Body.ToArray()));
        }
    }
}
