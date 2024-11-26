using Discord;

public class CanalDiscord : ICanal
{
    private readonly IMessageChannel canalDiscord;

    public CanalDiscord(IMessageChannel canalDiscord)
    {
        this.canalDiscord = canalDiscord;
    }

    public async Task EnviarMensajeAsync(string mensaje)
    {
        await canalDiscord.SendMessageAsync(mensaje);
    }
}
