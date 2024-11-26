using System.Data.SqlTypes;
using System.Text;
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
    // Atributos: (todos ellos son clases singleton)
    public WaitingList WaitingList { get; }
    
    private BattlesList BattlesList { get; }

    private VisitorPorTurno visitor;
    
    private static Fachada? _instance;

    // Constructor: (Privado para impedir que otras clases puedan crear instancias de esta)
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

    public async Task EnviarAUsuario(IGuildUser usuario, string mensaje)
    {
        await usuario.SendMessageAsync(mensaje);
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

    /// <summary>
    /// Intenta crear una batalla entre dos jugadores.
    /// </summary>
    /// <param name="playerDisplayName">El primer jugador.</param>
    /// <param name="opponentDisplayName">El oponente.</param>
    public void ChallengeTrainerToBattle(string playerDisplayName, string? opponentDisplayName, ICanal canal)
    {
        Entrenador opponent;
        string mensaje;
        
        // Si el nombre del oponente es null y no hay nadie esperando
        if (!OpponentProvided() && !SomebodyIsWaiting())
        {
            mensaje = "No hay nadie más esperando para batallar";
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
    
    /// <summary>
    /// Crea efectivamente una instancia de Battle usando los jugadores
    /// como parámetro, y les avisa por privado que comenzó la batalla.
    /// </summary>
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
    /// Llama a LeerArchivo y envía el catálogo de Pokemones ya procesado 
    /// al chat del jugador que envió el comando.
    /// </summary>
    public async Task ShowCatalog(ulong jugador)
    {
        Battle batallaActual = BattlesList.GetBattle(jugador);
        Entrenador entrenadorActual = batallaActual.GetEntrenadorActual(jugador);
        SocketGuildUser usuario = entrenadorActual.GetSocketGuildUser();
        
        // Obtener el catálogo procesado como un string
        string catalogo = LeerArchivo.ObtenerCatalogoProcesado();
        string mensaje = ("========================================\n" +
                          "**Estos son los pokemones disponibles:**\n" +
                          "========================================\n");
        await this.EnviarAUsuario(usuario, mensaje);
        
        // Verificar si el catálogo está vacío
        if (string.IsNullOrEmpty(catalogo))
        {
            mensaje = "No se pudo obtener el catálogo. Está vacío o hubo un error.";
            await this.EnviarAUsuario(usuario, mensaje);
        }
        else
        {
            // Verificar si el catálogo es demasiado largo para enviarlo de una vez
            string[] lineas = catalogo.Split('\n');
            int contadorLineas = 0;
            mensaje = "";
            foreach (string linea in lineas)
            {
                mensaje += linea + "\n";
                contadorLineas += 1;
                
                if (contadorLineas % 26 == 0)
                {
                    await this.EnviarAUsuario(usuario, mensaje);
                    mensaje = "";
                }
            }
            await this.EnviarAUsuario(usuario, mensaje);
        }
    }

    public void AddPokemonToList(ulong userId, string numIdentificadores)
    {
        string mensaje;
        // Obtiene el objeto entrenador del jugador que envió el comando
        Battle batalla = BattlesList.Instance.GetBattle(userId);
        Entrenador entrenador = batalla.GetEntrenadorActual(userId);
        SocketGuildUser usuario = entrenador.GetSocketGuildUser();
        
        // Obtiene la lista actual de Pokémon seleccionados por el entrenador
        List<Pokemon> listaDePokemones = entrenador.GetSeleccion();
        
        // Validar si ya ha seleccionado 6 Pokémon
        if (listaDePokemones.Count >= 6)
        {
            mensaje = "Ya has seleccionado el máximo de 6 Pokémon permitidos para la batalla.";
            this.EnviarAUsuario(usuario, mensaje);
        }

        // Separar los números identificadores por coma, eliminar espacios y asegurarse de que no haya duplicados.
        string[] identificadoresArray = numIdentificadores.Split(',')
        .Select(id => id.Trim()) // Eliminar los espacios alrededor de cada identificador
        .Where(id => !string.IsNullOrEmpty(id)) // Eliminar cualquier entrada vacía por si hay comas extra
        .ToArray();

        // Usamos HashSet para garantizar que los identificadores sean únicos
        HashSet<string> identificadoresUnicos = new HashSet<string>(identificadoresArray);

        // Lista para almacenar Pokémon que no se encontraron
        List<string> noEncontrados = new List<string>();
        List<string> pokemonesAgregados = new List<string>();

        // Recorrer los identificadores
        foreach (string numeroIdentificador in identificadoresUnicos)
        {
            // Verificar si el entrenador ya seleccionó ese Pokémon
            if (listaDePokemones.Any(p => p.NumeroIdentificador == numeroIdentificador))
            {
                mensaje = $"Ya has seleccionado el Pokémon con identificador {numeroIdentificador}, elige otro.";
                this.EnviarAUsuario(usuario, mensaje);
            }
            else
            {
                // Buscar el Pokémon en el catálogo
                Pokemon? pokemon = LeerArchivo.EncontrarPokemon(numeroIdentificador);

                if (pokemon != null)
                {
                    
                    // Añadir el Pokémon al equipo del entrenador
                    pokemonesAgregados.Add(pokemon.GetNombre());
                    entrenador.AñadirASeleccion(pokemon);
                    if (listaDePokemones.Count >= 6)
                    {
                        mensaje = "¡Ya has completado tu selección de 6 Pokémon!";
                        this.EnviarAUsuario(usuario, mensaje);
                        break;
                    }
                }
                else
                {
                    // Si no se encontró el Pokémon, agregarlo a la lista de no encontrados
                    noEncontrados.Add(numeroIdentificador);
                }
            }
        }

        // Enviar un solo mensaje con todos los Pokémon agregados
        if (pokemonesAgregados.Count > 0)
        {
            mensaje = string.Join(", ", pokemonesAgregados);
            mensaje = $"Los siguientes Pokémon han sido añadidos a tu selección: {mensaje}\n";
            this.EnviarAUsuario(usuario, mensaje);
        }

        // Enviar mensaje de error si hay Pokémon que no se encontraron
        if (noEncontrados.Count > 0)
        {
            mensaje = string.Join(", ", noEncontrados);
            mensaje = $"No se encontraron Pokémon con los identificadores: {mensaje}.";
            this.EnviarAUsuario(usuario, mensaje);
        }

        // Verificar si ya alcanzó el límite de 6 Pokémon
        if (listaDePokemones.Count >= 6)
        { 
            mensaje = "¡Estás listo para la batalla!\n\n" +
                      "Pokémon disponibles en tu selección:\n" +
                      "**Elige el pokemon con el que empezarás la batalla con el comando `!usar <numero identificador del pokemon de tu lista>`**\n";
            
            listaDePokemones = entrenador.GetSeleccion();
            for (int i = 0; i < listaDePokemones.Count; i++)
            {
                mensaje += $"{i + 1}) {listaDePokemones[i].GetNombre()}\n";
            }
            this.EnviarAUsuario(usuario, mensaje);
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

    public void Atacar(ulong userId, string nombreAtaque)
    {
        //agregar que solo entrenador con turno pueda hacerlo
        Battle batalla = BattlesList.GetBattle(userId);
        Entrenador atacante = batalla.GetEntrenadorActual(userId);
        Entrenador oponente = batalla.GetEntrenadorOponente(userId);
        Pokemon pokemonVictima = oponente.GetPokemonEnUso();
        Pokemon pokemonAtacante = atacante.GetPokemonEnUso();
        string mensaje;

        if (string.IsNullOrEmpty(nombreAtaque))
        {
            this.ListaAtaques(atacante);
        }
        else
        {
            bool EncontroAtaque = false;
            // Si es el turno del Jugador 1, intentará efectuar el ataque indicado sobre el Pokemon en Uso del Jugador 2
            foreach (Ataque ataque in pokemonAtacante.GetAtaques())
            {
                // Si encontró el ataque especificado en la lista de ataques del Pokemon en uso del jugador, ataca al pokemon en uso del rival
                if (ataque.GetNombre() == nombreAtaque)
                {
                    double vidaPrevia = pokemonVictima.GetVida();
                    pokemonVictima.RecibirDaño(ataque);
                    EncontroAtaque = true;
                    if (vidaPrevia > pokemonVictima.GetVida())
                    {
                        if (pokemonVictima.GetVida() <= 0)
                        {
                            mensaje = $"{pokemonVictima.GetNombre()} ha sido vencido";
                            this.EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                        }
                        else
                        {
                            mensaje =
                                $"{pokemonVictima.GetNombre()} ha sufrido daño, su vida es {pokemonVictima.GetVida()}";
                            this.EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                        }
                    }
                }
            }

            // Si sale del Foreach sin haber retornado antes, es que no encontró el ataque
            if (EncontroAtaque == false)
            {
                mensaje = "No se encontró el ataque";
                this.EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
            }
        }
    }

    public void ListaAtaques(Entrenador entrenador)
    {
        string mensaje = "Estos son los ataques disponibles: ";
        SocketGuildUser user = entrenador.GetSocketGuildUser();

        foreach (Ataque ataque in entrenador.GetPokemonEnUso().GetAtaques())
        {
            mensaje += ataque.GetNombre() + " / "; // Imprime cada nombre seguido de un espacio
        }

        mensaje.Trim(); // Elimina el último espacio extra al final de la cadena
        this.EnviarAUsuario(user, mensaje);
        Console.WriteLine(mensaje);
    }

    public void ComienzoDeTurno(ulong userId)
    {
        Battle batalla = BattlesList.GetBattle(userId);
        Entrenador jugador = batalla.GetEntrenadorActual(userId);
        jugador.AceptarVisitorPorTurno(this.visitor);
        
    }


}
