using Proyecto_Pokemones_I;
using System.Linq;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa la lista de batallas en curso.
/// </summary>
public class BattlesList
{
    public List<Battle> battles { get; }
    public static BattlesList? _instance;
    private BattlesList()
    {
     battles= new List<Battle>();
        
    }
    public static BattlesList Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BattlesList();
            }

            return _instance;
        }
    }
    
    /// <summary>
    /// Inicializa este singleton. Es necesario solo en los tests.
    /// </summary>
    public static void Reset()
    {
        _instance = null;
    }
    

    /// <summary>
    /// Crea una nueva batalla entre dos jugadores.
    /// </summary>
    /// <param name="player1">El primer jugador.</param>
    /// <param name="player2">El oponente.</param>
    /// <returns>La batalla creada.</returns>
    public void AddBattle(Entrenador player1, Entrenador player2)
    {
        var battle = new Battle(player1, player2);
        
        battles.Add(battle);
    }

    /// <summary>
    /// Busca una batalla que tenga como uno de sus jugadores al usuario.
    /// </summary>
    /// <param name="idUsuario">El ID del usuario.</param>
    /// <returns>La batalla si se encuentra, o null si no se encuentra.</returns>
    // Método para buscar al entrenador que tiene el idUsuario
    public Entrenador? ObtenerEntrenadorPorUsuario(ulong idUsuario)
    {
        // Buscar al entrenador en la lista de batallas
        foreach (var battle in battles)
        {
            if (battle.Player1.Id == idUsuario)
            {
                return battle.Player1;  // Si el idUsuario es el jugador 1, devolvemos el entrenador
            }
            else if (battle.Player2.Id == idUsuario)
            {
                return battle.Player2;  // Si el idUsuario es el jugador 2, devolvemos el entrenador
            }
        }

        // Si no se encuentra el entrenador en ninguna batalla, devolvemos null
        return null;
    }
    public Entrenador? ObtenerOponentePorUsuario(ulong idUsuario)
    {
        // Buscar al entrenador en la lista de batallas
        foreach (var battle in battles)
        {
            if (battle.Player1.Id == idUsuario)
            {
                return battle.Player2;  // Si el idUsuario es el jugador 1, devolvemos el entrenador 2
            }
            else if (battle.Player2.Id == idUsuario)
            {
                return battle.Player1;  // Si el idUsuario es el jugador 2, devolvemos el entrenador 1
            }
        }

        // Si no se encuentra el entrenador en ninguna batalla, devolvemos null
        return null;
    }

    public Battle? GetBattle(ulong id)
    {
        foreach (Battle batalla in this.battles)
        {
            if (batalla.Player1.Id == id || batalla.Player2.Id == id)
            {
                return batalla;
            }
        }

        return null;
    }
}