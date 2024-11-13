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
        Fachada fachada = Fachada.GetInstancia();
        fachada.LimpiarListaDeJugadores();

        fachada.AgregarJugadorALista("A");
        fachada.AgregarJugadorALista("B");
        fachada.entrenadorConTurno = fachada.Jugadores[0];
        

        for (int j = 0; j <= 1; j++)
        {
            // lo repite para los dos jugadores
            fachada.ElegirPokemon("PIKACHU");
            fachada.ElegirPokemon("CHARMANDER");
            fachada.ElegirPokemon("BULBASAUR");
            fachada.ElegirPokemon("SQUIRTLE");
            fachada.ElegirPokemon("EEVEE");
            fachada.ElegirPokemon("JIGGLYPUFF");
            fachada.CambiarTurno();
        }

        fachada.entrenadorConTurno = fachada.GetJugadorConTurno(); // para manejar mas facil la variable
        for (int i = 0; i <= 1; i++)
        {
            fachada.CambiarPokemonPor("PIKACHU");
            fachada.CambiarTurno();
            fachada.entrenadorConTurno = fachada.GetJugadorConTurno();
        }

        fachada.ChequearQuienEmpieza();
        Console.WriteLine($"{fachada.GetJugadorConTurno().GetNombre()} tiene al Pokemon más rápido");
        fachada.InformeDeSituacion();
        bool operacionExitosa = false; // chequea si se realizó alguna operación con éxito
        // de lo contrario muestra el menu principal de nuevo 

        // Verificar que los ataques se muestren al jugador en el turno actual
        Assert.That(fachada.ListaAtaques(), Is.EqualTo("IMPACTRUENO RAYO PUÑO TRUENO ATAQUE RÁPIDO"));
        
    }
}