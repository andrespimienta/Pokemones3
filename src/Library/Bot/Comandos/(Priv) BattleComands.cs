using Discord.Commands;
using Discord.WebSocket;
using Proyecto_Pokemones_I;
using System.Threading.Tasks;
using Discord;
using Ucu.Poo.DiscordBot.Domain;



namespace Ucu.Poo.DiscordBot.Commands
{
    public class BattleCommands : ModuleBase<SocketCommandContext>
    {
        // Instancia de la lista de batallas
        private static BattlesList battlesList = BattlesList.Instance;
        private static int entrenadoresListos = 0;

        [Command("StartBattle")] 
        public async Task BattleAsync()
        {
            // Obtener el ID del jugador actual usando su ID de Discord
            ulong usuarioId = Context.User.Id;
            
            //============================================================================================
            // ***** DE ACÁ PARA ABAJO ESTÁ MAL, DEBERÍA IMPLEMENTARLO FACHADA CON SU PROPIO MÉTODO. *****
            // ********* PERO ANTES DEBERÍA QUEDAR PRONTA LA CLASE GESTORA/IMPRESORA DE MENSAJES, ********
            // ********************** PORQUE EL MÉTODO EN FACHADA ENVIARÍA MENSAJES **********************
            
            Battle batalla = battlesList.GetBattle(usuarioId);
            Entrenador? entrenador = batalla.GetEntrenadorActual(usuarioId);

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
                IniciarBatallaAsync(batalla);
                //entrenadoresListos = 0; // Resetear el contador de listos después de iniciar la batalla
            }
            else
            {
                // Si solo uno está listo, esperar al oponente
                await ReplyAsync("Esperando a que tu oponente esté listo...");
            }
            // ********************** (HASTA ACÁ DEBERÍA SER UN MÉTODO DE FACHADA) **********************
            //===========================================================================================
        }

        private async Task IniciarBatallaAsync(Battle batalla)
        {
            // Obtener el ID del jugador actual usando su ID de Discord
            Console.WriteLine("Obteniendo usuario..1.");
            var user1 = batalla.Player1.GetSocketGuildUser();
            Console.WriteLine("Usuario obtenido.1");

            Console.WriteLine("Obteniendo usuario...2");
            var user2 = batalla.Player2.GetSocketGuildUser();
            Console.WriteLine("Usuario obtenido.2");

            await user1.SendMessageAsync("¡Ambos jugadores están listos! Comenzando la batalla...");
            await user2.SendMessageAsync("¡Ambos jugadores están listos! Comenzando la batalla...");

            await ChequearQuienEmpieza(batalla);
        }

        private async Task ChequearQuienEmpieza(Battle batalla)
        {
            Console.WriteLine("Obteniendo usuario..1.");
            var user1 = batalla.Player1.GetSocketGuildUser();
            Console.WriteLine("Usuario obtenido.2");

            Console.WriteLine("Obteniendo usuario...2");
            var user2 = batalla.Player2.GetSocketGuildUser();
            Console.WriteLine("Usuario obtenido.2");

            Pokemon pokemonJugador1 = batalla.Player1.GetPokemonEnUso();
            Pokemon pokemonJugador2 = batalla.Player2.GetPokemonEnUso();

            string turnoJugador;

            // Comparar la velocidad de los Pokémon para determinar quién empieza
            if (pokemonJugador1.GetVelocidadAtaque() > pokemonJugador2.GetVelocidadAtaque())
            {
                turnoJugador = batalla.Player1.GetNombre();
                batalla.EntrenadorConTurno = batalla.Player1;
            }
            else if (pokemonJugador2.GetVelocidadAtaque() > pokemonJugador1.GetVelocidadAtaque())
            {
                turnoJugador = batalla.Player2.GetNombre();
                batalla.EntrenadorConTurno = batalla.Player2;
            }
            else
            {
                // Si la velocidad es igual, se elige al azar
                turnoJugador = new System.Random().Next(2) == 0
                    ? batalla.Player1.GetNombre()
                    : batalla.Player2.GetNombre();
                if (turnoJugador == batalla.Player1.GetNombre())
                {
                    batalla.EntrenadorConTurno = batalla.Player1;
                }
                else
                {
                    batalla.EntrenadorConTurno = batalla.Player2;
                }
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
                await MostrarOpciones(user1);
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

        [Command("Atacar")]
        public async Task atacar([Remainder] [Summary("Si no es null, usa dicho ataque. De lo contrario muesta la lista de ataques.")]
            string? attackName = null)
        {
            ulong usuarioId = Context.User.Id;

            if (attackName == null)
            {
                string? aux = Fachada.Instance.ListaAtaques(usuarioId);
            
                await Context.Message.Author.SendMessageAsync(aux);
            }
            else
            {
                string? aux=Fachada.Instance.Atacar(usuarioId,attackName);
                await Context.Message.Author.SendMessageAsync(aux);
            }

        }

    }
}




