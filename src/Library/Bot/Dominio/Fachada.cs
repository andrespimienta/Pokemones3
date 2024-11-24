using System.Data.SqlTypes;
using Discord.WebSocket;
using Proyecto_Pokemones_I;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase recibe las acciones y devuelve los resultados que permiten
/// implementar las historias de usuario. Otras clases que implementan el bot
/// usan esta <see cref="Fachada"/> pero no conocen el resto de las clases del
/// dominio. Esta clase es un singleton.
/// </summary>
public class Fachada
{
    public WaitingList WaitingList { get; }
    
    private BattlesList BattlesList { get; }

    private VisitorPorTurno visitor;
    
    private static Fachada? _instance;

    // Este constructor privado impide que otras clases puedan crear instancias
    // de esta.
    private Fachada()
    {
        this.WaitingList= new WaitingList();
        this.BattlesList = BattlesList.Instance;
        this.visitor = new VisitorPorTurno();
    }

    /// <summary>
    /// Obtiene la única instancia de la clase <see cref="Fachada"/>.
    /// </summary>
    public static Fachada Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Fachada();
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

    public void EnviarACanal(ICanal canal, string mensaje)
    {
        canal.EnviarMensajeAsync(mensaje);
    }

    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <param name="user"></param>
    /// <param name="canal"></param>
    /// <returns>Un mensaje con el resultado.</returns>
    public void AddTrainerToWaitingList(ulong userId, string displayName, SocketGuildUser user, ICanal canal)
    {
        string mensaje;
        if (this.WaitingList.AgregarEntrenador(userId,displayName,user))
        {
            mensaje = $"{displayName} agregado a la lista de espera";
        }
        else
        {
            mensaje = $"{displayName} ya está en la lista de espera";
        }
        this.EnviarACanal(canal,mensaje);
    }

    /// <summary>
    /// Remueve un jugador de la lista de espera.
    /// </summary>
    /// <param name="displayName">El jugador a remover.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public void RemoveTrainerFromWaitingList(string displayName, ICanal canal)
    {
        string mensaje;
        if (WaitingList.EliminarEntrenador(displayName))
        {
            mensaje = $"{displayName} removido de la lista de espera";
        }
        else
        {
            mensaje = $"{displayName} no está en la lista de espera";
        }
        this.EnviarACanal(canal, mensaje);
    }

    /// <summary>
    /// Obtiene la lista de jugadores esperando.
    /// </summary>
    /// <returns>Un mensaje con el resultado.</returns>
    public string GetTrainerWaiting()
    {
        if (WaitingList.Count == 0)
        {
            return "No hay nadie esperando";
        }

        string result = WaitingList.GetListaDeEspera();
        
        return result;
    }

    /// <summary>
    /// Determina si un jugador está esperando para jugar.
    /// </summary>
    /// <param name="displayName">El jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string TrainerIsWaiting(string displayName)
    {
        Entrenador? trainer = this.WaitingList.EncontrarEntrenador(displayName);
        if (trainer == null)
        {
            return $"{displayName} no está esperando";
        }
        
        return $"{displayName} está esperando";
    }

    private string CreateBattle(string playerDisplayName, string opponentDisplayName)
    {
        Entrenador jugador = WaitingList.EncontrarEntrenador(playerDisplayName);
        Entrenador oponente = WaitingList.EncontrarEntrenador(opponentDisplayName);
        
        BattlesList.AddBattle(jugador, oponente);
        
        WaitingList.EliminarEntrenador(playerDisplayName);
        WaitingList.EliminarEntrenador(opponentDisplayName);
        return $"Comienza el enfrentamiento: **{playerDisplayName}** vs **{opponentDisplayName}**.\n\n" +
               $"¡Ahora debes elegir 6 pokémon para la batalla!\n" +
               $"Usa el comando `!catalogo` para ver la lista de pokémon disponibles.\n\n" +
               $"Para seleccionar tus pokémon, utiliza el comando: `!agregarPokemon <id1>,<id2>,<id3>,<id4>,<id5>,<id6>`\n" +
               $"Por ejemplo: `!agregarPokemon 1,2,3,4,5,6`.\n\n" +
               $"¡Prepárate para la batalla!";
    }

    /// <summary>
    /// Crea una batalla entre dos jugadores.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string StartBattle(string playerDisplayName, string? opponentDisplayName)
    {
        // El símbolo ? luego de Trainer indica que la variable opponent puede
        // referenciar una instancia de Trainer o ser null.
        Entrenador opponent;
        
        if (!OpponentProvided() && !SomebodyIsWaiting())
        {
            return "No hay nadie esperando";
        }
        
        if (!OpponentProvided()) // && SomebodyIsWaiting
        {
            opponent = WaitingList.GetAlguienEsperando();
            
            // El símbolo ! luego de opponent indica que sabemos que esa
            // variable no es null. Estamos seguros porque SomebodyIsWaiting
            // retorna true si y solo si hay usuarios esperando y en tal caso
            // GetAnyoneWaiting nunca retorna null.
            return CreateBattle(playerDisplayName, opponent!.GetNombre());
        }

        // El símbolo ! luego de opponentDisplayName indica que sabemos que esa
        // variable no es null. Estamos seguros porque OpponentProvided hubiera
        // retorna false antes y no habríamos llegado hasta aquí.
        opponent = WaitingList.EncontrarEntrenador(opponentDisplayName!);
        
        if (!OpponentFound())
        {
            return $"{opponentDisplayName} no está esperando";
        }
        
        return this.CreateBattle(playerDisplayName, opponent!.GetNombre());
        
        // Funciones locales a continuación para mejorar la legibilidad

        bool OpponentProvided()
        {
            return !string.IsNullOrEmpty(opponentDisplayName);
        }

        bool SomebodyIsWaiting()
        {
            return this.WaitingList.Count != 0;
        }

        bool OpponentFound()
        {
            return opponent != null;
        }
    }
    public string? ListaAtaques(ulong userId)
    {
        Battle batalla = BattlesList.GetBattle(userId);
        Entrenador entrenador = batalla.GetEntrenadorActual(userId);
        if (entrenador != null)
        {
            string resultado = "";

            foreach (Ataque ataque in entrenador.GetPokemonEnUso().GetAtaques())
            {
                string palabraAux = ataque.GetNombre();
                Console.Write(palabraAux + " / "); // Imprime cada nombre seguido de un espacio
                resultado += palabraAux + " "; // Agrega cada nombre a la cadena `resultado` seguido de un espacio
            }

            return resultado.Trim(); // Elimina el último espacio extra al final de la cadena
        }

        return null;
    }

    public string? Atacar(ulong userId, string nombreAtaque)
    {
        Battle batalla = BattlesList.GetBattle(userId);
        Pokemon pokemonVictima = batalla.GetEntrenadorOponente(userId).GetPokemonEnUso();
        Pokemon pokemonAtacante = batalla.GetEntrenadorActual(userId).GetPokemonEnUso();
        string result = null;
        
        // Si es el turno del Jugador 1, intentará efectuar el ataque indicado sobre el Pokemon en Uso del Jugador 2
        foreach (Ataque ataque in pokemonAtacante.GetAtaques())
        {
            // Si encontró el ataque especificado en la lista de ataques del Pokemon en uso del jugador, ataca al pokemon en uso del rival
            if (ataque.GetNombre() == nombreAtaque)
            {
                double vidaPrevia = pokemonVictima.GetVida();
                pokemonVictima.RecibirDaño(ataque);

                if (vidaPrevia > pokemonVictima.GetVida())
                {
                    if (pokemonVictima.GetVida() <= 0)
                    {
                        return result = $"{pokemonVictima.GetNombre()} ha sido vencido";
                    }
                    else
                    {
                        return result =
                            $"{pokemonVictima.GetNombre()} ha sufrido daño, su vida es {pokemonVictima.GetVida()}";
                    }
                }

            }
        }
        // Si sale del Foreach sin haber retornado antes, es que no encontró el ataque
        return result="No se encontró el ataque";
    }
    
    public void ComienzoDeTurno(ulong userId)
    {
        Battle batalla = BattlesList.GetBattle(userId);
        Entrenador jugador = batalla.GetEntrenadorActual(userId);
        jugador.AceptarVisitorPorTurno(this.visitor);
        
    }


}
