using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory6
{
    private Fachada1 fachada1;

    // Este método se ejecutará antes de cada prueba, garantizando que se crea una nueva instancia de Fachada.
    [SetUp]
    public void SetUp()
    {
        fachada1 = Fachada1.GetInstancia(); // Crea una nueva instancia de Fachada para cada test
        fachada1.LimpiarLisdores();taDeJuga
    }

    [Test]

    // "Como jugador, quiero ganar la batalla cuando la vida de todos los Pokémons oponente llegue a cero."

    public void JugadorGanaLaPartida()
    {
        Entrenador a = new Entrenador("A");
        Entrenador b = new Entrenador("B");
        fachada1.entrenadorConTurno = a;
        fachada1.entrenadorSinTurno = b;
        
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10000, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 100);
        List<Ataque> pikachuataques = new List<Ataque>();
        
        pikachuataques.Add(rayo);
        pikachuataques.Add(hidrobomba);
        
        Pokemon pikachu = new Pokemon("PIKACHU", "ELÉCTRICO", 100, 10, pikachuataques);

        Pokemon squirtle = new Pokemon("SQUIRTLE", "AGUA", 100, 10, pikachuataques);

        fachada1.entrenadorConTurno.AñadirASeleccion(squirtle); //Jugador A agrega a SQUIRTLE
        fachada1.entrenadorSinTurno.AñadirASeleccion(pikachu); //Jugador B agrega a PIKACHU
        
        squirtle.RecibirDaño(rayo); //PIKACHU ataca a Squirtle y lo mata

        // Capturar la salida de la consola para verificar la vida de los Pokémon
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        fachada1.ChequeoPantallaFinal(); // Verifica si hay un ganador

        // Verificamos que la salida de la consola coincida con avisar el jugador B salio Ganador
        string outputEsperadoTurno = "-----------------------------------------------------------------------\n" +
                                     $"\n¡Ha ganado B, felicidades! \n" +
                                     "\nFin de la partida \n" + 
                                     "----------------------------------------------------------------------";

        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno));
    }
}