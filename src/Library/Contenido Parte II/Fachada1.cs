using System.Runtime.InteropServices.JavaScript;
/*
namespace Proyecto_Pokemones_I;

public class Fachada1
{
    // (Esta clase es un Singleton)
    private static Fachada1 instancia;
    
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

  
   
     

    public void ChequearQuienEmpieza()
    {
        if (entrenadorConTurno.GetPokemonEnUso().GetVelocidadAtaque() < entrenadorSinTurno.GetPokemonEnUso().GetVelocidadAtaque())
        {
            CambiarTurno();
        }
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


    */

