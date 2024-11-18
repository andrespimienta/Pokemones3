using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Discord.Commands;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;

namespace Ucu.Poo.DiscordBot.Commands;

/// <summary>
/// Esta clase implementa el comando 'name' del bot. Este comando retorna el
/// nombre de un Pokémon dado su identificador.
/// </summary>
// ReSharper disable once UnusedType.Global
public class PokemonNameCommand : ModuleBase<SocketCommandContext>
{ 
    [Command("agregarPokemon")]
public async Task AgregarPokemonAsync(string numerosIdentificadores)
{
    // Obtener el entrenador del usuario actual
    Entrenador? entrenador = BattlesList.Instance.ObtenerEntrenadorPorUsuario(Context.User.Id); 

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

    // Separar los números identificadores por coma y eliminar espacios
    string[] identificadoresArray = numerosIdentificadores.Split(',');
    HashSet<string> identificadoresUnicos = new HashSet<string>(identificadoresArray.Select(id => id.Trim()));

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
            // Añadir el Pokémon al equipo del entrenador
            entrenador.AñadirASeleccion(pokemon);
            pokemonesAgregados.Add(pokemon.GetNombre());
        }
        else
        {
            // Si no se encontró el Pokémon, agregarlo a la lista de no encontrados
            noEncontrados.Add(numeroIdentificador);
        }

        // Verificar si ya alcanzó el límite de 6 Pokémon
        if (listaDePokemones.Count >= 6)
        {
            await ReplyAsync("Has completado tu selección de 6 Pokémon. ¡Estás listo para la batalla!");
            break;
        }
    }

    // Enviar un solo mensaje con todos los Pokémon agregados
    if (pokemonesAgregados.Count > 0)
    {
        string agregadosMsg = string.Join(", ", pokemonesAgregados);
        await ReplyAsync($"Los siguientes Pokémon han sido añadidos a tu selección: {agregadosMsg}");
    }

    // Enviar mensaje de error si hay Pokémon que no se encontraron
    if (noEncontrados.Count > 0)
    {
        string noEncontradosMsg = string.Join(", ", noEncontrados);
        await ReplyAsync($"No se encontraron Pokémon con los identificadores: {noEncontradosMsg}.");
    }
}

    
    [Command("catalogo")]
    public async Task MostrarCatalogoAsync()
    {
        // Obtener el catálogo procesado como un string
        string catalogo = LeerArchivo.ObtenerCatalogoProcesado();

        // Verificar si el catálogo está vacío
        if (string.IsNullOrEmpty(catalogo))
        {
            await ReplyAsync("No se pudo obtener el catálogo. Está vacío o hubo un error.");
        }
        else
        {
            // Verificar si el catálogo es demasiado largo para enviarlo de una vez
            const int maxMessageLength = 2000; // Límite de caracteres en un mensaje de Discord
            if (catalogo.Length > maxMessageLength)
            {
                // Si el catálogo es muy largo, dividirlo en partes
                int startIndex = 0;
                while (startIndex < catalogo.Length)
                {
                    // Enviar la parte del catálogo que no exceda el límite
                    var messagePart = catalogo.Substring(startIndex, Math.Min(maxMessageLength, catalogo.Length - startIndex));
                    await ReplyAsync(messagePart);
                    startIndex += maxMessageLength;
                }
            }
            else
            {
                // Si el catálogo cabe en un solo mensaje, enviarlo directamente
                await ReplyAsync($"**Estos son los pokemones disponibles:**\n{catalogo}");
            }
        }
    }
    [Command("elegirPokemon")]
    public async Task ElegirPokemonAsync(string pokemonNombre)
    {
        // Obtener al entrenador del usuario actual
        Entrenador entrenador = BattlesList.Instance.ObtenerEntrenadorPorUsuario(Context.User.Id);

        if (entrenador != null )
        {
            // Verificar si el nombre del Pokémon ingresado está en la selección del jugador
            Pokemon pokemonSeleccionado = entrenador.UsarPokemon(pokemonNombre);
            if (pokemonSeleccionado != null)
            {
                // Se seleccionó el Pokémon correctamente
                await ReplyAsync($"¡Has elegido a {pokemonSeleccionado.GetNombre()} para la batalla!");
            
                // Si ambos jugadores han elegido su Pokémon, puedes iniciar la batalla
                // Verificar si ambos jugadores han elegido un Pokémon
                // Si es necesario, puedes establecer la lógica aquí para iniciar la batalla
            }
            else
            {
                // El Pokémon no está en la selección
                await ReplyAsync($"No tienes un Pokémon llamado {pokemonNombre} en tu selección.");
            }
        }
        
    }



}
