using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Channels;

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

            try
            {
                var factory = new ConnectionFactory
                {
                   HostName = "localhost",
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "queue",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var mensagem = new Mensagem();

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
                     "0- Sair");

                    var menu = Console.ReadLine();
                
                    switch (menu)
                    {
                        case "0":
                            Console.WriteLine("Saindo...");
                            break;
                        case "1":
                            mensagem.Descricao = "Abrir Gaveta de Valores";
                            EnviarMensagem(mensagem, channel);
                        break;
                        case "2":
                            mensagem.Descricao = "Abrir Caixa";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "3":
                            mensagem.Descricao = "Aplicar Acréscimo Item";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "4":
                            mensagem.Descricao = "Aplicar Acréscimo no Total";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "5":
                            mensagem.Descricao = "Aplicar Desconto no Item";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "6":
                            mensagem.Descricao = "Aplicar Desconto no Total";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "7":
                            mensagem.Descricao = "Cancelar Item";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "8":
                            mensagem.Descricao = "Cancelamento de Venda";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "9":
                            mensagem.Descricao = "Consultar Saldo Crediário";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "10":
                            mensagem.Descricao = "Numeração Acabando";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "11":
                            mensagem.Descricao = "Modo Contingência";
                            EnviarMensagem(mensagem, channel);
                            break;
                        case "12":
                            mensagem.Descricao = "Fechar Caixa";
                            EnviarMensagem(mensagem, channel);
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

        private static void EnviarMensagem(Mensagem mensagem, IModel channel)
        {
            var json = JsonConvert.SerializeObject(mensagem);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: string.Empty,
            routingKey: "queue",
            basicProperties: null,
            body: body);

            Console.WriteLine($"Mensagem enviada: {mensagem.Descricao}");
        }
    }
}