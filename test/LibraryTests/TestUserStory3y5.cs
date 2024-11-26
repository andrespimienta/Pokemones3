using NUnit.Framework;
using Proyecto_Pokemones_I;

/*namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory3y5
{
    private Fachada1 fachada1;

    // Este método se ejecutará antes de cada prueba, garantizando que se crea una nueva instancia de Fachada.
    [SetUp]
    public void SetUp()
    {
        fachada1 = Fachada1.GetInstancia();   // Crea una nueva instancia de Fachada para cada test
        fachada1.LimpiarListaDeJugadores();
    }
    [Test]
    // "3. Como jugador, quiero ver la cantidad de vida (HP) de mis Pokémons y de los Pokémons oponentes para saber cuánta salud tienen."
    // "5. Como jugador, quiero saber de quién es el turno para estar seguro de cuándo atacar o esperar."
    public void VerVidaDePokemonesProgram()
    {
        fachada1.AgregarJugadorALista("1");
        fachada1.AgregarJugadorALista("2");
        fachada1.entrenadorConTurno = fachada1.Jugadores[0];


        // Seleccionamos los Pokémon de ambos jugadores
        for (int j = 0; j <= 1; j++)
        {
            // Lo repite para los dos jugadores
            fachada1.ElegirPokemon("Pikachu");
            string input = "PIKACHU"; // Simula la selección de Pokémon
            fachada1.CambiarPokemonPor(input);
            fachada1.CambiarTurno();
        }

        // Verificar quién empieza según la velocidad
        fachada1.ChequearQuienEmpieza();
        Console.WriteLine($"{fachada1.GetJugadorConTurno().GetNombre()} tiene al Pokémon más rápido");

        // Capturar la salida de la consola para verificar la vida de los Pokémon
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Llamamos a InformeDeSituacion
        fachada1.InformeDeSituacion();

        // Verificamos que la salida de la consola coincida con la vida de los Pokémon
        string outputEsperadoTurno = "El turno es de 1, El Pokémon usado es PIKACHU, vida = 35/35, su estado = consciente";
        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno));

        string outputEsperadoOponente = "Tu oponente es 2, El Pokémon usado es PIKACHU, vida = 35/35, su estado = consciente";
        Assert.That(consoleOutput.ToString().Contains(outputEsperadoOponente));

    }
}
*/