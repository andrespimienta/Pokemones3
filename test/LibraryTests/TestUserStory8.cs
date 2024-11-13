using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory8
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

    // "Como entrenador, quiero poder usar un ítem durante una batalla."
 
    public void JugadorUsaItem()
    {
        fachada.AgregarJugadorALista("A");
        fachada.AgregarJugadorALista("B");
        fachada.entrenadorConTurno = fachada.Jugadores[0];
        
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10000, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 99);
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
        
        Assert.That(pikachu.GetVida(), Is.EqualTo(100)); //Verifica que Pikachu tenga toda la vida
        pikachu.RecibirDaño(hidrobomba);
        Assert.That(pikachu.GetVida(), Is.LessThan(100)); //Verifica que el ataque le haya bajado la vida
        
        fachada.entrenadorConTurno.UsarItem("Cura total"); //El jugador usa el item de Cura total
        
        Assert.That(pikachu.GetVida(),  Is.GreaterThan(87)); //Verifica que Pikachu vuelva a tener más vida de la que se le quito
    }
}