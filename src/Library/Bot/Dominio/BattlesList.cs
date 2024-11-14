using Proyecto_Pokemones_I;
using System.Linq;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa la lista de batallas en curso.
/// </summary>
public class BattlesList
{
    private static List<Battle> battles = new List<Battle>();

    /// <summary>
    /// Crea una nueva batalla entre dos jugadores.
    /// </summary>
    /// <param name="player1">El primer jugador.</param>
    /// <param name="player2">El oponente.</param>
    /// <returns>La batalla creada.</returns>
    public Battle AddBattle(Entrenador player1, Entrenador player2)
    {
        var battle = new Battle(player1, player2);
        BattlesList.battles.Add(battle);
        return battle;
    }

    /// <summary>
    /// Busca una batalla que tenga como uno de sus jugadores al usuario.
    /// </summary>
    /// <param name="idUsuario">El ID del usuario.</param>
    /// <returns>La batalla si se encuentra, o null si no se encuentra.</returns>
    // Método para buscar al entrenador que tiene el idUsuario
    public static Entrenador? ObtenerEntrenadorPorUsuario(ulong idUsuario)
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
}