/*using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory7
{
    private Fachada1 fachada1;

    // Este método se ejecutará antes de cada prueba, garantizando que se crea una nueva instancia de Fachada.
    [SetUp]
    public void SetUp()
    {
        fachada1 = Fachada1.GetInstancia(); // Crea una nueva instancia de Fachada para cada test
        fachada1.LimpiarListaDeJugadores();
    }

    [Test]

    // "Como jugador, quiero poder cambiar de Pokémon durante una batalla."
 
    public void JugadorCambiaPokemon()
    {
        fachada1.AgregarJugadorALista("A");
        fachada1.AgregarJugadorALista("B");
        fachada1.entrenadorConTurno = fachada1.Jugadores[0];
        
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10000, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 100);
        List<Ataque> pikachuataques = new List<Ataque>();
        
        pikachuataques.Add(rayo);
        pikachuataques.Add(hidrobomba);
        
        Pokemon pikachu = new Pokemon("PIKACHU", "ELÉCTRICO", 100, 10, pikachuataques);

        Pokemon squirtle = new Pokemon("SQUIRTLE", "AGUA", 100, 10, pikachuataques);

        fachada1.entrenadorConTurno.AñadirASeleccion(squirtle); //Jugador A agrega a SQUIRTLE
        fachada1.entrenadorConTurno.AñadirASeleccion(pikachu); //Jugador A agrega a PIKACHU
        
        fachada1.CambiarTurno(); 

        fachada1.entrenadorConTurno.AñadirASeleccion(pikachu); //Jugador B agrega a PIKACHU
        fachada1.entrenadorConTurno.AñadirASeleccion(squirtle); //Jugador B agrega a SQUIRTLE

        fachada1.CambiarTurno(); 
        
        fachada1.CambiarPokemonPor("SQUIRTLE"); //Jugador A elije SQUIRTLE para combatir
        fachada1.CambiarTurno(); //Ahora el jugador con turno pasa a ser B
        fachada1.CambiarPokemonPor("PIKACHU"); //Jugador B elije PIKACHU para combatir
        fachada1.CambiarTurno(); //Ahora el jugador con turno vuelve a ser A

        //Simulamos que inicia el combate
        
        Assert.That(fachada1.entrenadorConTurno.GetPokemonEnUso(), Is.EqualTo(squirtle));  //Verifica el pokemón que está en uso de A
        fachada1.CambiarPokemonPor("PIKACHU"); //El jugador elige cambiar a su otro pokemon
        fachada1.CambiarTurno(); // Ahora lo hace el program pero proximamente lo hará la fachada
        Assert.That(fachada1.entrenadorSinTurno.GetPokemonEnUso(), Is.EqualTo(pikachu)); //Verifica el pokemón que se cambió en A
    }
}*/