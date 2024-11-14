using NUnit.Framework;
using Proyecto_Pokemones_I;


namespace TestLibrary;

public class TestUserStory2
{
    [Test]
    // "2. Como jugador, quiero ver los ataques disponibles de mis Pokémons para poder elegir cuál usar en cada turno."
    public void VerAtaquesDePokemonesProgram()
    {
        DiccionarioTipos.GetInstancia(); // Instancia el Singleton y define el contenido de todos sus diccionarios
        Fachada1 fachada1 = Fachada1.GetInstancia();
        fachada1.LimpiarListaDeJugadores();

        fachada1.AgregarJugadorALista("A");
        fachada1.AgregarJugadorALista("B");
        fachada1.entrenadorConTurno = fachada1.Jugadores[0];
        

        for (int j = 0; j <= 1; j++)
        {
            // lo repite para los dos jugadores
            fachada1.ElegirPokemon("PIKACHU");
            fachada1.ElegirPokemon("CHARMANDER");
            fachada1.ElegirPokemon("BULBASAUR");
            fachada1.ElegirPokemon("SQUIRTLE");
            fachada1.ElegirPokemon("EEVEE");
            fachada1.ElegirPokemon("JIGGLYPUFF");
            fachada1.CambiarTurno();
        }

        fachada1.entrenadorConTurno = fachada1.GetJugadorConTurno(); // para manejar mas facil la variable
        for (int i = 0; i <= 1; i++)
        {
            fachada1.CambiarPokemonPor("PIKACHU");
            fachada1.CambiarTurno();
            fachada1.entrenadorConTurno = fachada1.GetJugadorConTurno();
        }

        fachada1.ChequearQuienEmpieza();
        Console.WriteLine($"{fachada1.GetJugadorConTurno().GetNombre()} tiene al Pokemon más rápido");
        fachada1.InformeDeSituacion();
        bool operacionExitosa = false; // chequea si se realizó alguna operación con éxito
        // de lo contrario muestra el menu principal de nuevo 

        // Verificar que los ataques se muestren al jugador en el turno actual
        Assert.That(fachada1.ListaAtaques(), Is.EqualTo("IMPACTRUENO RAYO PUÑO TRUENO ATAQUE RÁPIDO"));
        
    }
}