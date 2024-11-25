using System.Data.SqlTypes;
using Discord;
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

    public void EnviarACanal (ICanal canal, string mensaje)
    {
        canal.EnviarMensajeAsync(mensaje);
    }

    public void EnviarAUsuario(IGuildUser usuario, string mensaje)
    {
        usuario.SendMessageAsync(mensaje);
    }

    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    public void AddTrainerToWaitingList(ulong userId, string displayName, SocketGuildUser user, ICanal canal)
    {
        //*** FALTA AÑADIR FUNCIONALIDAD DE QUE NO PERMITA UNIRSE A OTRA BATALLA SI YA ESTOY JUGANDO UNA. ***
        //*** POR AHORA NO ESTÁ AÑADIDA, PORQUE RENDIRSE NO ESTÁ AÑADIDO, Y ESO PUEDE DAR PROBLEMAS AL TESTEAR,***
        //*** SI EL BOT CONSIDERA QUE ESTOY EN BATALLA Y NO PUEDO SALIR DE ELLA ***
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
    public void GetTrainersWaiting(ICanal canal)
    {
        string mensaje;
        if (WaitingList.Count == 0)
        {
            mensaje = "No hay nadie esperando";
        }
        else
        {
            mensaje = WaitingList.GetListaDeEspera();
        }
        this.EnviarACanal(canal, mensaje);
    }

    /// <summary>
    /// Determina si un jugador está esperando para jugar.
    /// </summary>
    /// <param name="displayName">El jugador.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public void TrainerStatus(ulong userId, string displayName, ICanal canal)
    {
        string mensaje;
        Entrenador? trainer = this.WaitingList.EncontrarEntrenador(displayName);
        /* TODAVÍA NO ES IMPLEMENTABLE HASTA QUE LA LISTA DE BATALLAS SE VACÍE AUTOMÁTICAMENTE
        if (BattlesList.GetBattle(userId) != null)
        {
            mensaje = $"{displayName} está en una batalla";
        }
        else*/ if (trainer == null)
        {
            mensaje = $"{displayName} no está esperando";
        }
        else
        {
            mensaje = $"{displayName} está esperando";
        }
        this.EnviarACanal(canal, mensaje);
    }

    private void CreateBattle(string playerDisplayName, string opponentDisplayName)
    {
        Entrenador jugador = WaitingList.EncontrarEntrenador(playerDisplayName);
        Entrenador oponente = WaitingList.EncontrarEntrenador(opponentDisplayName);
        
        BattlesList.AddBattle(jugador, oponente);
        
        WaitingList.EliminarEntrenador(playerDisplayName);
        WaitingList.EliminarEntrenador(opponentDisplayName);
        string mensaje = $"==================================================================================\n"+
                         $"Comienza el enfrentamiento: **{playerDisplayName}** vs **{opponentDisplayName}**.\n" +
                         $"==================================================================================\n\n" +
                        $"¡Ahora debes **elegir 6 pokémon** para la batalla!\n" +
                        $"Usa el comando `!catalogo` para ver la lista de pokémon disponibles.\n\n" +
                        $"Para seleccionar tus pokémon, utiliza el comando: `!agregarPokemon <id1>,<id2>,<id3>,<id4>,<id5>,<id6>`\n" +
                        $"Por ejemplo: `!agregarPokemon 1,2,3,4,5,6`.\n\n" +
                        $"**¡Prepárate para la batalla!**";
        this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
        this.EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);
    }

    /// <summary>
    /// Crea una batalla entre dos jugadores.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    /// <returns>Un mensaje con el resultado.</returns>
    public void ChallengeTrainerToBattle(string playerDisplayName, string? opponentDisplayName, ICanal canal)
    {
        // El símbolo ? luego de Trainer indica que la variable opponent puede
        // referenciar una instancia de Trainer o ser null.
        Entrenador opponent;
        string mensaje;
        
        // Si el nombre del oponente es null y no hay nadie esperando
        if (!OpponentProvided() && !SomebodyIsWaiting())
        {
            mensaje = "No hay nadie esperando";
            this.EnviarACanal(canal, mensaje);
        }
        // No hay nombre del oponente, pero sí hay alguien esperando
        else if (!OpponentProvided()) // && SomebodyIsWaiting
        {
            // Si no se escribió un oponente, busca usuarios (que no sean quien envió el comando) en la lista de espera
            opponent = WaitingList.GetAlguienEsperando(playerDisplayName);
            mensaje = $"Comienza el enfrentamiento: **{playerDisplayName}** vs **{opponent.GetNombre()}**.";
            this.EnviarACanal(canal, mensaje);
                // El símbolo ! luego de opponent indica que sabemos que esa
                // variable no es null. Estamos seguros porque SomebodyIsWaiting
                // retorna true si y solo si hay usuarios esperando y en tal caso
                // GetAlguienEsperando nunca retorna null.
                CreateBattle(playerDisplayName, opponent!.GetNombre());
        }
        else
        {
            // El símbolo ! luego de opponentDisplayName indica que sabemos que esa
            // variable no es null. Estamos seguros porque OpponentProvided hubiera
            // retorna false antes y no habríamos llegado hasta aquí.
            opponent = WaitingList.EncontrarEntrenador(opponentDisplayName!);
            
            // Si no se encontró al oponente en la lista de espera
            if (opponent != null && opponent.GetNombre() != playerDisplayName)
            {
                mensaje = $"Comienza el enfrentamiento: **{playerDisplayName}** vs **{opponentDisplayName}**.";
                this.EnviarACanal(canal, mensaje);
                this.CreateBattle(playerDisplayName, opponentDisplayName);
            }
            else
            {
                mensaje = $"{opponentDisplayName} no está esperando";
                this.EnviarACanal(canal, mensaje);
            }
        }
        
        // Funciones locales a continuación para mejorar la legibilidad
        
        bool OpponentProvided() // Devuelve true si el nombre del oponente no es null
        {
            return !string.IsNullOrEmpty(opponentDisplayName);
        }

        bool SomebodyIsWaiting()    // Devuelve true si hay alguien esperando, que no sea quien se pasa por parámetro
        {
            if (WaitingList.GetAlguienEsperando(playerDisplayName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
