using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto_Pokemones_I;
using System.Threading.Tasks;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Ucu.Poo.DiscordBot.Services;

public class CanalDeDiscord : ICanal
{
    private readonly IMessageChannel canalDiscord;

    public CanalDeDiscord(IMessageChannel canalDiscord)
    {
        this.canalDiscord = canalDiscord;
    }

    public async Task EnviarMensajeAsync(string mensaje)
    {
        await canalDiscord.SendMessageAsync(mensaje);
    }
}
