using Discord;

namespace Ucu.Poo.DiscordBot.Domain;

public interface IGestorUsuario
{
    void EnviarMensaje(string mensaje, IMessageChannel canal);
}
