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
    public ulong UserId { get; set; }
    public ListaDeEspera ListaDeEspera { get; }
    
    private BattlesList BattlesList { get; }
    
    private static Fachada? _instance;

    // Este constructor privado impide que otras clases puedan crear instancias
    // de esta.
    private Fachada()
    {
        this.ListaDeEspera= new ListaDeEspera();
        this.BattlesList = BattlesList.Instance;
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
    
   

    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre del jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string AddTrainerToWaitingList(ulong userId, string displayName,  SocketGuildUser? user)
    {
        if (this.ListaDeEspera.AgregarEntrenador(userId,displayName,user))
        {
            this.UserId = userId;
            return $"{displayName} agregado a la lista de espera";
            
        }
        
        return $"{displayName} ya está en la lista de espera";
    }

    /// <summary>
    /// Remueve un jugador de la lista de espera.
    /// </summary>
    /// <param name="displayName">El jugador a remover.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string EliminarEntrenadorDeListaEspera(string displayName)
    {
        if (ListaDeEspera.EliminarEntrenador(displayName))
        {
            return $"{displayName} removido de la lista de espera";
        }
        else
        {
            return $"{displayName} no está en la lista de espera";
        }
    }

    /// <summary>
    /// Obtiene la lista de jugadores esperando.
    /// </summary>
    /// <returns>Un mensaje con el resultado.</returns>
    public string GetTrainerWaiting()
    {
        if (ListaDeEspera.Count == 0)
        {
            return "No hay nadie esperando";
        }

        string result = ListaDeEspera.GetListaDeEspera();
        
        return result;
    }

    /// <summary>
    /// Determina si un jugador está esperando para jugar.
    /// </summary>
    /// <param name="displayName">El jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public string TrainerIsWaiting(string displayName)
    {
        Entrenador? trainer = this.ListaDeEspera.EncontrarEntrenador(displayName);
        if (trainer == null)
        {
            return $"{displayName} no está esperando";
        }
        
        return $"{displayName} está esperando";
    }
    
    private string CreateBattle(string playerDisplayName, string opponentDisplayName)
    {
        Entrenador jugador = ListaDeEspera.EncontrarEntrenador(playerDisplayName);
        Entrenador oponente = ListaDeEspera.EncontrarEntrenador(opponentDisplayName);
        
        BattlesList.AddBattle(jugador, oponente);
        
        ListaDeEspera.EliminarEntrenador(playerDisplayName);
        ListaDeEspera.EliminarEntrenador(opponentDisplayName);
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
            opponent = ListaDeEspera.GetAlguienEsperando();
            
            // El símbolo ! luego de opponent indica que sabemos que esa
            // variable no es null. Estamos seguros porque SomebodyIsWaiting
            // retorna true si y solo si hay usuarios esperando y en tal caso
            // GetAnyoneWaiting nunca retorna null.
            return CreateBattle(playerDisplayName, opponent!.GetNombre());
        }

        // El símbolo ! luego de opponentDisplayName indica que sabemos que esa
        // variable no es null. Estamos seguros porque OpponentProvided hubiera
        // retorna false antes y no habríamos llegado hasta aquí.
        opponent = ListaDeEspera.EncontrarEntrenador(opponentDisplayName!);
        
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
            return this.ListaDeEspera.Count != 0;
        }

        bool OpponentFound()
        {
            return opponent != null;
        }
    }

    public bool? estaParalizado(ulong usuarioId)
    {
        Entrenador? trainer = BattlesList.ObtenerOponentePorUsuario(usuarioId);//busca al entrenador
        if (trainer != null)
        {
            string aux = trainer.GetStatusPokemonEnUso();
            aux = aux.ToUpper();
            if (aux != null)
            {
                if (aux == "PARALIZADO")
                {
                    return false;
                }

                if (aux == "DORMIDO")
                {
                    Random random = new Random();

                    // Generar un valor aleatorio entre 0 y 1
                    bool resultado = random.NextDouble() < 0.5;
                    return resultado;
                }
            }

            return false;
        }

        return null;
    }

    public string? ListaAtaques(ulong id)
    {
        Entrenador entrenador = BattlesList.ObtenerEntrenadorPorUsuario(id);
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



}
