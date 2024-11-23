using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto_Pokemones_I;
using System.Threading.Tasks;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;
using Ucu.Poo.DiscordBot.Services;


public class GestorDiscord : ModuleBase<SocketCommandContext>, IGestorUsuario
{
    // Singleton
    private static GestorDiscord instance;
    private ulong? usuarioId;

    public static GestorDiscord Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GestorDiscord();
            }
            return instance;
        }
    }
    private GestorDiscord()
    {
        this.usuarioId = null;
    }
    
    // En realidad no sirve, porque si fachada acepta IGestorUsuario, este método no está en la interfaz, es exclusivo de Discord
   
    public async void EnviarMensaje(string mensaje,IMessageChannel canal)
    {
        // Enviar el mensaje al canal
        await canal.SendMessageAsync(mensaje);
    }

}