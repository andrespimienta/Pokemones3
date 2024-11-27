#nullable enable
using System.Transactions;
using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Items;

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
        this.CalcularProbGanador(); //Se calcula la probabilidad del ganador cada vez que se efectua el turno
    }
    
    // Método para calcular que jugador tiene más probabilidad de ganar. DEFENSA 27 DE NOVIEMBRE.
    public string CalcularProbGanador()
    {
        string posibleGanador = "";
        Entrenador player1 = this.Player1;
        Entrenador player2 = this.Player2;

        //Se crea una variable que guarde los puntos de probabilidad de cada jugador
        double prob1 = 0; 
        double prob2 = 0;

        //Se trae la cantidad de pokemones vivos del jugador y se multiplican por 10
        int pokemones1 = player1.GetCantidadPokemonesVivos();
        prob1 = pokemones1 * 10;
        
        int pokemones2 = player2.GetCantidadPokemonesVivos();
        prob2 = pokemones2 * 10;

        //Se trae la cantidad de Items del jugador y se multiplican por 4.2, que en total suman 30
        int cantidadItems1 = 0;
        foreach (Item i in player1.GetListaItems())
        {
            cantidadItems1 += 1;
        }
        prob1 += cantidadItems1 * 4.2;
        
        int cantidadItems2 = 0;
        foreach (Item i in player2.GetListaItems())
        {
            cantidadItems2 += 1;
        }
        prob2 += cantidadItems2 * 4.2;

        //Se busca si el jugador tiene pokemones afectados
        bool p1Afectado = false;
        foreach (Pokemon pokemon in player1.GetSeleccion())
        {
            if (pokemon.EfectoActivo != null)
            {
                p1Afectado = true;
            }
        }

        if (p1Afectado = true)
        {
            prob1 -= 10;
        }
        
        
        bool p2Afectado = false;
        foreach (Pokemon pokemon in player2.GetSeleccion())
        {
            if (pokemon.EfectoActivo != null)
            {
                p2Afectado = true;
            }
        }

        if (p2Afectado = true)
        {
            prob2 -= 10;
        }

        //Se verifica que jugador tiene más probabilidad de ganar y se retorna la string
        if (prob1 > prob2)
        {
            posibleGanador=$"El jugador más probable de ganar es {player1.GetNombre()}";
        }
        else if(prob1 <prob2)
        {
            posibleGanador=$"El jugador más probable de ganar es {player2.GetNombre()}";
        }
        else
        {
            posibleGanador=$"Ambos jugadores tienen la misma probabilidad de ganar";
        }

        return posibleGanador;
    }
}
