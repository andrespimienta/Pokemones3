using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

public class TestUserStory1
{
    [Test]
    // "1. Como jugador, quiero elegir 6 Pokémons del catálogo disponible para comenzar la batalla"
    public void Elige6pokemones()
    {
        // Crear el entrenador
        Entrenador j1 = new Entrenador("a");

        // Crear un ataque y una lista de ataques
        AtaqueBasico ataque = new AtaqueBasico("IMPACTRUENO", "ELÉCTRICO", 40, 90);
        List<IAtaque> Ataques = new List<IAtaque> { ataque };

        // Crear 6 Pokémon y añadirlos a la selección del entrenador
        Pokemon p1 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques);
        Pokemon p2 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques);
        Pokemon p3 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques);
        Pokemon p4 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques);
        Pokemon p5 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques);
        Pokemon p6 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques);

        j1.AñadirASeleccion(p1);
        j1.AñadirASeleccion(p2);
        j1.AñadirASeleccion(p3);
        j1.AñadirASeleccion(p4);
        j1.AñadirASeleccion(p5);
        j1.AñadirASeleccion(p6);

        // Contar los Pokemones en la lista del entrenador
        int contadorPokemones = 0;
        foreach (var pokemones in j1.GetSeleccion())
        {
            contadorPokemones++;
        }

        // Retorna que debe haber 6 Pokémon en la selección del entrenador
        Assert.That(contadorPokemones, Is.EqualTo(6));
    }

    [Test]
    public void Elige6pokemonesProgram()
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
            fachada.ElegirPokemon("Pikachu");
            fachada.ElegirPokemon("CHARMANDER");
            fachada.ElegirPokemon("BULBASAUR");
            fachada.ElegirPokemon("SQUIRTLE");
            fachada.ElegirPokemon("EEVEE");
            fachada.ElegirPokemon("JIGGLYPUFF");
            fachada.CambiarTurno();
        }
        
        // Asegúrate de que hay exactamente 2 jugadores en la lista antes de contar
        Assert.That(fachada.Jugadores.Count, Is.EqualTo(2));

        // Contador para el primer jugador
        int contadorPokemonesJugador1 = 0;
        foreach (var pokemon in fachada.Jugadores[0].GetSeleccion())
        {
            contadorPokemonesJugador1++;
        }

        // Contador para el segundo jugador
        int contadorPokemonesJugador2 = 0;
        foreach (var pokemon in fachada.Jugadores[1].GetSeleccion())
        {
            contadorPokemonesJugador2++;
        }

        // Asegurar que ambos jugadores tengan exactamente 6 Pokémon
        Assert.That(contadorPokemonesJugador1, Is.EqualTo(6), $"El jugador {fachada.Jugadores[0].GetNombre()} no tiene 6 Pokémon.");
        Assert.That(contadorPokemonesJugador2, Is.EqualTo(6), $"El jugador {fachada.Jugadores[1].GetNombre()} no tiene 6 Pokémon.");
    }
}
