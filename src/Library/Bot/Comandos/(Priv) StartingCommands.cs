using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Discord.Commands;
using Discord.WebSocket;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'name' del bot. Este comando retorna el
/// nombre de un Pokémon dado su identificador.
/// </summary>
public class StartingCommands : ModuleBase<SocketCommandContext>
{ 
    /// <summary>
    /// Muestra el catálogo de pokemones al jugador que envíe el comando.
    /// </summary>
    [Command("catalogo")]
    public async Task MostrarCatalogoAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        SocketGuildUser? user = CommandHelper.GetUser(Context, displayName);

        Fachada.Instance.ShowCatalog(user);
    }
    
    [Command("agregarPokemon")]
    public async Task AgregarPokemonAsync(string numerosIdentificadores)
    {
        ulong userId = Context.User.Id;
        Battle batalla = BattlesList.Instance.GetBattle(userId);
        Entrenador? entrenador = batalla.GetEntrenadorActual(userId); 

        if (entrenador == null)
        {
            await ReplyAsync("No se ha encontrado un entrenador asociado a tu cuenta. ¿Estás en medio de una batalla?");
            return;
        }

        // Obtener la lista actual de Pokémon seleccionados por el entrenador
        List<Pokemon> listaDePokemones = entrenador.seleccionPokemones;

        // Validar si ya ha seleccionado 6 Pokémon
        if (listaDePokemones.Count >= 6)
        {
            await ReplyAsync("Ya has seleccionado el máximo de 6 Pokémon permitidos para la batalla.");
            return;
        }

        // Separar los números identificadores por coma, eliminar espacios y asegurarse de que no haya duplicados.
        string[] identificadoresArray = numerosIdentificadores.Split(',')
        .Select(id => id.Trim()) // Eliminar los espacios alrededor de cada identificador
        .Where(id => !string.IsNullOrEmpty(id)) // Eliminar cualquier entrada vacía por si hay comas extra
        .ToArray();

        // Usamos HashSet para garantizar que los identificadores sean únicos
        HashSet<string> identificadoresUnicos = new HashSet<string>(identificadoresArray);

        // Lista para almacenar Pokémon que no se encontraron
        List<string> noEncontrados = new List<string>();
        List<string> pokemonesAgregados = new List<string>();

        // Recorrer los identificadores
        foreach (string numeroIdentificador in identificadoresUnicos)
        {
            // Verificar si el entrenador ya seleccionó ese Pokémon
            if (listaDePokemones.Any(p => p.NumeroIdentificador == numeroIdentificador))
            {
                await ReplyAsync($"Ya has seleccionado el Pokémon con identificador {numeroIdentificador}, elige otro.");
                continue;
            }

            // Buscar el Pokémon en el catálogo
            Pokemon? pokemon = LeerArchivo.EncontrarPokemon(numeroIdentificador);

            if (pokemon != null)
            {
                // Verificar si el Pokémon ya está en la lista del entrenador para evitar duplicados
                if (listaDePokemones.Contains(pokemon))
                {
                    await ReplyAsync($"Ya has añadido a {pokemon.GetNombre()} a tu equipo.");
                    return;
                }

                // Añadir el Pokémon al equipo del entrenador
                entrenador.AñadirASeleccion(pokemon);
                pokemonesAgregados.Add(pokemon.GetNombre());
            }
            else
            {
                // Si no se encontró el Pokémon, agregarlo a la lista de no encontrados
                noEncontrados.Add(numeroIdentificador);
            }
            // Enviar un solo mensaje con todos los Pokémon agregados
            if (pokemonesAgregados.Count > 0)
            {
                string agregadosMsg = string.Join(", ", pokemonesAgregados);
                await ReplyAsync($"Los siguientes Pokémon han sido añadidos a tu selección: {agregadosMsg}\n");
            }

            // Enviar mensaje de error si hay Pokémon que no se encontraron
            if (noEncontrados.Count > 0)
            {
                string noEncontradosMsg = string.Join(", ", noEncontrados);
                await ReplyAsync($"No se encontraron Pokémon con los identificadores: {noEncontradosMsg}.");
            }

            // Verificar si ya alcanzó el límite de 6 Pokémon
            if (listaDePokemones.Count >= 6)
            {
                await ReplyAsync("Has completado tu selección de 6 Pokémon. ¡Estás listo para la batalla!\n\n" +
                             "Pokémon disponibles en tu selección:\n" +
                             "**Elige uno de tus pokemones con el comando `!usar <numero identificador del pokemon de tu lista>`**\n");
            
                List<Pokemon> seleccionPokemones = entrenador.seleccionPokemones;
                StringBuilder respuesta = new StringBuilder();
                for (int i = 0; i < seleccionPokemones.Count; i++)
                {
                    respuesta.AppendLine($"{i + 1}. {seleccionPokemones[i].GetNombre()}");
                }
                await ReplyAsync(respuesta.ToString());
            }
        }
    }
    
    [Command("usar")]
    public async Task UsarPokemonAsync(int numero)
    {
        ulong userId = Context.User.Id;
        Battle batalla = BattlesList.Instance.GetBattle(userId);
        Entrenador? entrenador = batalla.GetEntrenadorActual(userId); 

        if (entrenador != null)
        {
            List<Pokemon> seleccionPokemones = entrenador.seleccionPokemones;

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
