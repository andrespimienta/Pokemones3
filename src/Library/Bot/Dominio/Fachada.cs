using Discord;
using Discord.WebSocket;
using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using Library.Contenido_Parte_II.Items;

namespace Library.Bot.Dominio
{
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
            this.WaitingList = new WaitingList();
            this.BattlesList = BattlesList.Instance;
            this.visitor = VisitorPorTurno.GetInstancia();
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

        public async Task EnviarACanal(ICanal canal, string mensaje)
        {
            await canal.EnviarMensajeAsync(mensaje);
        }

        public async Task EnviarAUsuario(IGuildUser usuario, string mensaje)
        {
            await usuario.SendMessageAsync(mensaje);
        }

        /// <summary>
        /// Agrega un jugador a la lista de espera.
        /// </summary>
        public void AddTrainerToWaitingList(ulong userID, string displayName, SocketGuildUser user, ICanal canal)
        {
            //*** FALTA AÑADIR FUNCIONALIDAD DE QUE NO PERMITA UNIRSE A OTRA BATALLA SI YA ESTOY JUGANDO UNA. ***
            //*** POR AHORA NO ESTÁ AÑADIDA, PORQUE RENDIRSE NO ESTÁ AÑADIDO, Y ESO PUEDE DAR PROBLEMAS AL TESTEAR,***
            //*** SI EL BOT CONSIDERA QUE ESTOY EN BATALLA Y NO PUEDO SALIR DE ELLA ***
            string mensaje;
            if (this.WaitingList.AgregarEntrenador(userID, displayName, user))
            {
                mensaje = $"{displayName} agregado a la lista de espera";
            }
            else
            {
                mensaje = $"{displayName} ya está en la lista de espera";
            }

            this.EnviarACanal(canal, mensaje);
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
        public void TrainerStatus(ulong userID, string displayName, ICanal canal)
        {
            string mensaje;
            Entrenador? trainer = this.WaitingList.EncontrarEntrenador(displayName);
            /* TODAVÍA NO ES IMPLEMENTABLE HASTA QUE LA LISTA DE BATALLAS SE VACÍE AUTOMÁTICAMENTE
            if (BattlesList.GetBattle(userId) != null)
            {
                mensaje = $"{displayName} está en una batalla";
            }
            else*/
            if (trainer == null)
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

            bool SomebodyIsWaiting() // Devuelve true si hay alguien esperando, que no sea quien se pasa por parámetro
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
            string mensaje = $"====================================================================\n" +
                             $"                Comienza el enfrentamiento: \n" +
                             $"                **{playerDisplayName}** :crossed_swords: **{opponentDisplayName}**.\n" +
                             $"====================================================================\n\n" +
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
        public async Task ShowCatalog(ulong userID)
        {
            Battle batallaActual = BattlesList.GetBattle(userID);
            Entrenador entrenadorActual = batallaActual.GetEntrenadorActual(userID);

            // Obtener el catálogo procesado como un string
            string catalogo = LeerArchivo.ObtenerCatalogoProcesado();
            string mensaje = (
                "===========================================================\n" +
                "                         **Estos son los pokemones disponibles:**\n" +
                "===========================================================\n");
            await this.EnviarAUsuario(entrenadorActual.GetSocketGuildUser(), mensaje);

            // Verificar si el catálogo está vacío
            if (string.IsNullOrEmpty(catalogo))
            {
                mensaje = "No se pudo obtener el catálogo. Está vacío o hubo un error.";
                await this.EnviarAUsuario(entrenadorActual.GetSocketGuildUser(), mensaje);
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
                        await this.EnviarAUsuario(entrenadorActual.GetSocketGuildUser(), mensaje);
                        mensaje = "";
                    }
                }

                await this.EnviarAUsuario(entrenadorActual.GetSocketGuildUser(), mensaje);
            }
        }

        /// <summary>
        /// Agrega uno o más pokemones a la selección del
        /// jugador, indicados por su número identificador
        /// </summary>
        public async Task AddPokemonToList(ulong userID, string numIdentificadores)
        {
            string mensaje;
            // Obtiene el objeto entrenador del jugador que envió el comando
            Battle batalla = BattlesList.Instance.GetBattle(userID);
            Entrenador entrenador = batalla.GetEntrenadorActual(userID);

            // Obtiene la lista actual de Pokémon seleccionados por el entrenador
            List<Pokemon> listaDePokemones = entrenador.GetSeleccion();

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
                    mensaje = $"Ya has seleccionado el Pokémon con identificador {numeroIdentificador}.";
                    await this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
                }
                else
                {
                    // Buscar el Pokémon en el catálogo
                    Pokemon? pokemon = LeerArchivo.EncontrarPokemon(numeroIdentificador);

                    if (pokemon != null)
                    {
                        if (listaDePokemones.Count >= 6)
                        {
                            mensaje = $"¡Ya has completado tu selección de 6 Pokémon!, tus chances de ganar son de {entrenador.ChancesGanar()}";
                            await this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
                            break;
                        }
                        else
                        {
                            // Añadir el Pokémon al equipo del entrenador
                            pokemonesAgregados.Add(pokemon.GetNombre());
                            entrenador.AñadirASeleccion(pokemon);
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
                await this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
            }

            // Enviar mensaje de error si hay Pokémon que no se encontraron
            if (noEncontrados.Count > 0)
            {
                mensaje = string.Join(", ", noEncontrados);
                mensaje = $"No se encontraron Pokémon con los identificadores: {mensaje}.";
                await this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
            }

            // Verificar si ya alcanzó el límite de 6 Pokémon
            if (listaDePokemones.Count >= 6)
            {
                mensaje = "¡Estás listo para la batalla!\n\n" +
                          "**Elige el pokemon con el que empezarás la batalla** con el comando `!usar <numero identificador del pokemon de tu lista>`\n\n" +
                          "**Pokémon disponibles en tu selección:**\n";

                listaDePokemones = entrenador.GetSeleccion();
                for (int i = 0; i < listaDePokemones.Count; i++)
                {
                    mensaje +=
                        $"**{i + 1})** {listaDePokemones[i].GetNombre()} {DiccionarioTipos.GetEmoji(listaDePokemones[i].GetTipo())}\n";
                }

                await this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
            }
        }

        /// <summary>
        /// Selecciona el pokemon indicado como el pokemon
        /// en uso del jugador. Será con el que ataque primero.
        /// </summary>
        public void SelectPokemonInUse(ulong userID, string numEleccion)
        {
            Battle batalla = BattlesList.Instance.GetBattle(userID);
            Entrenador? entrenador = batalla.GetEntrenadorActual(userID);
            List<Pokemon> seleccionPokemones = entrenador.GetSeleccion();
            string mensaje;

            // Exception si el usuario escribe algo que no es un número después de !usar
            try
            {
                int numElegido = int.Parse(numEleccion);

                // Validar que el número ingresado se válido
                if (numElegido < 1 || numElegido > seleccionPokemones.Count)
                {
                    mensaje = $"Por favor, ingresa un número válido entre 1 y {seleccionPokemones.Count}.";
                    this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
                }
                else
                {
                    // Seleccionar el Pokémon basado en el número ingresado
                    Pokemon pokemonSeleccionado = seleccionPokemones[numElegido - 1];

                    // Usar el Pokémon seleccionado
                    entrenador.UsarPokemon(pokemonSeleccionado);

                    mensaje = $"¡Has elegido a {pokemonSeleccionado.GetNombre()} para la batalla!\n" +
                              "**Indica que estás listo para la batalla:** Usa el comando `!startBattle` " +
                              "para confirmar que estás listo para luchar.";
                    this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
                }
            }
            catch (FormatException)
            {
                mensaje = "Error: Has ingresado carácteres no numéricos.";
                this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
                this.EnviarACanal(CanalConsola.Instance, mensaje);
            }
            catch (OverflowException)
            {
                mensaje = "Error: El número ingresado está fuera del rango permitido para un entero.";
                this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
                this.EnviarACanal(CanalConsola.Instance, mensaje);
            }
        }

        public async Task StartBattle(ulong usuarioId)
        {
            Battle batalla = BattlesList.GetBattle(usuarioId);

            Entrenador? entrenador = batalla.GetEntrenadorActual(usuarioId);

            SocketGuildUser user = entrenador.GetSocketGuildUser();


            // Si el jugador ya está marcado como listo, no se incrementa el contador ni se hace nada más
            string mensaje;
            if (entrenador.EstaListo)
            {
                mensaje = "Ya estás marcado como listo para la batalla.";
                await this.EnviarAUsuario(entrenador.GetSocketGuildUser(), mensaje);
            }

            // Marcar al entrenador como listo y aumentar el contador
            entrenador.EstaListo = true;

            mensaje = $"{entrenador.GetNombre()} está listo para la batalla.";
            await EnviarAUsuario(user, mensaje);



            // Comprobar si ambos jugadores están listos
            if (batalla.EstanListos() == true)
            {
                await ChequearQuienEmpieza(usuarioId);
                //entrenadoresListos = 0; // Resetear el contador de listos después de iniciar la batalla
            }
            else
            {
                // Si solo uno está listo, esperar al oponente
                mensaje = "Esperando a que tu oponente esté listo...";
                await EnviarAUsuario(user, mensaje);
            }

        }

        public async Task ChequearQuienEmpieza(ulong userId)
        {
            Battle batalla = this.BattlesList.GetBattle(userId);
            var user1 = batalla.Player1.GetSocketGuildUser();
            var user2 = batalla.Player2.GetSocketGuildUser();

            string mensajeDeComienzo = "¡Ambos jugadores están listos! Comenzando la batalla...";
            await EnviarAUsuario(user1, mensajeDeComienzo);
            await EnviarAUsuario(user2, mensajeDeComienzo);

            Pokemon pokemonJugador1 = batalla.Player1.GetPokemonEnUso();
            Pokemon pokemonJugador2 = batalla.Player2.GetPokemonEnUso();

            string turnoJugador;

            // Comparar la velocidad de los Pokémon para determinar quién empieza
            if (pokemonJugador1.GetVelocidadAtaque() > pokemonJugador2.GetVelocidadAtaque())
            {
                turnoJugador = batalla.Player1.GetNombre();
                batalla.EntrenadorConTurno = batalla.Player1;
            }
            else if (pokemonJugador2.GetVelocidadAtaque() > pokemonJugador1.GetVelocidadAtaque())
            {
                turnoJugador = batalla.Player2.GetNombre();
                batalla.EntrenadorConTurno = batalla.Player2;
            }
            else
            {
                // Si la velocidad es igual, se elige al azar
                turnoJugador = new System.Random().Next(2) == 0
                    ? batalla.Player1.GetNombre()
                    : batalla.Player2.GetNombre();
                if (turnoJugador == batalla.Player1.GetNombre())
                {
                    batalla.EntrenadorConTurno = batalla.Player1;
                }
                else
                {
                    batalla.EntrenadorConTurno = batalla.Player2;
                }
            }

            // Notificar a ambos jugadores sobre quién empieza
            if (turnoJugador == batalla.Player1.GetNombre())
            {
                string mensaje2 =
                    $"{batalla.Player2.GetNombre()}, tu oponente {batalla.Player1.GetNombre()} ha elegido {pokemonJugador1.GetNombre()} y comenzará con el turno.\n" +
                    "Esperando a que tu oponente decida que hacer...";
                await EnviarAUsuario(user2, mensaje2);

                string mensaje1 =
                    $"{batalla.Player1.GetNombre()}, es tu turno.\nTu oponente está usando {pokemonJugador2.GetNombre()}.";
                await EnviarAUsuario(user1, mensaje1);

                // Mostrar opciones solo al jugador que tiene el turno
                await MostrarOpciones(user1);
            }
            else
            {
                string mensaje1 =
                    $"{batalla.Player1.GetNombre()}, tu oponente {batalla.Player2.GetNombre()} ha elegido {pokemonJugador2.GetNombre()} y comenzará con el turno.\n" +
                    "Esperando a que tu oponente decida que hacer...";
                await EnviarAUsuario(user1, mensaje1);

                string mensaje2 =
                    $"{batalla.Player2.GetNombre()}, es tu turno.\nTu oponente está usando {pokemonJugador1.GetNombre()}.";
                await EnviarAUsuario(user2, mensaje2);

                // Mostrar opciones solo al jugador que tiene el turno
                await MostrarOpciones(user2);
            }
        }

        public async Task ComienzoDeTurno(Battle batalla)
        {
            Entrenador jugador = batalla.GetEntrenadorConTurno();
            Entrenador oponente = batalla.GetEntrenadorOponente(jugador.Id);
            Pokemon pokeOponente = oponente.GetPokemonEnUso();

            string mensaje = "\n__**ES TU TURNO**__\n" +
                             $"Tu oponente está usando a {pokeOponente.GetNombre()} " +
                             $"( {DiccionarioTipos.GetEmoji(pokeOponente.GetTipo())} , " +
                             $"❤️ {pokeOponente.GetVida()} )\n\n";
            await EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);

            // Calcula todos los efectos y muestra el estado de los pokemones
            await jugador.AceptarVisitorPorTurno(this.visitor);
            await ShowPokemonStatus(jugador);

            await MostrarOpciones(jugador.GetSocketGuildUser());
        }

        /// <summary>
        /// Muestra el estado actual de todos tus pokemones
        /// </summary>
        public async Task ShowPokemonStatus(Entrenador jugador)
        {
            List<Pokemon> listaPokemones = jugador.GetSeleccion();
            List<Pokemon> listaVencidos = jugador.GetListaMuertos();

            string mensaje = "\nEste es el estado de tus pokemones:\n" +
                             "__**VIVOS:**__\n";
            foreach (Pokemon pokemon in listaPokemones)
            {
                string condición;
                if (pokemon.EfectoActivo == null)
                {
                    condición = "👍🏻 SALUDABLE";
                }
                else
                {
                    condición = $"{DiccionarioTipos.GetEmoji(pokemon.EfectoActivo)} " +
                                $"{pokemon.EfectoActivo}";
                }

                if (pokemon == jugador.GetPokemonEnUso())
                {
                    mensaje += $"_**{pokemon.GetNombre()}**," +
                               $"  {DiccionarioTipos.GetEmoji(pokemon.GetTipo())}," +
                               $"  {condición}," +
                               $"  ❤️ {pokemon.GetVida()} (seleccionado)_\n";
                }
                else
                {
                    mensaje += $"{pokemon.GetNombre()}," +
                               $"  {DiccionarioTipos.GetEmoji(pokemon.GetTipo())}," +
                               $"  {condición}," +
                               $"  ❤️ {pokemon.GetVida()}\n";
                }
            }

            mensaje += "\n__**VENCIDOS**__\n";
            if (listaVencidos.Count == 0)
            {
                mensaje += "_(Ninguno)_\n";
            }
            else
            {
                foreach (Pokemon pokemon in listaVencidos)
                {
                    mensaje += $"_~~{pokemon.GetNombre()}~~_," +
                               $"  {DiccionarioTipos.GetEmoji(pokemon.GetTipo())}  ❌️\n";
                }

            }

            await this.EnviarACanal(CanalConsola.Instance, mensaje);
            await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
        }

        public async Task MostrarOpciones(SocketGuildUser jugador)
        {
            string mensaje = "__**Elige una acción:**__\n" +
                             "`!Atacar`\n" +
                             "`!CambiarPokemon`\n" +
                             "`!UsarPocion`\n" +
                             "`!Rendirse`";
            EnviarAUsuario(jugador, mensaje);
        }

        public async Task Atacar(ulong userId, string numeroAtaque)
        {
            Battle batalla = BattlesList.GetBattle(userId);
            Entrenador atacante = batalla.GetEntrenadorActual(userId);

            //SOLO LO PUEDE EJECUTAR EL JUGADOR CON TURNO
            if (atacante == batalla.EntrenadorConTurno && atacante.GetPokemonEnUso().PuedeAtacar)
            {
                // FALTA NO PERMITIR USAR EL ATAQUE ESPECIAL SI TODAVÍA ESTÁ RECARGÁNDOSE

                Entrenador oponente = batalla.GetEntrenadorOponente(userId);
                Pokemon pokemonVictima = oponente.GetPokemonEnUso();
                Pokemon pokemonAtacante = atacante.GetPokemonEnUso();
                string mensaje;

                if (string.IsNullOrEmpty(numeroAtaque))
                {
                    this.MostrarListaAtaques(atacante);
                }
                else
                {
                    bool EncontroAtaque = false;

                    if (!int.TryParse(numeroAtaque, out int nAtaque))
                    {
                        // Si la conversión falla
                        await EnviarAUsuario(atacante.GetSocketGuildUser(),
                            "El número de ataque ingresado no es válido. Por favor, intentá nuevamente.");
                        return;
                    }

                    // Si es el turno del Jugador 1, intentará efectuar el ataque indicado sobre el Pokemon en Uso del Jugador 2
                    var ataques = pokemonAtacante.GetAtaques();
                    if (nAtaque < 1 || nAtaque > ataques.Count)
                    {
                        string mensajeError = "Número de ataque inválido. Por favor, selecciona un número válido.";
                        await EnviarAUsuario(atacante.GetSocketGuildUser(), mensajeError);
                        return;
                    }

                    // Encuentra el ataque correspondiente al número
                    Ataque ataque = ataques[nAtaque - 1];
                    EncontroAtaque = true;

                    // Procede con el ataque
                    mensaje = "**¡Tu oponente decidió atacarte!**\n";
                    await EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);

                    double vidaPrevia = pokemonVictima.GetVida();
                    mensaje = pokemonVictima.RecibirDaño(ataque);

                    await EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                    await EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);

                    // Si recibió daño
                    if (vidaPrevia > pokemonVictima.GetVida())
                    {
                        // Si lo mató
                        if (pokemonVictima.GetVida() <= 0)
                        {
                            pokemonVictima.PuedeAtacar = false;
                            oponente.AgregarAListaMuertos(pokemonVictima);
                            mensaje = $"{pokemonVictima.GetNombre()} ha sido vencido\n";
                            await EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                            await EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);
                        }
                        else
                        {
                            mensaje =
                                $"{pokemonVictima.GetNombre()} ha sufrido daño, su vida es {pokemonVictima.GetVida()}\n";
                            await EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                            await EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);
                        }
                    }

                    // Si sale del Foreach sin haber retornado antes, es que no encontró el ataque
                    if (EncontroAtaque == false)
                    {
                        mensaje = "No se encontró el ataque";
                        await EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                    }

                    else if (await ChequeoPantallaFinal(userId, batalla) == false) // si no hay ganador
                    {
                        CambiarTurno(userId);
                        mensaje = $"Concluíste tu turno.\n" +
                                  $"__**ES EL TURNO DE {oponente.GetNombre()}**__";
                        await EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                        await ComienzoDeTurno(batalla);
                    }
                }
            }
            else if (atacante.GetListaMuertos().Contains(atacante.GetPokemonEnUso()))
            {
                string mensje = "Tu pokemón ya fue vencido.\n" +
                                "Debes cambiar tu pokemón.\n";
                await EnviarAUsuario(atacante.GetSocketGuildUser(), mensje);
            }
            else if (atacante.GetPokemonEnUso().PuedeAtacar == false)
            {
                string mensje = $"Tu pokemón está {atacante.GetPokemonEnUso().EfectoActivo}, **no puede atacar**\n" +
                                "Debes cambiar tu pokemón.\n";
                await EnviarAUsuario(atacante.GetSocketGuildUser(), mensje);
            }
            else
            {
                string mensje = "**NO ES TU TURNO AUN, espera a que tu contrincante termine su turno**\n";
                await EnviarAUsuario(atacante.GetSocketGuildUser(), mensje);
            }
        }

        public void MostrarListaAtaques(Entrenador entrenador)
        {
            // Obtén el usuario para enviar el mensaje
            SocketGuildUser user = entrenador.GetSocketGuildUser();

            // Construye el mensaje
            string mensaje =
                "Estos son los ataques disponibles, elige el ataque con el comando `!Atacar <numero del ataque>`:\n\n";

            // Separa los ataques normales y el ataque especial
            var ataques = entrenador.GetPokemonEnUso().GetAtaques();
            var ataquesNormales = ataques.Take(3).ToList(); // Los primeros 3 son normales
            var ataqueEspecial = ataques.Skip(3).FirstOrDefault(); // El cuarto es el especial

            // Agregar ataques normales
            mensaje += "Ataques normales:\n";
            for (int i = 0; i < ataquesNormales.Count; i++)
            {
                mensaje += $"{i + 1}) {ataquesNormales[i].GetNombre()}\n";
            }

            // Agregar ataque especial
            if (ataqueEspecial != null)
            {
                mensaje += "\nAtaque especial:\n";
                mensaje += $"4) {ataqueEspecial.GetNombre()}\n";
            }

            // Envía el mensaje al usuario y lo imprime en la consola
            this.EnviarAUsuario(user, mensaje);
            Console.WriteLine(mensaje);
        }


        public async Task CambiarPokemon(ulong userId, string nombrePokemon)
        {

            Battle batalla = BattlesList.GetBattle(userId);
            Entrenador atacante = batalla.GetEntrenadorActual(userId);
            SocketGuildUser user = atacante.GetSocketGuildUser();
            string mensaje = "";

            //SOLO LO PUEDE EJECUTAR EL JUGADOR CON TURNO
            if (atacante == batalla.EntrenadorConTurno)
            {
                //Mostrar los pokemones disponibles si 
                if (string.IsNullOrEmpty(nombrePokemon))
                {
                    mensaje =
                        "**Elige el pokemon que quieres utilizar** con el comando `!CambiarPokemon <nombre del pokemon de tu lista>`\n\n" +
                        "**Pokémon disponibles en tu selección:**\n";
                    // Obtiene la lista actual de Pokémon seleccionados por el entrenador
                    List<Pokemon> listaDePokemones = atacante.GetSeleccion();

                    for (int i = 0; i < listaDePokemones.Count; i++)
                    {
                        mensaje += $"{i + 1}) {listaDePokemones[i].GetNombre()}\n";
                    }

                    await this.EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);
                }

                else //Busca al Pokemon
                {
                    // FALTA AGREGAR CASO LÍMITE EN QUE SE ELIGE EL MISMO POKEMON QUE YA ESTÁ EN USO

                    nombrePokemon = nombrePokemon.ToUpper();
                    bool PokemonEncontrado = false;
                    bool PokemonCambiado = false;


                    foreach (Pokemon pokemon in
                             atacante
                                 .GetSeleccion()) // Intenta encontrar el Pokemon indicado en la selección del jugador
                    {
                        if (pokemon.GetNombre() == nombrePokemon)
                        {
                            if (pokemon.GetVida() > 0)
                            {
                                // Si encontró al Pokemon y todavía está vivo, realiza el cambio exitosamente
                                atacante.UsarPokemon(pokemon); // Usar Pokemon ya borra el pokemon que tienes usado.
                                mensaje = "Ahora tu Pokemon en uso es " + pokemon.GetNombre();
                                await this.EnviarAUsuario(user, mensaje);
                                PokemonEncontrado = true;
                                PokemonCambiado = true;
                            }
                            else
                            {
                                // Si encontró al Pokemon, pero está muerto, cancela el cambio
                                mensaje = "Ese Pokemon está muerto, no puedes elegirlo";
                                await this.EnviarAUsuario(user, mensaje);
                                PokemonEncontrado = true;
                            }
                        }
                    }

                    if (PokemonEncontrado == false)
                    {
                        // Si no encontró el Pokemon en la selección del jugador.
                        mensaje = "No se encontró ese Pokemon en tu selección";
                        await this.EnviarAUsuario(user, mensaje);
                    }
                    else if (PokemonEncontrado == true && PokemonCambiado == true) // si encontró el pokemon y lo cambió
                    {
                        CambiarTurno(userId);
                        Entrenador oponente = batalla.GetEntrenadorOponente(userId);
                        mensaje = $"Concluíste tu turno.\n" +
                                  $"__**ES EL TURNO DE {oponente.GetNombre()}**__";
                        await EnviarAUsuario(atacante.GetSocketGuildUser(), mensaje);

                        string mensajeOponente =
                            $"Tu oponente {atacante.GetNombre()} ha decidido cambiar su pokemon por {nombrePokemon}";
                        await EnviarAUsuario(oponente.GetSocketGuildUser(), mensajeOponente);
                        await ComienzoDeTurno(batalla);
                    }
                }
            }
            else
            {
                string mensje = "**NO ES TU TURNO AUN, espera a que tu contrincante termine su turno**\n";
                await EnviarAUsuario(atacante.GetSocketGuildUser(), mensje);
            }
        }

        public async Task UsarPocion(ulong userID, string? entrada)
        {
            Battle batalla = BattlesList.GetBattle(userID);
            Entrenador jugador = batalla.GetEntrenadorActual(userID);
            Entrenador oponente = batalla.GetEntrenadorOponente(jugador.Id);
            var textoIngresado = this.ProcesarEntrada(entrada);
            int? numEleccion = textoIngresado.numero;
            string? pokemon = textoIngresado.nombre;
            string mensaje = "";

            //SOLO LO PUEDE EJECUTAR EL JUGADOR CON TURNO
            if (jugador == batalla.EntrenadorConTurno)
            {
                if (numEleccion == null)
                {
                    await this.ShowItemList(jugador);
                }
                else if (numEleccion < 1 || numEleccion > 3)
                {
                    mensaje = "El número de opción ingresado es inválido. Por favor ingresa un número válido";
                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                    
                }
                else
                {
                    if (pokemon == null)
                    {
                        mensaje = "Elige qué pokemon recibirá la poción. \n" +
                                  "**Utiliza el comando:** `!UsarPocion <númeroPoción> <pokemonQueRecibe>`\n" +
                                  "Por ejemplo: `!UsarPocion 1 Pikachu`\n";
                        await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                    }
                    else if (jugador.GetPokemonEnListaVivos(pokemon) == null &&
                             jugador.GetPokemonEnListaMuertos(pokemon) == null)
                    {
                        mensaje = "El pokemon ingresado no se encontró entre tus pokemones. Intentalo de nuevo.\n";
                        await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                    }
                    else
                    {
                        switch (numEleccion)
                        {
                            case 1:
                            {
                                if (jugador.GetCantidadItem("Súper Poción") <= 0)
                                {
                                    mensaje = "No tienes más Súper Pociones. Elige otra poción o intenta otra acción.\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                }
                                else if (jugador.GetPokemonEnListaVivos(pokemon) == null)
                                {
                                    mensaje = "¡No puedes darle una Súper Poción a uno de tus pokemones vencidos! " +
                                              "Intenta con otro pokemon u otra poción.\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                }
                                else
                                {
                                    Pokemon pokemonReceptor = jugador.GetPokemonEnListaVivos(pokemon);
                                    Item pocion = jugador.RemoverItem("Súper Poción");
                                    pokemonReceptor.AceptarItem(pocion);

                                    mensaje = $"Le diste una **Súper Poción** a **{pokemonReceptor.GetNombre()}** y " +
                                              $"ahora tiene ❤️ {pokemonReceptor.GetVida()}\n";
                                    Console.WriteLine(mensaje);
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                    await this.EnviarACanal(CanalConsola.Instance, mensaje);
                                    
                                    mensaje = $"**¡Tu oponente decidió darle una Súper Poción a {pokemonReceptor.GetNombre()} y " +
                                              $"ahora tiene ❤️ {pokemonReceptor.GetVida()} !**\n";
                                    await this.EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);
                                    
                                    CambiarTurno(userID);
                                    mensaje = $"Concluíste tu turno.\n" +
                                              $"__**ES EL TURNO DE {oponente.GetNombre()}**__";
                                    Console.WriteLine(mensaje);
                                    await EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                    
                                    await ComienzoDeTurno(batalla);
                                }
                                break;
                            }
                            
                            case 2:
                                if (jugador.GetCantidadItem("Cura Total") <= 0)
                                {
                                    mensaje = "No tienes más pociones 'Cura Total'. Elige otra poción o intenta otra acción.\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                }
                                else if (jugador.GetPokemonEnListaVivos(pokemon) == null)
                                {
                                    mensaje = "¡No puedes darle una poción 'Cura Total' a uno de tus pokemones vencidos! " +
                                              "Intenta con otro pokemon u otra poción.\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                }
                                else
                                {
                                    Pokemon pokemonReceptor = jugador.GetPokemonEnListaVivos(pokemon);
                                    Item pocion = jugador.RemoverItem("Cura Total");
                                    pokemonReceptor.AceptarItem(pocion);
                                    
                                    mensaje = $"Le diste una poción '**Cura Total**' a **{pokemonReceptor.GetNombre()}** y " +
                                              $"ahora está 👍🏻 SALUDABLE\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                    
                                    mensaje = $"**¡Tu oponente decidió darle una poción 'Cura Total' a {pokemonReceptor.GetNombre()} y " +
                                              $"ahora está 👍🏻 SALUDABLE !**\n";
                                    await this.EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);
                                    
                                    CambiarTurno(userID);
                                    mensaje = $"Concluíste tu turno.\n" +
                                              $"__**ES EL TURNO DE {oponente.GetNombre()}**__";
                                    await EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                    await ComienzoDeTurno(batalla);
                                }
                                break;
                            
                            case 3:
                                if (jugador.GetCantidadItem("Revivir") <= 0)
                                {
                                    mensaje = "No tienes más pociones 'Revivir'. Elige otra poción o intenta otra acción.\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                }
                                else if (jugador.GetPokemonEnListaMuertos(pokemon) == null)
                                {
                                    mensaje = "¡No puedes darle una poción 'Revivir' a uno de tus pokemones vivos! " +
                                              "Intenta con otro pokemon u otra poción.\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                }
                                else
                                {
                                    Pokemon pokemonReceptor = jugador.GetPokemonEnListaMuertos(pokemon);
                                    Item pocion = jugador.RemoverItem("Revivir");
                                    jugador.RemoverDeListaMuertos(pokemonReceptor);
                                    pokemonReceptor.AceptarItem(pocion);
                                    
                                    mensaje = $"Le diste una poción '**Revivir**' a **{pokemonReceptor.GetNombre()}** y " +
                                              $"ahora tiene ❤️ {pokemonReceptor.GetVida()}\n";
                                    await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                    
                                    mensaje = $"**¡Tu oponente decidió darle una poción 'Cura Total' a {pokemonReceptor.GetNombre()} y " +
                                              $"ahora tiene ❤️ {pokemonReceptor.GetVida()} !**\n";
                                    await this.EnviarAUsuario(oponente.GetSocketGuildUser(), mensaje);
                                    
                                    CambiarTurno(userID);
                                    mensaje = $"Concluíste tu turno.\n" +
                                              $"__**ES EL TURNO DE {oponente.GetNombre()}**__";
                                    await EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
                                    await ComienzoDeTurno(batalla);
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                string mensje = "**NO ES TU TURNO AUN, espera a que tu contrincante termine su turno**\n";
                await EnviarAUsuario(jugador.GetSocketGuildUser(), mensje);
            }
        }

        /// <summary>
        /// Función interna para subdividir strings que van
        /// con los comandos en dos datos diferentes.
        /// </summary>
        /// <param name="entrada"></param>
        /// <returns></returns>
        private (int? numero, string? nombre) ProcesarEntrada(string entrada)
        {
            if (string.IsNullOrWhiteSpace(entrada))
            {
                return (null, null);
            }

            // Divide la entrada en dos partes
            string[] partes = entrada.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            // Verifica si la primera parte es un número
            if (int.TryParse(partes[0], out int numero))
            {
                string nombre;
                // Si hay más partes, toma la segunda como nombre
                if (partes.Length > 1)
                {
                    nombre = partes[1];
                }
                // De lo contrario, el nombre es null
                else
                {
                    nombre = null;
                }
                return (numero, nombre);
            }

            // Si no hay un número inicial, devuelve null
            return (null, null);
        }
        
        public async Task ShowItemList(Entrenador jugador)
        {
            SuperPocion superPocion = new SuperPocion();
            CuraTotal curaTotal = new CuraTotal();
            Revivir revivir = new Revivir();
            string mensaje = $"__**Estas son tus pociones disponibles:**__\n\n" +
                             $"**1) Súper Poción** (x{jugador.GetCantidadItem("Súper Poción")}):\n" +
                             $"{superPocion.DescribirItem()}\n\n" +
                             $"**2) Cura Total** (x{jugador.GetCantidadItem("Cura Total")})\n" +
                             $"{curaTotal.DescribirItem()}\n\n" +
                             $"**3) Revivir** (x{jugador.GetCantidadItem("Revivir")})\n" +
                             $"{revivir.DescribirItem()}\n\n" +
                             $"\n**Utiliza el comando:** `!UsarPocion <númeroPoción> <pokemonQueLaRecibe>`\n" +
                             $"Por ejemplo: `!UsarPocion 1 Pikachu`\n";
            
            await this.EnviarAUsuario(jugador.GetSocketGuildUser(), mensaje);
            await this.EnviarACanal(CanalConsola.Instance, mensaje);
        }

        public async Task Rendirse(ulong userID)
        {
            Battle batalla = BattlesList.GetBattle(userID);
            Entrenador atacante = batalla.GetEntrenadorActual(userID);
            Entrenador oponente = batalla.GetEntrenadorOponente(userID);
            SocketGuildUser user = atacante.GetSocketGuildUser();
            
            batalla.Ganador = oponente; 
            this.ChequeoPantallaFinal(userID, batalla);

        }
        
        public void CambiarTurno(ulong userId)
        {
            Battle batalla = BattlesList.GetBattle(userId);
            batalla.CambiarTurno();
        }

        private async Task<bool> ChequeoPantallaFinal(ulong userId, Battle battle)
        {
            bool result = false;
            // Pregunta si el jugador sin turno se rindio
            if (battle.Ganador == null)
            {
                Entrenador entrenador = battle.GetEntrenadorActual(userId);
                Entrenador oponente = battle.GetEntrenadorOponente(userId);

                if (oponente.GetCantidadPokemonesVivos() == 0)
                {
                    battle.Ganador = entrenador;
                    result = true;
                    string mensajeGanador = $"🎊  **¡¡Felicitaciones has ganado la batalla, sigue asi!!**  🎊\n";

                    string mensajeOponente =
                        $"El entrenador **{battle.Ganador.GetNombre()}** ha sido el vencedor de este encuentro. 😢\n";

                    await EnviarAUsuario(battle.Ganador.GetSocketGuildUser(), mensajeGanador);

                    if (battle.Ganador == battle.Player1)
                    {
                        EnviarAUsuario(battle.Player2.GetSocketGuildUser(), mensajeOponente);
                    }
                    else
                    {
                        EnviarAUsuario(battle.Player1.GetSocketGuildUser(), mensajeOponente);
                    }
                    
                    BattlesList.RemoveBattle(battle);
                    return result;
                }
            }

            // Si Ganador no es null, es porque alguien llamó al método rendirse
            else
            {
                Entrenador perdedor;
                if (battle.Ganador == battle.Player1)
                {
                    perdedor = battle.Player2;
                }
                else
                {
                    perdedor = battle.Player1;
                }
                string mensajeGeneral = "====================================================================\n" +
                                        $"                  🎊  ¡Ha ganado **{battle.Ganador.GetNombre()}** porque **{perdedor.GetNombre()}** se ha rendido!  🎊\n" +
                                        "\n                                                      **~ Fin de la partida ~**" +
                                        "\n====================================================================";

                string mensajeGanador = "**¡Tu oponente decidió rendirse, felicitaciones!**\n" + mensajeGeneral;

                string mensajeRendido = "Te has rendido, sigue practicando. Mucha suerte en tus próximas batallas\n" + mensajeGeneral;

                await EnviarAUsuario(battle.Ganador.GetSocketGuildUser(), mensajeGanador);
                
                await EnviarAUsuario(perdedor.GetSocketGuildUser(), mensajeRendido);
                
                BattlesList.RemoveBattle(battle);
                result = true;
            }
            return result;
        }

        
    }
}




