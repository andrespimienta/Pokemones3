using Discord.Commands;
using Library.Bot.Dominio;

namespace Library.Bot.Comandos;

public class StartingCommands : ModuleBase<SocketCommandContext>
{ 
    /// <summary>
    /// Muestra el catálogo de pokemones al jugador que envíe el comando.
    /// </summary>
    [Command("catalogo")]
    public async Task MostrarCatalogoAsync()
    {
        ulong userID = Context.User.Id;  // Obtener el ID del usuario
        
        Fachada.Instance.ShowCatalog(userID);
    }
    
    /// <summary>
    /// Agrega uno o más pokemones a la selección del jugador,
    /// proporcionados por sus números identificadores.
    /// </summary>
    [Command("agregarPokemon")]
    public async Task AgregarPokemonAsync(string numerosIdentificadores)
    {
        ulong userID = Context.User.Id;
        
        Fachada.Instance.AddPokemonToList(userID, numerosIdentificadores);
    }
    
    /// <summary>
    /// Selecciona cuál de los pokemones de la colección del jugador
    /// será el Pokemon en uso para el comienzo de la batalla.
    /// </summary>
    [Command("usar")]
    public async Task UsarPokemonAsync(string numero)
    {
        ulong userID = Context.User.Id;
        
        Fachada.Instance.SelectPokemonInUse(userID, numero);
    }
    [Command("StartBattle")]
    public async Task StartBattleAsync()
    {
        // Obtener el ID del jugador actual usando su ID de Discord
        ulong usuarioId = Context.User.Id;
        Fachada.Instance.StartBattle(usuarioId);

    }
}
