using Library.Bot.Dominio;
using Library.Contenido_Parte_II;


namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory8
{

    
    [Test]
    // "Como entrenador, quiero poder usar un ítem durante una batalla."
   public void JugadorUsarItem_TerminaTurno()
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

    // Inicia la batalla
    Fachada.Instance.StartBattle(1);
    Fachada.Instance.StartBattle(2);
    
    Battle batalla = BattlesList.Instance.GetBattle(1);
    batalla.EntrenadorConTurno = batalla.Player1;

  

    // Configuración inicial
    // Para capturar la salida que normalmente iría a la consola
    var consoleOutput = new StringWriter(); 
    Console.SetOut(consoleOutput); // Redirigimos la salida estándar a nuestro StringWriter
    Pokemon pokemonReceptor = batalla.GetEntrenadorActual(1).GetPokemonEnUso();
    // El entrenador 1 usa una Súper Poción en su Pokémon activo
    Fachada.Instance.UsarPocion(1, "1 Pikachu");

    // Obtiene la batalla activa para validar el estado
    
    
    Entrenador oponente = batalla.EntrenadorConTurno; // Entrenador contrario

    // Verifica los mensajes enviados a la consola
    string mensajeEsperado = $"Le diste una **Súper Poción** a **PIKACHU** y ahora tiene ❤️ {pokemonReceptor.vida}";
   

    // Usamos Assert para verificar que los mensajes salieron por consola
    Assert.That(consoleOutput.ToString(), Does.Contain(mensajeEsperado));
    
}

}