using System.Runtime.InteropServices.JavaScript;

namespace Proyecto_Pokemones_I;

public class Fachada
{
    // (Esta clase es un Singleton)
    private static Fachada instancia;
    
    // Atributos:
    public List<Entrenador> Jugadores { get; private set; }
    public Entrenador entrenadorConTurno;
    public Entrenador entrenadorSinTurno;
    private VisitorPorTurno visitorTurno;
    public bool existeGanador = false;

    // Getters:
    public Entrenador GetJugadorConTurno()
    {
        return this.entrenadorConTurno;
    }
    public Entrenador GetJugadorSinTurno()
    {
        return this.entrenadorSinTurno;
    }

    // Constructor:
    private Fachada()
    {
        Jugadores = new List<Entrenador>();
        visitorTurno = new VisitorPorTurno();
    }

    // Métodos: (En orden de aparición en la Batalla)
    public static Fachada GetInstancia()
    {
        if (instancia == null)
        {
            instancia = new Fachada();
        }
        return instancia;
    }
    
    public void AgregarJugadorALista(string nombreJugador)
    {
        // Validar que el nombre no esté vacío
        if (string.IsNullOrWhiteSpace(nombreJugador))
        {
            Console.WriteLine("El nombre no puede estar vacío. Por favor, inténtelo de nuevo.");
            return;
        }

        // Crear un nuevo jugador y agregarlo a la lista
        Entrenador jugador = new Entrenador(nombreJugador);
        Jugadores.Add(jugador);

        Console.WriteLine($"\n¡Bienvenido, {nombreJugador}! Has sido agregado a la lista de espera.");
        Console.WriteLine($"Jugadores en la lista de espera: {Jugadores.Count}/2");
    }
    
    public void ListaDeEspera()
    {
        Console.WriteLine("------------------ ¡BIENVENIDO A LA BATALLA POKÉMON! -------------------");
        Console.WriteLine("Prepárate para una aventura épica, Entrenador.");
        Console.WriteLine("Ingresa tu nombre para unirte a la batalla.");
        Console.WriteLine("Necesitamos al menos 2 jugadores para empezar.");
        Console.WriteLine("------------------------------------------------------------------------");

        // Bucle para agregar jugadores hasta tener al menos 2
        while (Jugadores.Count < 2)
        {
            Console.WriteLine("\nIngrese su nombre: ");
            string nombreJugador = Console.ReadLine();

            // Llamar al método para agregar jugador
            AgregarJugadorALista(nombreJugador);
            
            Console.WriteLine($"\nEsperando más jugadores para iniciar la batalla...");
        }

        // Asignar roles a los jugadores una vez que haya suficientes
        entrenadorConTurno = Jugadores[0];
        entrenadorSinTurno = Jugadores[1];

        Console.WriteLine("\n¡Ya hay suficientes jugadores! La batalla está a punto de comenzar...");
        Console.WriteLine($"El entrenador {entrenadorConTurno.GetNombre()} comenzará eligiendo");
        Console.WriteLine($"El entrenador {entrenadorSinTurno.GetNombre()} deberá esperar su turno de elegir.");
    }
    
    public void LimpiarListaDeJugadores()
    {
        Jugadores.Clear();  // Limpia todos los jugadores de la lista
    }
    
    public void MostrarCatalogo()
    {
        Console.WriteLine("======================================================================" +
                          "\n*--------------* Estos son los pokemones disponibles: *--------------*\n" +
                          "======================================================================");
        LeerArchivo.ImprimirCatalogoProcesado();
        Console.WriteLine("======================================================================");
    }
    
    public bool ElegirPokemon(string nombrePokemon)
    {
        bool elecciónExitosa = true;    // Una simple variable para indicarle a Program si se efectuó o no la elección

        Pokemon pokemonElegido = LeerArchivo.EncontrarPokemon(nombrePokemon); // Intenta buscar el Pokemon indicado en el catálogo
        if (pokemonElegido == null)     // Si no lo encontró o dio error, se cancela la elección del Pokemon
        {
            elecciónExitosa = false;
            return elecciónExitosa;
        }

        foreach (Pokemon pokemon in entrenadorConTurno.GetSeleccion()) // Intenta buscar el Pokemon indicado en la selección del jugador
        {
            if (pokemon.GetNombre() == nombrePokemon)   // Si el Pokemon ya estaba en la selección, se cancela la elección del Pokemon
            {
                Console.WriteLine("¡Ya habías añadido ese Pokemon a tu selección!");
                elecciónExitosa = false;
                return elecciónExitosa;
            }
        }

        entrenadorConTurno.AñadirASeleccion(pokemonElegido);   // Si no se dio ninguno de los casos anteriores, añade al pokemon a la selección
        return elecciónExitosa;
    }

    public void CambiarTurno()
    {
        if (entrenadorConTurno == Jugadores[0])
        {
            entrenadorConTurno = Jugadores[1];
            entrenadorSinTurno = Jugadores[0];
        }
        else if (entrenadorConTurno == Jugadores[1])
        {
            entrenadorConTurno = Jugadores[0];
            entrenadorSinTurno = Jugadores[1];
        }
        entrenadorConTurno.AceptarVisitorPorTurno(this.visitorTurno);
    }
    
    public void ChequearQuienEmpieza()
    {
        if (entrenadorConTurno.GetPokemonEnUso().GetVelocidadAtaque() < entrenadorSinTurno.GetPokemonEnUso().GetVelocidadAtaque())
        {
            CambiarTurno();
        }
    }
    
    public void InformeDeSituacion()
    {
        Console.WriteLine($"\n El turno es de {entrenadorConTurno.GetNombre()}, " +
                          $"El Pokémon usado es {entrenadorConTurno.GetPokemonEnUso().GetNombre()}, " +
                          $"vida = {(entrenadorConTurno.GetPokemonEnUso().GetVida() <= 0 ? "muerto" : entrenadorConTurno.GetPokemonEnUso().GetVida().ToString())}/{entrenadorConTurno.GetPokemonEnUso().GetVidaMax()}" +
                          $"{(entrenadorConTurno.GetPokemonEnUso().GetVida() > 0 ? $", su estado = {(entrenadorConTurno.GetPokemonEnUso().EfectoActivo ?? "consciente")}\n" : "")}\n" +
                          $"Tu oponente es {entrenadorSinTurno.GetNombre()}, " +
                          $"El Pokémon usado es {entrenadorSinTurno.GetPokemonEnUso().GetNombre()}, " +
                          $"vida = {(entrenadorSinTurno.GetPokemonEnUso().GetVida() <= 0 ? "muerto" : entrenadorSinTurno.GetPokemonEnUso().GetVida().ToString())}/{entrenadorSinTurno.GetPokemonEnUso().GetVidaMax()}" +
                          $"{(entrenadorSinTurno.GetPokemonEnUso().GetVida() > 0 ? $", su estado = {(entrenadorSinTurno.GetPokemonEnUso().EfectoActivo ?? "consciente")}" : "")}\n");
    }
    
    public string ListaAtaques()
    {
        string resultado = "";
        
        foreach (IAtaque ataque in this.entrenadorConTurno.GetPokemonEnUso().GetAtaques())
        {
            string palabraAux = ataque.GetNombre();
            Console.Write(palabraAux + " / "); // Imprime cada nombre seguido de un espacio
            resultado += palabraAux + " ";   // Agrega cada nombre a la cadena `resultado` seguido de un espacio
        }
        return resultado.Trim(); // Elimina el último espacio extra al final de la cadena
    }

    public bool Atacar(string nombreAtaque)
    {
        Pokemon pokemonVictima = entrenadorSinTurno.GetPokemonEnUso();
        Pokemon pokemonAtacante = entrenadorConTurno.GetPokemonEnUso();
        
        bool ataqueExitoso = true;
        
        // Si es el turno del Jugador 1, intentará efectuar el ataque indicado sobre el Pokemon en Uso del Jugador 2
        foreach (IAtaque ataque in pokemonAtacante.GetAtaques())
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
                            Console.WriteLine($"{pokemonVictima.GetNombre()} ha sido vencido");
                        }
                        else
                        {
                            Console.WriteLine(
                                $"{pokemonVictima.GetNombre()} ha sufrido daño, su vida es {pokemonVictima.GetVida()}");
                        }
                    }
                return ataqueExitoso;
            }
        }
        
        // Si sale del Foreach sin haber retornado antes, es que no encontró el ataque
        Console.WriteLine("No se encontró el ataque");
        ataqueExitoso = false;
        return ataqueExitoso;  
    }

    public bool CambiarPokemonPor(string nombrePokemon)
    {
        // FALTA AGREGAR CASO LÍMITE EN QUE SE ELIGE EL MISMO POKEMON QUE YA ESTÁ EN USO
        bool cambioExitoso = true;  // Una simple variable para indicarle a Program si se efectuó o no el cambio
        
        foreach (Pokemon pokemon in entrenadorConTurno.GetSeleccion()) // Intenta encontrar el Pokemon indicado en la selección del jugador
        {
            if (pokemon.GetNombre() == nombrePokemon)
            {
                if (pokemon.GetVida() > 0)      
                {
                    // Si encontró al Pokemon y todavía está vivo, realiza el cambio exitosamente
                    entrenadorConTurno.GuardarPokemon();
                    entrenadorConTurno.UsarPokemon(pokemon);
                    return cambioExitoso;
                }
                else
                {
                    // Si encontró al Pokemon, pero está muerto, cancela el cambio
                    Console.WriteLine("Ese Pokemon está muerto, no puedes elegirlo");
                    cambioExitoso = false;  
                    return cambioExitoso;
                }
            }
        }
        // Si llegó a este punto es porque no encontró el Pokemon en la selección del jugador, por lo que cancela el cambio
        Console.WriteLine("No se encontró ese Pokemon en tu selección");
        cambioExitoso = false;
        return cambioExitoso;
    }
    
    public bool ChequeoPantallaFinal()
    {
        if (existeGanador == true) // el jugador con turno se rindio
        {
            Console.WriteLine("----------------------------------------------------------------------\n" +
                              $"\n¡Ha ganado {entrenadorSinTurno.GetNombre()} porque tu oponente se ha rendido, felicidades! \n" +
                              "\nFin de la partida \n" + 
                              "----------------------------------------------------------------------");
            return existeGanador;
        }
        // Si el Jugador 2 no tiene más Pokemones vivos, gana el Jugador 1, imprime la pantalla y devuelve que hay un ganador
        else if (entrenadorConTurno.GetPokemonesVivos() == 0) 
        {
            Console.WriteLine("-----------------------------------------------------------------------\n" +
                              $"\n¡Ha ganado {entrenadorSinTurno.GetNombre()}, felicidades! \n" +
                              "\nFin de la partida \n" + 
                              "----------------------------------------------------------------------");
            existeGanador = true;
            return existeGanador;
        }
        
        // Si ambos tienen Pokemones vivos, no hay ganador, devuelve el valor por defecto (false)
        else
        {
            return existeGanador;
        }
    }
    
}


    

