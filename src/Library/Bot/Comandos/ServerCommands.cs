using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;

public class ServerCommands : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'join'. Este comando une al jugador que envía el
    /// mensaje a la lista de jugadores esperando para jugar y los empareja automáticamente si hay al menos dos.
    /// </summary>
    [Command("join")]
    [Summary("Une el usuario que envía el mensaje a la lista de espera y empareja si hay otro jugador disponible")]
    public async Task JoineAsync()
    {
        // Obtiene los 3 atributos necesarios para crear un objeto Entrenador:
        ulong userId = Context.User.Id;  // Obtener el ID del usuario
        string displayName = CommandHelper.GetDisplayName(Context); // Obtiene el nombre del usuario/Entrenador
        SocketGuildUser? user = CommandHelper.GetUser(Context, displayName);
        
        // Solamente en esta instancia se puede obtener el canal. Por ende, es Expert
        ICanal canal = new CanalDeDiscord(Context.Channel);

        // Agrega al jugador a la lista de espera usando el método existente, fachada no queda acoplada a la plataforma que se use.
        Fachada.Instance.AddTrainerToWaitingList(userId, displayName,user,canal);
    }
    
    /// <summary>
    /// Implementa el comando 'leave' del bot. Este comando remueve el jugador
    /// que envía el mensaje de la lista de jugadores esperando para jugar.
    /// </summary>
    [Command("leave")]
    [Summary("Remueve el usuario que envía el mensaje de la lista de espera")]
    // ReSharper disable once UnusedMember.Global
    public async Task LeaveAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        ICanal canal = new CanalDeDiscord(Context.Channel);
        
        // Elimina al jugador de la lista de espera usando el método existente, fachada no queda acoplada a la plataforma que se use.
        Fachada.Instance.RemoveTrainerFromWaitingList(displayName,canal);
    }
}
