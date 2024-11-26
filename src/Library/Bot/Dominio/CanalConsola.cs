namespace Ucu.Poo.DiscordBot.Domain;

public class CanalConsola : ICanal
{
    private static CanalConsola? _instance;

    public static CanalConsola Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CanalConsola();
            }
            return _instance;
        }
    }
    private CanalConsola()
    {
    }

    public async Task EnviarMensajeAsync(string mensaje)
    {
        Console.WriteLine(mensaje);
    }
}