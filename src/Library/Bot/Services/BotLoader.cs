using System.Reflection;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Library.Bot.Services
{
    public static class BotLoader
    {
        private static IBot? botInstance;
        private static DiscordSocketClient? discordClient;

        public static async Task LoadAsync()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddLogging(options =>
                {
                    options.ClearProviders();
                    options.AddConsole();
                })
                .AddSingleton<IConfiguration>(configuration)
                .AddScoped<IBot, Bot>()
                .BuildServiceProvider();

            try
            {
                botInstance = serviceProvider.GetRequiredService<IBot>();
                await botInstance.StartAsync(serviceProvider);

                // Asigna el cliente de Discord para uso posterior
                if (botInstance is Bot bot)
                {
                    discordClient = bot.client;

                    // Escuchar el evento Ready
                    discordClient.Ready += OnBotReady;
                }

                Console.WriteLine("Conectado a Discord. Presione 'q' para salir...");

                do
                {
                    var keyInfo = Console.ReadKey();

                    if (keyInfo.Key != ConsoleKey.Q) continue;

                    Console.WriteLine("\nFinalizado");
                    await botInstance.StopAsync();

                    return;
                } while (true);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Environment.Exit(-1);
            }
        }

        // Método que se ejecuta cuando el bot está listo
        public static async Task OnBotReady()
        {
            Console.WriteLine("Bot está listo. Enviando mensaje de bienvenida...");
            await SendWelcomeMessage();
        }

        public static async Task SendWelcomeMessage()
        {
            try
            {
                if (discordClient == null)
                {
                    Console.WriteLine("El cliente de Discord no está inicializado.");
                    return;
                }

                string bienvenida = @"
¡Bienvenidos al servidor de batalla de Pokémon! 🎮✨

Aún no hay jugadores disponibles para comenzar la batalla, pero no te preocupes, ¡todos pueden unirse!

Para unirte a la lista de espera y encontrar un oponente, simplemente escribe el comando `!join` y espera a ser emparejado con otro entrenador.

Aquí están los pasos para participar:

1. **Unirte a la lista de espera**: Usa el comando `!join` para ser añadido a la lista de espera.

2. **Esperar un oponente**:
   - Usa el comando `!battle` para emparejarte automáticamente con cualquier otro jugador que esté esperando.
   - Si prefieres elegir un oponente específico, usa `!waitingList` para ver todos los jugadores en espera, y luego usa `!battle <nombreOponente>` para desafiar a alguien en particular.

¡Buena suerte y que gane el mejor entrenador! 💥";


                // Reemplaza con el ID de tu canal de texto
                ulong channelId = 1301322498348159028;

                var channel = discordClient.GetChannel(channelId) as IMessageChannel;

                if (channel != null)
                {
                    await channel.SendMessageAsync(bienvenida);
                    Console.WriteLine("Mensaje de bienvenida enviado.");
                }
                else
                {
                    Console.WriteLine("No se pudo encontrar el canal.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el mensaje de bienvenida: {ex.Message}");
            }
        }
    }
}



