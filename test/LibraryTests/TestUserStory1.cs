using Library.Bot.Dominio;
using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using NUnit.Framework;

namespace TestLibrary;

public class TestUserStory1
{
    [Test]
    // "1. Como jugador, quiero elegir 6 Pokémons del catálogo disponible para comenzar la batalla"
    public void Elige6pokemones()
    {
        // Crear el entrenador
        Entrenador j1 = new Entrenador("a", 3, null);

        // Crear un ataque y una lista de ataques
        AtaqueBasico ataque = new AtaqueBasico("IMPACTRUENO", "ELÉCTRICO", 40, 90);
        List<Ataque> Ataques = new List<Ataque> { ataque };

        // Crear 6 Pokémon y añadirlos a la selección del entrenador
        Pokemon p1 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, Ataques, "id1");
        Pokemon p2 = new Pokemon("CHARMANDER", "FUEGO", 35, 1.5, Ataques, "id2");
        Pokemon p3 = new Pokemon("MACHOP", "LUCHA", 232, 1.5, Ataques, "8");
        Pokemon p4 = new Pokemon("PKE", "ELÉCTRICO", 35, 1.5, Ataques, "id4");
        Pokemon p5 = new Pokemon("POKI", "ELÉCTRICO", 35, 1.5, Ataques, "id5");
        Pokemon p6 = new Pokemon("PUIKO", "ELÉCTRICO", 35, 1.5, Ataques, "id6");

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
        Fachada.Reset();
    }
}
