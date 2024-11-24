using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'leave' del bot. Este comando remueve el
/// jugador que envía el mensaje de la lista de jugadores esperando para jugar.
/// </summary>
// ReSharper disable once UnusedType.Global
public class LeaveCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'leave' del bot. Este comando remueve el jugador
    /// que envía el mensaje de la lista de jugadores esperando para jugar.
    /// </summary>
    [Command("leave")]
    [Summary("Remueve el usuario que envía el mensaje de la lista de espera")]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync()
    {
        ulong userId = Context.User.Id;  // Obtener el ID del usuario
        string displayName = CommandHelper.GetDisplayName(Context);
        ICanal canal = new CanalDeDiscord(Context.Channel);// Solamente en esta instancia se puede obtener el canal. Por ende, es Expert
        
        // Eliminar al jugador de la lista de espera usando tu método existente, fachada no queda acoplada a la plataforma que se use.
        Fachada.Instance.RemoveTrainerFromWaitingList(displayName,canal);
    }
}