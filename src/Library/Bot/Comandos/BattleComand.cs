using Discord.Commands;
using Discord.WebSocket;
using Proyecto_Pokemones_I;
using System.Threading.Tasks;
using Discord;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class BattleCommand : ModuleBase<SocketCommandContext>
    {
        // Instancia de la lista de batallas
        private static BattlesList battlesList = BattlesList.Instance;
        private static int entrenadoresListos = 0;

        [Command("StartBattle")]
        public async Task BattleAsync()
        {
            // Obtener el ID del jugador actual usando su ID de Discord
            ulong usuarioId = Context.User.Id;
            Battle batalla = battlesList.GetBattle(usuarioId);

            Entrenador? entrenador = battlesList.ObtenerEntrenadorPorUsuario(usuarioId);

            if (entrenador == null)
            {
                await ReplyAsync("No estás en ninguna batalla activa.");
                return;
            }

            // Si el jugador ya está marcado como listo, no se incrementa el contador ni se hace nada más
            if (entrenador.EstaListo)
            {
                await ReplyAsync("Ya estás marcado como listo para la batalla.");
                return; // Sale del método si ya está listo
            }

            // Marcar al entrenador como listo y aumentar el contador
            entrenador.EstaListo = true;
            entrenadoresListos++;

            await ReplyAsync($"{Context.User.Username} está listo para la batalla.");

            // Comprobar si ambos jugadores están listos
            if (batalla.EstanListos() == true)
            {
                Entrenador player1 = batalla.Player1;
                Entrenador player2 = batalla.Player2;
                IniciarBatallaAsync(batalla, player1, player2);
                //entrenadoresListos = 0; // Resetear el contador de listos después de iniciar la batalla
            }
            else
            {
                // Si solo uno está listo, esperar al oponente
                await ReplyAsync("Esperando a que tu oponente esté listo...");
            }
        }

        private async Task IniciarBatallaAsync(Battle batalla, Entrenador player1, Entrenador player2)
        {
            // Obtener el ID del jugador actual usando su ID de Discord

            // Entrenador player1 = batalla.Player1;
            // Entrenador player2 = batalla.Player2;

            var user1 = Context.Guild.GetUser(player1.Id);
            var user2 = Context.Guild.GetUser(player2.Id);

            await user2.SendMessageAsync("¡Ambos jugadores están listos! Comenzando la batalla...");
            await user1.SendMessageAsync("¡Ambos jugadores están listos! Comenzando la batalla...");

            await ChequearQuienEmpieza(batalla);
        }

        private async Task ChequearQuienEmpieza(Battle batalla)
        {
            var user1 = Context.Guild.GetUser(batalla.Player1.Id);
            var user2 = Context.Guild.GetUser(batalla.Player2.Id);

            Pokemon pokemonJugador1 = batalla.Player1.GetPokemonEnUso();
            Pokemon pokemonJugador2 = batalla.Player2.GetPokemonEnUso();

            string turnoJugador;

            // Comparar la velocidad de los Pokémon para determinar quién empieza
            if (pokemonJugador1.GetVelocidadAtaque() > pokemonJugador2.GetVelocidadAtaque())
            {
                turnoJugador = batalla.Player1.GetNombre();
            }
            else if (pokemonJugador2.GetVelocidadAtaque() > pokemonJugador1.GetVelocidadAtaque())
            {
                turnoJugador = batalla.Player2.GetNombre();
            }
            else
            {
                // Si la velocidad es igual, se elige al azar
                turnoJugador = new System.Random().Next(2) == 0
                    ? batalla.Player1.GetNombre()
                    : batalla.Player2.GetNombre();
            }

            // Notificar a ambos jugadores sobre quién empieza
            if (turnoJugador == batalla.Player1.GetNombre())
            {
                await user2.SendMessageAsync(
                    $"{batalla.Player2.GetNombre()}, tu oponente {batalla.Player1.GetNombre()} ha elegido {pokemonJugador1.GetNombre()} y comenzará con el turno.");
                await user1.SendMessageAsync(
                    $"{batalla.Player1.GetNombre()}, es tu turno.\nTu oponente está usando {pokemonJugador2.GetNombre()}.");

                // Mostrar opciones solo al jugador que tiene el turno
                await MostrarOpciones(user1);
            }
            else
            {
                await user1.SendMessageAsync(
                    $"{batalla.Player1.GetNombre()}, tu oponente {batalla.Player2.GetNombre()} ha elegido {pokemonJugador2.GetNombre()} y comenzará con el turno.");
                await user2.SendMessageAsync(
                    $"{batalla.Player2.GetNombre()}, es tu turno.\nTu oponente está usando {pokemonJugador1.GetNombre()}.");

                // Mostrar opciones solo al jugador que tiene el turno
                await MostrarOpciones(user2);
            }
        }

        private async Task MostrarOpciones(SocketGuildUser jugador)
        {
            await jugador.SendMessageAsync("Elige una acción:\n" +
                                           "(1) Atacar\n" +
                                           "(2) Cambiar de Pokémon\n" +
                                           "(3) Usar poción\n" +
                                           "(4) Rendirse");
        }


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
            [Remainder] [Summary("Display name del oponente, opcional")]
            string? opponentDisplayName = null)
        {
            string displayName = CommandHelper.GetDisplayName(Context);

            SocketGuildUser? opponentUser = CommandHelper.GetUser(
                Context, opponentDisplayName);

            string result;
            if (opponentUser != null)
            {
                result = Fachada.Instance.StartBattle(displayName, opponentUser.DisplayName);
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
}


