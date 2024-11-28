using System.Text;
using Library.Bot.Dominio;
using Library.Contenido_Parte_II;
using NUnit.Framework;

namespace TestLibrary;

public class TestCalculoDeProbabilidad
{
    
    //AMBOS JUGADORES TIENEN LA MISMA CANTIDAD DE POKEMONES Y DE ITEMS
    [Test]
    public void CalculoDeProbabilidadMismaCondicion()
    {
        // Instancia del canal de consola para manejar mensajes al usuario (Singleton)
        CanalConsola consola = CanalConsola.Instance;

        // Reinicia la fachada para asegurar un estado limpio
        Fachada.Reset();

        // Agrega entrenadores a la lista de espera utilizando la fachada
        Fachada.Instance.AddTrainerToWaitingList(1, "pepe", null, consola);
        Fachada.Instance.AddTrainerToWaitingList(2, "pepito", null, consola);

        // Crea una batalla entre los entrenadores agregados
        Fachada.Instance.ChallengeTrainerToBattle("pepe", "pepito", consola);

        // Agrega 6 Pokémon a cada entrenador
        for (ulong entrenadorId = 1; entrenadorId <= 2; entrenadorId++)
        {
            for (int i = 1; i <= 6; i++)
            {
                string identificador = i.ToString();
                Fachada.Instance.AddPokemonToList(entrenadorId, identificador);
            }
        }

        // Selecciona el Pokémon en uso para cada entrenador
        Fachada.Instance.SelectPokemonInUse(1, "1"); // Entrenador 1 usa Pokémon con ID "1"
        Fachada.Instance.SelectPokemonInUse(2, "2"); // Entrenador 2 usa Pokémon con ID "2"

        // Configuración inicial
        // Para capturar la salida que normalmente iría a la consola
        var consoleOutput = new StringWriter(); 
        Console.SetOut(consoleOutput); // Redirigimos la salida estándar a nuestro StringWriter
        
        // Inicia la batalla
        Fachada.Instance.StartBattle(1);
        Fachada.Instance.StartBattle(2);

        string salidaEsperada = "Ambos jugadores tienen la misma probabilidad de ganar el combate\r\n"; // Misma cantidad de pokemones
        
        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada), "El mensaje de salida no coincide con el esperado.");
        
    }

    [Test]
    public void CalculoDeProbabilidadSituacionFavorableAUsuario()
    {
        // Instancia del canal de consola para manejar mensajes al usuario (Singleton)
        CanalConsola consola = CanalConsola.Instance;

        // Reinicia la fachada para asegurar un estado limpio
        Fachada.Reset();

        // Agrega entrenadores a la lista de espera utilizando la fachada
        Fachada.Instance.AddTrainerToWaitingList(1, "pepe", null, consola);
        Fachada.Instance.AddTrainerToWaitingList(2, "pepito", null, consola);

        // Crea una batalla entre los entrenadores agregados
        Fachada.Instance.ChallengeTrainerToBattle("pepe", "pepito", consola);

        // Agrega 6 Pokémon a cada entrenador
        for (ulong entrenadorId = 1; entrenadorId <= 2; entrenadorId++)
        {
            for (int i = 1; i <= 6; i++)
            {
                string identificador = i.ToString();
                Fachada.Instance.AddPokemonToList(entrenadorId, identificador);
            }
        }

        // Selecciona el Pokémon en uso para cada entrenador
        Fachada.Instance.SelectPokemonInUse(1, "1"); // Entrenador 1 usa Pokémon con ID "1"
        Fachada.Instance.SelectPokemonInUse(2, "2"); // Entrenador 2 usa Pokémon con ID "2"
        
        Fachada.Instance.StartBattle(1);
        Fachada.Instance.StartBattle(2);
        
        // Configuración inicial
        // Para capturar la salida que normalmente iría a la consola
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput); // Redirigimos la salida estándar a nuestro StringWriter
        // Inicia la batalla
        
        
        Battle batalla=BattlesList.Instance.GetBattle(1);
        batalla.EntrenadorConTurno = batalla.GetEntrenadorActual(1);

    //menos catidad de posiones por parte del usuario
        Fachada.Instance.UsarPocion(1, "1 Pikachu");


      
        Entrenador oponente = batalla.GetEntrenadorOponente(1);
        
        string salidaEsperada = $"El jugador {oponente.GetNombre()} tiene mas probabilidad de ganar el combate\n\r\n"; // Caso favorable a oponente
        
        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada), "El mensaje de salida no coincide con el esperado.");
    }
}