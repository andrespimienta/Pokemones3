using System.Diagnostics;
using Library.Bot.Dominio;
using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory7
{
    [Test]
    public void CambiarPokemon()
    {
        // Inicialización de la fachada y los entrenadores
        Fachada fachada = Fachada.Instance; ///"Obtiene la instancia de la fachada para gestionar las operaciones de la batalla"
        Entrenador usuario = new Entrenador("Jugador", 3, null); ///"Crea el entrenador jugador con ID 3"
        Entrenador oponente = new Entrenador("Oponente", 1, null); ///"Crea el entrenador oponente con ID 1"
        
        // Creación de Pokémon y asignación al jugador
        Pokemon p1 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, null, "id1"); ///"Crea a Pikachu con tipo ELÉCTRICO y vida inicial de 35"
        Pokemon p2 = new Pokemon("CHARMANDER", "FUEGO", 35, 1.5, null, "id2"); ///"Crea a Charmander con tipo FUEGO y vida inicial de 35"
        usuario.AñadirASeleccion(p1); ///"Agrega a Pikachu al equipo del jugador"
        usuario.AñadirASeleccion(p2); ///"Agrega a Charmander al equipo del jugador"
        usuario.UsarPokemon(p1); ///"Selecciona a Pikachu como el Pokémon inicial en uso"

        // Configuración de la batalla
        BattlesList.Instance.AddBattle(usuario, oponente); ///"Registra la batalla entre el jugador y el oponente en la lista de batallas activas"
        Battle batalla = BattlesList.Instance.GetBattle(3); ///"Recupera la batalla específica en la que participa el jugador con ID 3"
        batalla.EntrenadorConTurno = usuario; ///"Define que el turno inicial es del jugador"

        // Cambio de Pokémon durante el turno
        fachada.CambiarPokemon(usuario.Id, "CHARMANDER"); ///"El jugador usa la fachada para cambiar su Pokémon activo a Charmander"

        // Verificaciones
        Assert.That(usuario.GetPokemonEnUso().GetNombre(), Is.EqualTo("CHARMANDER")); ///"Verifica que el Pokémon en uso del jugador se haya cambiado correctamente a Charmander"
        Assert.That(batalla.GetEntrenadorConTurno().GetNombre(), Is.EqualTo("Oponente")); ///"Verifica que al cambiar de Pokémon, el turno haya pasado automáticamente al oponente"
    }
}
