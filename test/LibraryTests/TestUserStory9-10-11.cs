using System.Reactive.PlatformServices;
using Library.Bot.Dominio;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;

namespace TestLibrary;

[TestFixture]
public class TestUserStory_9_10_11
{
    [Test]
    public void AgregarJugadorLista()
    {
        // Configuración inicial
        // Para capturar la salida que normalmente iría a la consola
        var consoleOutput = new StringWriter(); 
        Console.SetOut(consoleOutput); // Redirigimos la salida estándar a nuestro StringWriter

        // Instancia del canal de consola para manejar mensajes al usuario
        CanalConsola consola = CanalConsola.Instance;

        // Agregar un entrenador a la lista de espera
        // Utilizamos la fachada para agregar al jugador, desacoplando la lógica de la plataforma concreta
        Fachada.Instance.AddTrainerToWaitingList(1, "pepe", null, consola);

        // Verificar que el mensaje en la consola es el esperado
        string salidaEsperada = "pepe agregado a la lista de espera\r\n"; // El mensaje esperado, incluyendo salto de línea
        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada), "El mensaje de salida no coincide con el esperado.");
    }

    [Test]
    public void TestListaDeJugadoresEsperando()
    {
        // Instancia del canal de consola para manejar mensajes al usuario
        CanalConsola consola = CanalConsola.Instance;
        Fachada.Reset();
        Fachada.Instance.AddTrainerToWaitingList(1, "pepe", null, consola);
        // Configuración inicial
        // Para capturar la salida que normalmente iría a la consola
        var consoleOutput = new StringWriter(); 
        Console.SetOut(consoleOutput); // Redirigimos la salida estándar a nuestro StringWriter
        
      
        Fachada.Instance.GetTrainersWaiting(consola);
        
        string salidaEsperada = "pepe\r\n"; // El mensaje esperado, incluyendo salto de línea
        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada), "El mensaje de salida no coincide con el esperado.");
        
    }
    [Test]
    public void TestIniciarBatalla()
    {
       

        // Instancia del canal de consola para manejar mensajes al usuario
        CanalConsola consola = CanalConsola.Instance;
        Fachada.Reset();
        // Agregar un entrenador a la lista de espera
        // Utilizamos la fachada para agregar al jugador, desacoplando la lógica de la plataforma concreta
        Fachada.Instance.AddTrainerToWaitingList(1, "pepe", null, consola);
        Fachada.Instance.AddTrainerToWaitingList(2, "pepito", null, consola);
        
        // Configuración inicial
        // Para capturar la salida que normalmente iría a la consola
        var consoleOutput = new StringWriter(); 
        Console.SetOut(consoleOutput); // Redirigimos la salida estándar a nuestro StringWriter

        Fachada.Instance.ChallengeTrainerToBattle("pepe","pepito",consola);
        string salidaEsperada = "Comienza el enfrentamiento: **pepe** vs **pepito**.\r\n";

        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada), "El mensaje de salida no coincide con el esperado.");
    }





}