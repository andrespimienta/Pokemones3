using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory4
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

    // "Como jugador, quiero atacar en mi turno y hacer daño basado en la efectividad de los tipos de Pokémon."

    public void DañoBasadoEnEfectividadDébiles()
    {
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 100);
        List<Ataque> pikachuataques = new List<Ataque>();
        pikachuataques.Add(rayo);
        pikachuataques.Add(hidrobomba);


        Pokemon pikachu = new Pokemon("PIKACHU", "ELÉCTRICO", 100, 10, pikachuataques);
        
        Pokemon squirtle = new Pokemon("SQUIRTLE", "AGUA", 100, 10, pikachuataques);
        
        // Capturar la salida de la consola para verificar la vida de los Pokémon
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        squirtle.RecibirDaño(rayo);

        // Verificamos que la salida de la consola coincida con avisar que el pokemon es débil al ataque que recibe
        string outputEsperadoTurno = "SQUIRTLE es débil a los ataques de tipo ELÉCTRICO, recibió el doble del daño";
        
        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno));
    }
    
    [Test]

    public void DañoBasadoEnEfectividadInmunes()
    {
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 100);
        List<Ataque> pikachuataques = new List<Ataque>();
        pikachuataques.Add(rayo);
        pikachuataques.Add(hidrobomba);


        Pokemon pikachu = new Pokemon("PIKACHU", "ELÉCTRICO", 100, 10, pikachuataques);
        
        Pokemon squirtle = new Pokemon("SQUIRTLE", "AGUA", 100, 10, pikachuataques);
        
        // Capturar la salida de la consola para verificar la vida de los Pokémon
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        pikachu.RecibirDaño(rayo);

        // Verificamos que la salida de la consola coincida con avisar que el ataque es inmune
        string outputEsperadoTurno = "PIKACHU es inmune a los ataques de tipo ELÉCTRICO, no recibió daño";
        
        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno));
    }
    
    [Test]
    
    public void DañoBasadoEnEfectividadResistentes()
    {
        AtaqueBasico rayo = new AtaqueBasico("RAYO", "ELÉCTRICO", 10, 100);
        AtaqueBasico hidrobomba = new AtaqueBasico("HIDROBOMBA", "AGUA", 10, 100);
        List<Ataque> pikachuataques = new List<Ataque>();
        pikachuataques.Add(rayo);
        pikachuataques.Add(hidrobomba);


        Pokemon pikachu = new Pokemon("PIKACHU", "ELÉCTRICO", 100, 10, pikachuataques);
        
        Pokemon squirtle = new Pokemon("SQUIRTLE", "AGUA", 100, 10, pikachuataques);
        
        // Capturar la salida de la consola para verificar la vida de los Pokémon
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        
        squirtle.RecibirDaño(hidrobomba);

        // Verificamos que la salida de la consola coincida con avisar que el ataque es resistente
        string outputEsperadoTurno = "SQUIRTLE es resistente a los ataques de tipo AGUA, recibió la mitad del daño";
        
        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno));
    }
}

