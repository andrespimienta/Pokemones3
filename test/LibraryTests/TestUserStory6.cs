using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory6
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

    // "Como jugador, quiero ganar la batalla cuando la vida de todos los Pokémons oponente llegue a cero."

    public void JugadorGanaLaPartida()
    {
        Entrenador a = new Entrenador("A");
        Entrenador b = new Entrenador("B");
        fachada.entrenadorConTurno = a;
        fachada.entrenadorSinTurno = b;
        
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10000, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 100);
        List<IAtaque> pikachuataques = new List<IAtaque>();
        
        pikachuataques.Add(rayo);
        pikachuataques.Add(hidrobomba);
        
        Pokemon pikachu = new Pokemon("PIKACHU", "ELÉCTRICO", 100, 10, pikachuataques);

        Pokemon squirtle = new Pokemon("SQUIRTLE", "AGUA", 100, 10, pikachuataques);

        fachada.entrenadorConTurno.AñadirASeleccion(squirtle); //Jugador A agrega a SQUIRTLE
        fachada.entrenadorSinTurno.AñadirASeleccion(pikachu); //Jugador B agrega a PIKACHU
        
        squirtle.RecibirDaño(rayo); //PIKACHU ataca a Squirtle y lo mata

        // Capturar la salida de la consola para verificar la vida de los Pokémon
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        fachada.ChequeoPantallaFinal(); // Verifica si hay un ganador

        // Verificamos que la salida de la consola coincida con avisar el jugador B salio Ganador
        string outputEsperadoTurno = "-----------------------------------------------------------------------\n" +
                                     $"\n¡Ha ganado B, felicidades! \n" +
                                     "\nFin de la partida \n" + 
                                     "----------------------------------------------------------------------";

        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno));
    }
}