using System.Text;
using Discord.WebSocket;
using Library.Bot.Dominio;
using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using NUnit.Framework;

namespace TestLibrary;

// ESTOS SON LOS TESTS DE LAS CLASES Y MÉTODOS
// AÑADIDOS EN LA DEFENSA (ESTEBAN DURÁN)
[TestFixture]
public class TestProbabilityUtils
{
    
    [Test]
    [TestCase("Ash Ketchup", 12345678901234567890, null)]
    [TestCase("Misty gonzalez", 12345678901234567890, null)]
    
    public void TestCalcularChanceToWin(string nombre, ulong id, SocketGuildUser guild)
    {
        int chancesDeGanar;
        // Crea un entrenador y le asigna 6 Pokemons
        Entrenador unEntrenador = new Entrenador(nombre, id, guild);
        
        Pokemon pokeFuego = new Pokemon("charmander", "FUEGO", 100, 2,null, "id");
        Pokemon pokeElectrico = new Pokemon("pikachu", "ELÉCTRICO", 100, 2,null, "id1");
        Pokemon pokeVolador = new Pokemon("pichanga", "VOLADOR", 100, 2,null, "id2");
        Pokemon pokeRoca = new Pokemon("rocudo", "ROCA", 100, 2,null, "id3");
        Pokemon pokePlanta = new Pokemon("peashooter", "PLANTA", 100, 2,null, "id3");
        Pokemon pokeAgua = new Pokemon("splashy", "AGUA", 100, 2,null, "id3");
        unEntrenador.AñadirASeleccion(pokeElectrico);
        unEntrenador.AñadirASeleccion(pokeFuego);
        unEntrenador.AñadirASeleccion(pokeVolador);
        unEntrenador.AñadirASeleccion(pokeRoca);
        unEntrenador.AñadirASeleccion(pokePlanta);
        unEntrenador.AñadirASeleccion(pokeAgua);
        
        // CHEQUEA CHANCES DE GANAR INICIALES
        chancesDeGanar = ChanceToWin.CalcularChanceToWin(unEntrenador);
        Assert.That(chancesDeGanar, Is.EqualTo(98));
        
        // Crea un par de ataques para herir a los Pokemons y que cambien las chances de ganar
        AtaqueEspecial paralizador = new AtaqueEspecial("paralizador", "ELÉCTRICO", 0, 100, ("PARALIZAR"));
        
        pokeRoca.RecibirDaño(paralizador); // -10 puntos
        pokeAgua.AlterarVida(-25);  // -2 puntos
        pokePlanta.AlterarVida(-50);    // -4 puntos
        
        // CHEQUEA QUE RESTE CHANCES POR EFECTO Y CANTIDAD DE VIDA
        chancesDeGanar = ChanceToWin.CalcularChanceToWin(unEntrenador);
        Assert.That(chancesDeGanar, Is.EqualTo(82));
        
        pokeAgua.AceptarItem(unEntrenador.RemoverItem("Súper Poción")); 
        // +2 puntos por curar, -4 puntos por item faltante
        
        pokeRoca.AceptarItem(unEntrenador.RemoverItem("Cura Total"));
        // +10 puntos por Pokemons sin Efectos Negativos, -4 puntos por item faltante
        
        pokeVolador.AceptarItem(unEntrenador.RemoverItem("Súper Poción")); 
        // +0 puntos por curar (ya tenía la Vida Máx.), -4 puntos por item faltante
        
        unEntrenador.AgregarAListaMuertos(pokePlanta);
        // - 6 puntos (el Pokemon ya no está vivo, no suma los 6 puntos que antes aportaba)
        
        // CHEQUEA CHANCES POR USAR ITEMS Y PERDER POKEMONES
        chancesDeGanar = ChanceToWin.CalcularChanceToWin(unEntrenador);
        Assert.That(chancesDeGanar, Is.EqualTo(76));
    }

    [Test]
    
    public void TestShowChanceToWin()
    {
        // Crea 2 entrenadores con 3 Pokemons cada uno
        Entrenador entrenadorActual = new Entrenador("Ash", 123456789, null);
        Entrenador entrenadorOponente = new Entrenador("Misty", 987654321, null);
        
        Pokemon pokeFuego = new Pokemon("charmander", "FUEGO", 100, 2,null, "id");
        Pokemon pokeElectrico = new Pokemon("pikachu", "ELÉCTRICO", 100, 2,null, "id1");
        Pokemon pokeVolador = new Pokemon("pichanga", "VOLADOR", 100, 2,null, "id2");
        entrenadorActual.AñadirASeleccion(pokeElectrico);
        entrenadorActual.AñadirASeleccion(pokeFuego);
        entrenadorActual.AñadirASeleccion(pokeVolador);
        
        Pokemon pokeRoca = new Pokemon("rocudo", "ROCA", 100, 2,null, "id3");
        Pokemon pokePlanta = new Pokemon("peashooter", "PLANTA", 100, 2,null, "id3");
        Pokemon pokeAgua = new Pokemon("splashy", "AGUA", 100, 2,null, "id3");
        entrenadorOponente.AñadirASeleccion(pokeRoca);
        entrenadorOponente.AñadirASeleccion(pokePlanta);
        entrenadorOponente.AñadirASeleccion(pokeAgua);
        
        // Captura la salida que normalmente iría a la consola y la redirige a consoleOutput
        var consoleOutput = new StringWriter(); 
        Console.SetOut(consoleOutput);
        Fachada.Instance.ShowChanceToWin(entrenadorActual, entrenadorOponente);
        string mensajeRecibido = consoleOutput.ToString();
        string mensajeEsperado = "**¡Ambos tienen exactamente la misma probabilidad de ganar!**";
        
        // CHEQUEA MENSAJE EN CONDICIONES INICIALES
        Assert.That(mensajeRecibido, Does.Contain(mensajeEsperado));
        
        entrenadorOponente.AgregarAListaMuertos(pokeRoca); // -10 puntos (Oponente)
        pokeAgua.AlterarVida(-25); // -2 puntos (Oponente)
        entrenadorActual.RemoverItem("Cura Total"); // // -4 puntos (Actual)
        
        // Captura la salida que normalmente iría a la consola y la redirige a consoleOutput
        Console.SetOut(consoleOutput);
        Fachada.Instance.ShowChanceToWin(entrenadorActual, entrenadorOponente);
        mensajeRecibido = consoleOutput.ToString();
        mensajeEsperado = "**¡Ash es el entrenador con más chances de ganar, por una diferencia de un 8% !**";
        
        // CHEQUEA MENSAJE CUANDO VA PERDIENDO EL OPONENTE
        Assert.That(mensajeRecibido, Does.Contain(mensajeEsperado));

        entrenadorActual.RemoverItem("Súper Poción"); // -4 puntos (Actual)
        entrenadorActual.RemoverItem("Revivir"); // -4 puntos (Actual)
        pokeElectrico.AlterarVida(-25); // -2 puntos (Actual)
        
        // Captura la salida que normalmente iría a la consola y la redirige a consoleOutput
        Console.SetOut(consoleOutput);
        Fachada.Instance.ShowChanceToWin(entrenadorActual, entrenadorOponente);
        mensajeRecibido = consoleOutput.ToString();
        mensajeEsperado = "**¡Misty es el entrenador con más chances de ganar, por una diferencia de un 2% !**";
        
        // CHEQUEA MENSAJE CUANDO VA PERDIENDO EL ENTRENADOR ACTUAL
        Assert.That(mensajeRecibido, Does.Contain(mensajeEsperado));
    }
}