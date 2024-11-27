using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;

namespace TestLibrary;

public class TestUserStory1
{
    [Test]
    // "1. Como jugador, quiero elegir 6 Pokémons del catálogo disponible para comenzar la batalla"
    public void Elige6pokemones()
    {
        // Crear el entrenador
        Entrenador j1 = new Entrenador("a", 435646547645, null);

        // Crear un ataque y una lista de ataques
        AtaqueBasico ataque = new AtaqueBasico("IMPACTRUENO", "ELÉCTRICO", 40, 90);
        List<Ataque> Ataques = new List<Ataque> { ataque };

        // Crear 6 Pokémon y añadirlos a la selección del entrenador
        Pokemon p1 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques, "id1" );
        Pokemon p2 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques, "id2");
        Pokemon p3 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques, "id3");
        Pokemon p4 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques, "id4");
        Pokemon p5 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques, "id5");
        Pokemon p6 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques, "id6");

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
        Fachada fachada1 = Fachada.Instance;
        //fachada1.LimpiarListaDeJugadores();
        
        fachada1.AddTrainerToWaitingList( 435646547645,"A", null, null);
        fachada1.AddTrainerToWaitingList( 435646547645, "B", null, null);
        fachada1.EntrenadorConTurno = fachada1.Jugadores[0];
        
        for (int j = 0; j <= 1; j++)
        {
            // lo repite para los dos jugadores
            fachada1.AddPokemonToList(1, "1");
            fachada1.AddPokemonToList(2, "2");
            fachada1.AddPokemonToList(3, "3");
            fachada1.AddPokemonToList(4, "4");
            fachada1.AddPokemonToList(5, "5");
            fachada1.AddPokemonToList(6, "6");
            fachada1.CambiarTurno(1);
        }
        
        // Asegúrate de que hay exactamente 2 jugadores en la lista antes de contar
        Assert.That(fachada1.Jugadores.Count, Is.EqualTo(2));

        // Contador para el primer jugador
        int contadorPokemonesJugador1 = 0;
        foreach (var pokemon in fachada1.Jugadores[0].GetSeleccion())
        {
            contadorPokemonesJugador1++;
        }

        // Contador para el segundo jugador
        int contadorPokemonesJugador2 = 0;
        foreach (var pokemon in fachada1.Jugadores[1].GetSeleccion())
        {
            contadorPokemonesJugador2++;
        }

        // Asegurar que ambos jugadores tengan exactamente 6 Pokémon
        Assert.That(contadorPokemonesJugador1, Is.EqualTo(6), $"El jugador {fachada1.Jugadores[0].GetNombre()} no tiene 6 Pokémon.");
        Assert.That(contadorPokemonesJugador2, Is.EqualTo(6), $"El jugador {fachada1.Jugadores[1].GetNombre()} no tiene 6 Pokémon.");
    }
}
