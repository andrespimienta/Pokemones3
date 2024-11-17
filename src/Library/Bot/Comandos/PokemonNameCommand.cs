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
    public async Task AgregarPokemonAsync(string pokemonNombre)
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

        // Validar si ya ha seleccionado 2 Pokémon
        if (listaDePokemones.Count >= 2)
        {
            await ReplyAsync("Ya has seleccionado los 2 Pokémon permitidos para la batalla. No puedes agregar más.");
            return;
        }

        // Buscar el Pokémon en el catálogo
        Pokemon? pokemon = LeerArchivo.EncontrarPokemon(pokemonNombre);

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

            // Enviar mensaje de confirmación
            await ReplyAsync($"¡El Pokémon {pokemon.GetNombre()} ha sido añadido a tu selección!");

            // Avisar si ya ha completado la selección de 2 Pokémon
            if (listaDePokemones.Count == 2)
            {
                await ReplyAsync("Has completado tu selección de Pokémon. ¡Estás listo para la batalla!");
                
            }
        }
        else
        {
            // Si no se encontró el Pokémon, mostrar un mensaje de error
            await ReplyAsync($"No se encontró un Pokémon llamado {pokemonNombre} en el catálogo.");
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
