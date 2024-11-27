using Proyecto_Pokemones_I;
using System.Linq;
using Library.Bot.Dominio;
using Library.Contenido_Parte_II;

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