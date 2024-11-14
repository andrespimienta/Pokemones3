using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;

//using Ucu.Poo.DiscordBot.Domain;

namespace Proyecto_Pokemones_I.Bot.Comandos;

/// <summary>
/// Esta clase implementa el comando 'battle' del bot. Este comando une al
/// jugador que envía el mensaje con el oponente que se recibe como parámetro,
/// si lo hubiera, en una batalla; si no se recibe un oponente, lo une con
/// cualquiera que esté esperando para jugar.
/// </summary>
// ReSharper disable once UnusedType.Global
public class BattleCommand : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'battle'. Este comando une al jugador que envía el
    /// mensaje a la lista de jugadores esperando para jugar.
    /// </summary>
    [Command("battle")]
    [Summary(
        """
        Une al jugador que envía el mensaje con el oponente que se recibe
        como parámetro, si lo hubiera, en una batalla; si no se recibe un
        oponente, lo une con cualquiera que esté esperando para jugar.
        """)]
    // ReSharper disable once UnusedMember.Global
    public async Task ExecuteAsync(
        [Remainder]
        [Summary("Display name del oponente, opcional")]
        string? opponentDisplayName = null)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        
        SocketGuildUser? opponentUser = CommandHelper.GetUser(
            Context, opponentDisplayName);

        string result;
        if (opponentUser != null)
        {
            result = Ucu.Poo.DiscordBot.Domain.Fachada.Instance.StartBattle(displayName, opponentUser.DisplayName);
            await Context.Message.Author.SendMessageAsync(result);
            await opponentUser.SendMessageAsync(result);
        }
        else
        {
            result = $"No hay un usuario {opponentDisplayName}";
        }

        await ReplyAsync(result);
    }
}
