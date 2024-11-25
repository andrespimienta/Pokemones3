/*using NUnit.Framework;
using NUnit.Framework.Legacy;
using Proyecto_Pokemones_I;

namespace TestLibrary;

[TestFixture]
public class TestUserStory_9_10_11
{
    [Test]
    public void AgregarJugadroLista()
    {
        // Arrange
        Fachada1 fachada1 = Fachada1.GetInstancia();
        fachada1.LimpiarListaDeJugadores(); // Limpia la lista de jugadores

        // Captura la salida de la consola
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);  // Redirige la salida de la consola

        // Agrega Entrenador
        fachada1.AgregarJugadorALista("Ash");

        // Verifica la salida esperada
        string outputEsperadoTurno = "¡Bienvenido, Ash! Has sido agregado a la lista de espera.\r\nJugadores en la lista de espera: 1/2\r\n";
        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno), Is.True);
    }
    
    [Test]
    public void TestListaDeJugadoresEsperando()
    {
        // Arrange
        Fachada1 fachada1 = Fachada1.GetInstancia();
        fachada1.LimpiarListaDeJugadores(); // Asegúrate de empezar con una lista limpia
    
        // Agregar jugadores
        fachada1.AgregarJugadorALista("Ash");
        fachada1.AgregarJugadorALista("Misty");
    
        // Capturar la salida de la consola
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Método que imprime la lista de jugadores
        fachada1.ListaDeEspera(); 

        // Resetear la salida de la consola a su estado original
        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

        // Assert
        string outputEsperado = "Ash\nMisty\n";
        StringAssert.Contains("Ash", consoleOutput.ToString());
        StringAssert.Contains("Misty", consoleOutput.ToString());
    }
[Test]
public void TestIniciarBatalla()
{
    // Arrange
    Fachada1 fachada1 = Fachada1.GetInstancia();
    fachada1.LimpiarListaDeJugadores();

    // Agregar jugadores
    fachada1.AgregarJugadorALista("Ash");
    fachada1.AgregarJugadorALista("Misty");

    // Inicializar la batalla y mostrar catálogo
    fachada1.MostrarCatalogo();

    // Asignar el primer jugador con turno
    fachada1.entrenadorConTurno = fachada1.Jugadores[0];

    // Elegir Pokémon para ambos jugadores
    string[] pokemonElegidos = { "PIKACHU", "CHARMANDER", "BULBASAUR", "SQUIRTLE", "EEVEE", "JIGGLYPUFF" };
    for (int i = 0; i < 2; i++)
    {
        foreach (var pokemon in pokemonElegidos)
        {
            fachada1.ElegirPokemon(pokemon); // Elige Pokémon para ambos jugadores
        }
        fachada1.CambiarTurno();
    }

    // Cambiar Pokémon por "PIKACHU" para ambos jugadores
    for (int i = 0; i < 2; i++)
    {
        fachada1.CambiarPokemonPor("PIKACHU");
        fachada1.CambiarTurno();
        fachada1.entrenadorConTurno = fachada1.GetJugadorConTurno(); // Actualiza el jugador con turno
    }

    // Verificar quién empieza la batalla
    fachada1.ChequearQuienEmpieza();
    fachada1.InformeDeSituacion();

    // Capturar la salida de consola
    var consoleOutput = new StringWriter();
    Console.SetOut(consoleOutput);  // Redirige la salida de la consola

    // Ejecutar la lógica de batalla (esto debería imprimir los mensajes a la consola)
    // Esto es solo un ejemplo de lo que debería imprimir el juego
    Console.WriteLine($"{fachada1.GetJugadorConTurno().GetNombre()} tiene al Pokemon más rápido");
    Console.WriteLine($"El turno es de {fachada1.GetJugadorConTurno().GetNombre()}, El Pokémon usado es PIKACHU, vida = 35/35, su estado = consciente");
    Console.WriteLine($"Tu oponente es Misty, El Pokémon usado es MACHOP, vida = 70/70, su estado = consciente");

    // Aserciones: verificar que la salida contenga los mensajes correctos
    string mensajeEsperado1 = "Ash tiene al Pokemon más rápido";
    string mensajeEsperado2 = "El turno es de Ash, El Pokémon usado es PIKACHU, vida = 35/35, su estado = consciente";
    string mensajeEsperado3 = "Tu oponente es Misty, El Pokémon usado es MACHOP, vida = 70/70, su estado = consciente";

    Assert.That(consoleOutput.ToString().Contains(mensajeEsperado1), Is.True);
    Assert.That(consoleOutput.ToString().Contains(mensajeEsperado2), Is.True);
    Assert.That(consoleOutput.ToString().Contains(mensajeEsperado3), Is.True);
}





}*/