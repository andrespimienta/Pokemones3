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

        // Agregar al jugador a la lista de espera usando tu método existente
        string result = Fachada.Instance.AddTrainerToWaitingList(userId, displayName);
        await ReplyAsync(result);

        // Comprobar si hay al menos dos jugadores en la lista de espera
        ListaDeEspera ListaDeEspera = Fachada.Instance.ListaDeEspera;

        if (ListaDeEspera.Count >= 2)
        {
            // Emparejar a los dos primeros jugadores de la lista de espera
            Entrenador player1 = ListaDeEspera.entrenadores[0];
            Entrenador player2 = ListaDeEspera.entrenadores[1];

            // Iniciar la batalla entre los dos jugadores
            string battleResult = Fachada.Instance.CrearBatalla(player1.GetNombre(), player2.GetNombre());

            // Notificar a ambos jugadores que han sido emparejados
            var user1 = Context.Guild.GetUser(player1.Id);
            var user2 = Context.Guild.GetUser(player2.Id);

            if (user1 != null && user2 != null)
            {
                await user1.SendMessageAsync($"¡Has sido emparejado con {player2.GetNombre()} para la batalla! 🎮");
                await user2.SendMessageAsync($"¡Has sido emparejado con {player1.GetNombre()} para la batalla! 🎮");
                await user1.SendMessageAsync(battleResult);
                await user2.SendMessageAsync(battleResult);

                // Enviar las instrucciones para la batalla
                string instrucciones = @"¡La batalla ha comenzado! Ahora es el momento de elegir tus Pokémon.

Para participar en la batalla, debes elegir 6 Pokémon de tu catálogo. Aquí están los pasos:

1. **Ver el catálogo de Pokémon**: Usa el comando `!catalogo` para ver todos los Pokémon disponibles.
2. **Elegir un Pokémon**: Usa el comando `!agregarPokemon <nombreDelPokemon>` para añadirlo a tu equipo.
3. **Elegir el Pokémon para la batalla**: Una vez que hayas seleccionado tus 6 Pokémon, utiliza el comando `!elegirPokemon <nombreDelPokemon>` para elegir tu Pokémon inicial.

¡Buena suerte y que gane el mejor entrenador! 💥";

                await user1.SendMessageAsync(instrucciones);
                await user2.SendMessageAsync(instrucciones);
            }
        }
    }
}
