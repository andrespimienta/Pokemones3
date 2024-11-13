using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory7
{
    private Fachada fachada;

    // Este método se ejecutará antes de cada prueba, garantizando que se crea una nueva instancia de Fachada.
    [SetUp]
    public void SetUp()
    {
        DiccionarioTipos.GetInstancia(); // Inicializa el Singleton
        fachada = Fachada.GetInstancia(); // Crea una nueva instancia de Fachada para cada test
        fachada.LimpiarListaDeJugadores();
    }

    [Test]

    // "Como jugador, quiero poder cambiar de Pokémon durante una batalla."
 
    public void JugadorCambiaPokemon()
    {
        fachada.AgregarJugadorALista("A");
        fachada.AgregarJugadorALista("B");
        fachada.entrenadorConTurno = fachada.Jugadores[0];
        
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10000, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 100);
        List<IAtaque> pikachuataques = new List<IAtaque>();
        
        pikachuataques.Add(rayo);
        pikachuataques.Add(hidrobomba);
        
        Pokemon pikachu = new Pokemon("PIKACHU", "ELÉCTRICO", 100, 10, pikachuataques);

        Pokemon squirtle = new Pokemon("SQUIRTLE", "AGUA", 100, 10, pikachuataques);

        fachada.entrenadorConTurno.AñadirASeleccion(squirtle); //Jugador A agrega a SQUIRTLE
        fachada.entrenadorConTurno.AñadirASeleccion(pikachu); //Jugador A agrega a PIKACHU
        
        fachada.CambiarTurno(); 

        fachada.entrenadorConTurno.AñadirASeleccion(pikachu); //Jugador B agrega a PIKACHU
        fachada.entrenadorConTurno.AñadirASeleccion(squirtle); //Jugador B agrega a SQUIRTLE

        fachada.CambiarTurno(); 
        
        fachada.CambiarPokemonPor("SQUIRTLE"); //Jugador A elije SQUIRTLE para combatir
        fachada.CambiarTurno(); //Ahora el jugador con turno pasa a ser B
        fachada.CambiarPokemonPor("PIKACHU"); //Jugador B elije PIKACHU para combatir
        fachada.CambiarTurno(); //Ahora el jugador con turno vuelve a ser A

        //Simulamos que inicia el combate
        
        Assert.That(fachada.entrenadorConTurno.GetPokemonEnUso(), Is.EqualTo(squirtle));  //Verifica el pokemón que está en uso de A
        fachada.CambiarPokemonPor("PIKACHU"); //El jugador elige cambiar a su otro pokemon
        fachada.CambiarTurno(); // Ahora lo hace el program pero proximamente lo hará la fachada
        Assert.That(fachada.entrenadorSinTurno.GetPokemonEnUso(), Is.EqualTo(pikachu)); //Verifica el pokemón que se cambió en A
    }
}