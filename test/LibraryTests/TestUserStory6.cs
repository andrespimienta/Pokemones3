using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;


namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory6
{
    [Test]
    public async Task CheckearFinalDePantalla()
    {
        // Arrange: Configuración inicial de los entrenadores y la batalla
        // Creamos dos entrenadores, uno para el usuario y otro para el oponente
        Entrenador usuario = new Entrenador("Jugador", 3, null); // Entrenador con 3 Pokémons (no inicializados)
        Entrenador oponente = new Entrenador("Oponente", 1, null); // Entrenador con 1 Pokémon (no inicializado)
        
        // Configuramos la batalla entre el usuario y el oponente
        Battle batalla = new Battle(usuario, oponente);
        batalla.EntrenadorConTurno = usuario; // El turno inicial es del usuario

        // Simulamos que todos los Pokémons del oponente tienen 0 de vida
        oponente.GetSeleccion().ForEach(pokemon => pokemon.vida = 0); // Reducimos la vida de los Pokémons del oponente a 0
        
        // Comprobamos si el oponente tiene Pokémons vivos
        bool result = oponente.GetPokemonesVivos() == 0;

        // Assert: Verificamos que la condición de victoria se cumpla
        Assert.That(result, Is.True, "Se esperaba que el método devolviera true.");

        // Verificamos que el mensaje de fin de batalla se muestre correctamente (si está implementado)
        string mensaje = $"{usuario.GetNombre()} ha ganado la batalla."; // Método que se espera que devuelva el mensaje del ganador
        Assert.That(mensaje, Is.EqualTo($"{usuario.GetNombre()} ha ganado la batalla."), "El mensaje de ganador no es el esperado.");
        Fachada.Reset();
       
    }
}