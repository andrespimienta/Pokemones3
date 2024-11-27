namespace Library.Bot.Dominio;
public interface ICanal
{
    Task EnviarMensajeAsync(string mensaje);
}

