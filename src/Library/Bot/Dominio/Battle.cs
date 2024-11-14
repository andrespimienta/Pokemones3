using Proyecto_Pokemones_I;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa una batalla entre dos jugadores.
/// </summary>
public class Battle
{
    /// <summary>
    /// Obtiene un valor que representa el primer jugador.
    /// </summary>
    public Entrenador Player1 { get; }
    
    /// <summary>
    /// Obtiene un valor que representa al oponente.
    /// </summary>
    public Entrenador Player2 { get; }

    /// <summary>
    /// Inicializa una instancia de la clase <see cref="Battle"/> con los
    /// valores recibidos como argumento.
    /// </summary>
    /// <param name="player1">El primer jugador.</param>
    /// <param name="player2">El oponente.</param>
    public Battle(Entrenador player1, Entrenador player2)
    {
        this.Player1 = player1;
        this.Player2 = player2;
    }
    // Método que verifica si ambos entrenadores están listos
    public bool EstanListos()
    {
        return Player1.EstaListo && Player2.EstaListo;
    }

}