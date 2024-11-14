using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;

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
        
        // Buscar al oponente por nombre de usuario
        SocketGuildUser? opponentUser = CommandHelper.GetUser(
            Context, opponentDisplayName);

        string result;
        if (opponentUser != null)
        {
            // Si el oponente está en la lista de espera, se inicia la batalla
            var battleResult = Fachada.Instance.StartBattle(displayName, opponentUser.DisplayName);
            
            // Informamos al usuario y al oponente que la batalla ha comenzado
            await Context.Message.Author.SendMessageAsync(battleResult);
            await opponentUser.SendMessageAsync(battleResult);

            // Enviar las instrucciones más prolijas
            string instrucciones = @"¡La batalla ha comenzado! Ahora es el momento de elegir tus Pokémon.

Para participar en la batalla, debes elegir 6 Pokémon de tu catálogo. Aquí están los pasos:

Ver el catálogo de Pokémon: Utiliza el comando !catalogo para ver todos los Pokémon disponibles.
Elegir un Pokémon: Una vez que hayas decidido qué Pokémon quieres en tu equipo, usa el comando !agregarPokemon <nombreDelPokemon> para añadirlo a tu selección.
Elegir el Pokémon para la batalla: Una vez que hayas seleccionado tus 6 Pokémon, utiliza el comando !elegirPokemon <nombreDelPokemon> para elegir qué Pokémon utilizar en la batalla.
Recuerda que necesitarás elegir 6 Pokémon antes de que empiece la batalla. ¡Buena suerte y que gane el mejor entrenador!

¡Que empiece la competencia!";

            // Enviar mensaje a ambos jugadores
            await Context.Message.Author.SendMessageAsync(instrucciones);
            await opponentUser.SendMessageAsync(instrucciones);
        }
        else
        {
            result = $"No se ha encontrado un usuario con el nombre `{opponentDisplayName}` en la lista de espera. Asegúrate de que el nombre esté correctamente escrito.";
            await ReplyAsync(result);
        }
    }
}
