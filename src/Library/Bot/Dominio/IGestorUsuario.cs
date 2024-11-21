namespace Ucu.Poo.DiscordBot.Domain;

public interface IGestorUsuario
{
    public void EnviarMensaje(string mensaje);

    public string RecibirMensaje();
}