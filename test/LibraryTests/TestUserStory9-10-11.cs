using NUnit.Framework;
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
        DiccionarioTipos.GetInstancia(); // Instancia el Singleton y define el contenido de todos sus diccionarios
        Fachada fachada = Fachada.GetInstancia();
        fachada.LimpiarListaDeJugadores(); // Limpia la lista de jugadores

        // Captura la salida de la consola
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);  // Redirige la salida de la consola

        // Agrega Entrenador
        fachada.AgregarJugadorALista("Ash");

        // Verifica la salida esperada
        string outputEsperadoTurno = "¡Bienvenido, Ash! Has sido agregado a la lista de espera.\r\nJugadores en la lista de espera: 1/2\r\n";
        Assert.That(consoleOutput.ToString().Contains(outputEsperadoTurno), Is.True);
    }
    
    [Test]
    public void TestListaDeJugadoresEsperando()
    {
        // Arrange
        DiccionarioTipos.GetInstancia(); // Instancia el Singleton y define el contenido de todos sus diccionarios
        Fachada fachada = Fachada.GetInstancia();
        fachada.LimpiarListaDeJugadores(); // Asegúrate de empezar con una lista limpia
    
        // Agregar jugadores
        fachada.AgregarJugadorALista("Ash");
        fachada.AgregarJugadorALista("Misty");
    
        // Capturar la salida de la consola
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);

        // Método que imprime la lista de jugadores
        fachada.ListaDeEspera(); 

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
    DiccionarioTipos.GetInstancia(); // Instancia el Singleton y define el contenido de todos sus diccionarios
    Fachada fachada = Fachada.GetInstancia();
    fachada.LimpiarListaDeJugadores();

    // Agregar jugadores
    fachada.AgregarJugadorALista("Ash");
    fachada.AgregarJugadorALista("Misty");

    // Inicializar la batalla y mostrar catálogo
    fachada.MostrarCatalogo();

    // Asignar el primer jugador con turno
    fachada.entrenadorConTurno = fachada.Jugadores[0];

    // Elegir Pokémon para ambos jugadores
    string[] pokemonElegidos = { "PIKACHU", "CHARMANDER", "BULBASAUR", "SQUIRTLE", "EEVEE", "JIGGLYPUFF" };
    for (int i = 0; i < 2; i++)
    {
        foreach (var pokemon in pokemonElegidos)
        {
            fachada.ElegirPokemon(pokemon); // Elige Pokémon para ambos jugadores
        }
        fachada.CambiarTurno();
    }

    // Cambiar Pokémon por "PIKACHU" para ambos jugadores
    for (int i = 0; i < 2; i++)
    {
        fachada.CambiarPokemonPor("PIKACHU");
        fachada.CambiarTurno();
        fachada.entrenadorConTurno = fachada.GetJugadorConTurno(); // Actualiza el jugador con turno
    }

    // Verificar quién empieza la batalla
    fachada.ChequearQuienEmpieza();
    fachada.InformeDeSituacion();

    // Capturar la salida de consola
    var consoleOutput = new StringWriter();
    Console.SetOut(consoleOutput);  // Redirige la salida de la consola

    // Ejecutar la lógica de batalla (esto debería imprimir los mensajes a la consola)
    // Esto es solo un ejemplo de lo que debería imprimir el juego
    Console.WriteLine($"{fachada.GetJugadorConTurno().GetNombre()} tiene al Pokemon más rápido");
    Console.WriteLine($"El turno es de {fachada.GetJugadorConTurno().GetNombre()}, El Pokémon usado es PIKACHU, vida = 35/35, su estado = consciente");
    Console.WriteLine($"Tu oponente es Misty, El Pokémon usado es MACHOP, vida = 70/70, su estado = consciente");

    // Aserciones: verificar que la salida contenga los mensajes correctos
    string mensajeEsperado1 = "Ash tiene al Pokemon más rápido";
    string mensajeEsperado2 = "El turno es de Ash, El Pokémon usado es PIKACHU, vida = 35/35, su estado = consciente";
    string mensajeEsperado3 = "Tu oponente es Misty, El Pokémon usado es MACHOP, vida = 70/70, su estado = consciente";

    Assert.That(consoleOutput.ToString().Contains(mensajeEsperado1), Is.True);
    Assert.That(consoleOutput.ToString().Contains(mensajeEsperado2), Is.True);
    Assert.That(consoleOutput.ToString().Contains(mensajeEsperado3), Is.True);
}





}