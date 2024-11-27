#nullable enable
using Library.Contenido_Parte_II;
using Proyecto_Pokemones_I;

namespace Library.Bot.Dominio;

/// <summary>
/// Esta clase representa una batalla entre dos jugadores.
/// </summary>
public class Battle
{
    // Atributos:
    /// <summary>
    /// Obtiene un valor que representa el primer jugador.
    /// </summary>
    public Entrenador Player1 { get; }

    /// <summary>
    /// Obtiene un valor que representa al oponente.
    /// </summary>
    public Entrenador Player2 { get; }

    /// <summary>
    /// Obtiene un valor que representa al ganador.
    /// </summary>
    public Entrenador? Ganador { get; set; }

    public Entrenador? EntrenadorConTurno {get; set;}

// Constructor:
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
        this.Ganador = null;
    }
    
    // Métodos
    /// <summary>
    /// Chequea que ambos jugadores estén preparados.
    /// </summary>
    public bool EstanListos()
    {
        return Player1.EstaListo && Player2.EstaListo;
    }

    /// <summary>
    /// Busca si alguno de los entrenadores de esta batalla tiene el ID del parámetro, y si es así, lo devuelve.
    /// </summary>
    /// <param name="userId">El ID del usuario.</param>
    /// <returns>El entrenador si se encuentra, o null si no se encuentra.</returns>
    // Método para buscar al entrenador que tiene el userID
    public Entrenador? GetEntrenadorActual(ulong userId)
    {
        if (this.Player1.Id == userId)
        {
            return this.Player1;  // Si el idUsuario es el jugador 1, devolvemos el entrenador
        }
        else if (this.Player2.Id == userId)
        {
            return this.Player2;  // Si el idUsuario es el jugador 2, devolvemos el entrenador
        }
        // Si no se encuentra el entrenador en ninguna batalla, devolvemos null
        return null;
    }
    
    /// <summary>
    /// Busca si alguno de los entrenadores de esta batalla tiene el ID del parámetro, y si es así, lo devuelve.
    /// </summary>
    /// <param name="userId">El ID del usuario.</param>
    /// <returns>El entrenador si se encuentra, o null si no se encuentra.</returns>
    // Método para buscar al entrenador oponente respecto al que tiene el userID
    public Entrenador? GetEntrenadorOponente(ulong userId)
    {
        if (this.Player1.Id == userId)
        {
            return this.Player2; // Si el idUsuario es el jugador 1, devolvemos el entrenador 2
        }
        else if (this.Player2.Id == userId)
        {
            return this.Player1; // Si el idUsuario es el jugador 2, devolvemos el entrenador 1
        }
        // Si no se encuentra el entrenador en la batalla, devolvemos null
        return null;
    }

    public Entrenador? GetEntrenadorConTurno()
    {
        return this.EntrenadorConTurno;
    }

    /// <summary>
    /// Asigna un ganador a la batalla.
    /// </summary>
    // Método para indicar el ganador de la batalla
    public void HayGanador(Entrenador elGanador)
    {
        this.Ganador = elGanador;
    }

    public void CambiarTurno()
    {
        if (EntrenadorConTurno == Player1)
        {
            EntrenadorConTurno = Player2;
        }
        else
        {
            EntrenadorConTurno = Player1;
        }
    }
}
