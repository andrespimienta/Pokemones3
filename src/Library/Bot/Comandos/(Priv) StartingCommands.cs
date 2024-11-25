using Discord.Commands;
using Discord.WebSocket;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;
using Ucu.Poo.DiscordBot.Commands;

public class StartingCommands : ModuleBase<SocketCommandContext>
{ 
    /// <summary>
    /// Muestra el catálogo de pokemones al jugador que envíe el comando.
    /// </summary>
    [Command("catalogo")]
    public async Task MostrarCatalogoAsync()
    {
        ulong userId = Context.User.Id;  // Obtener el ID del usuario
        
        Fachada.Instance.ShowCatalog(userId);
    }
    
    [Command("agregarPokemon")]
    public async Task AgregarPokemonAsync(string numerosIdentificadores)
    {
        ulong userId = Context.User.Id;
        
        Fachada.Instance.AddPokemonToList(userId, numerosIdentificadores);
    }
    
    [Command("usar")]
    public async Task UsarPokemonAsync(int numero)
    {
        ulong userId = Context.User.Id;
        Battle batalla = BattlesList.Instance.GetBattle(userId);
        Entrenador? entrenador = batalla.GetEntrenadorActual(userId); 

        if (entrenador != null)
        {
            List<Pokemon> seleccionPokemones = entrenador.GetSeleccion();

            // Validar si el número ingresado es válido
            if (numero < 1 || numero > seleccionPokemones.Count)
            {
                await ReplyAsync($"Por favor, ingresa un número válido entre 1 y {seleccionPokemones.Count}.");
                return;
            }

            // Seleccionar el Pokémon basado en el número ingresado
            Pokemon pokemonSeleccionado = seleccionPokemones[numero - 1];

            // Usar el Pokémon seleccionado
            entrenador.pokemonEnUso = pokemonSeleccionado;

            await ReplyAsync($"¡Has elegido a {pokemonSeleccionado.GetNombre()} para la batalla!\n"+
                              "**Indica que estás listo para la batalla: Usa el comando `!startBattle` para confirmar que estás listo para luchar.**");
        }
        else
        {
            await ReplyAsync("No se ha encontrado un entrenador asociado a tu cuenta.");
        }
    }
}
