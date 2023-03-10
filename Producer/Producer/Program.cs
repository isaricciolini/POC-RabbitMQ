using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Producer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                 .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                 .CreateLogger();
            logger.Information(
                "Testando o envio de mensagens para uma Fila do RabbitMQ");


            string queueName = "FilaIsa";
            string json;

            logger.Information($"Queue = {queueName}");

            try
            {
                var factory = new ConnectionFactory()
                {
                    Uri = new Uri("amqps://bnyfjfyo:jjgYCL6Oxw2ZSQ6I5ajpuOVkKI-65knk@jackal.rmq.cloudamqp.com/bnyfjfyo"),
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var mensagem = new Mensagem();
                mensagem.NumeroPDV = 1;
                mensagem.Operador = "Isabelle";

                    logger.Information(
                     "Escolha uma opção abaixo: \n " +
                     "1- Abrir gaveta \n " +
                     "2- Abertura de Caixa \n " +
                     "3- Acréscimo Item \n " +
                     "4- Acréscimo no Total \n " +
                     "5- Desconto Item \n " +
                     "6- Desconto Total \n " +
                     "7- Cancelar Item \n " +
                     "8- Cancelar Venda \n " +
                     "9- Consultar Saldo Crediário \n " +
                     "10- Acabando Numeração \n " +
                     "11- Modo Contingência \n " +
                     "12- Fechamento de Caixa \n" +
                     ". - Enviar \n" +
                     "0- Sair");

                    var menu = Console.ReadLine();

                    switch (menu)
                    {
                        case "0":
                            Console.WriteLine("Saindo...");
                            break;
                        case "1":
                            mensagem.Descricao = "Abrir Gaveta de Valores";
                            break;
                        case "2":
                            mensagem.Descricao = "Abrir Caixa";
                            break;
                        case "3":
                            mensagem.Descricao = "Aplicar Acréscimo Item";
                            break;
                        case "4":
                            mensagem.Descricao = "Aplicar Acréscimo no Total";
                            break;
                        case "5":
                            mensagem.Descricao = "Aplicar Desconto no Item";
                            break;
                        case "6":
                            mensagem.Descricao = "Aplicar Desconto no Total";
                            break;
                        case "7":
                            mensagem.Descricao = "Cancelar Item";
                            break;
                        case "8":
                            mensagem.Descricao = "Cancelamento de Venda";
                            break;
                        case "9":
                            mensagem.Descricao = "Consultar Saldo Crediário";
                            break;
                        case "10":
                            mensagem.Descricao = "Numeração Acabando";
                            break;
                        case "11":
                            mensagem.Descricao = "Modo Contingência";
                            break;
                        case "12":
                            mensagem.Descricao = "Fechar Caixa";
                            break;
                        case ".":
                            json = JsonConvert.SerializeObject(mensagem);

                            channel.BasicPublish(exchange: string.Empty,
                            routingKey: "routingkey",
                            basicProperties: null,
                            body: Encoding.UTF8.GetBytes(json));

                            Console.WriteLine("[x] Enviado {0}", mensagem);
                            break;
                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }
            }
            catch (Exception ex)
            {
                logger.Error($"Exceção: {ex.GetType().FullName} | " +
                             $"Mensagem: {ex.Message}");
            }
        }
    }
}