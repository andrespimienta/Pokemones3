using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;

public class JoinCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'join'. Este comando une al jugador que envía el
    /// mensaje a la lista de jugadores esperando para jugar y los empareja automáticamente si hay al menos dos.
    /// </summary>
    [Command("join")]
    [Summary("Une el usuario que envía el mensaje a la lista de espera y empareja si hay otro jugador disponible")]
    public async Task ExecuteAsync()
    {
        ulong userId = Context.User.Id;  // Obtener el ID del usuario
        string displayName = CommandHelper.GetDisplayName(Context);
        SocketGuildUser? user = CommandHelper.GetUser(Context, displayName);
        IMessageChannel canal = Context.Channel;

        // Agregar al jugador a la lista de espera usando tu método existente
        Fachada.Instance.AddTrainerToWaitingList(userId, displayName,user,canal);
        
    }
}
