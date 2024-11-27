using Library.Bot.Dominio;
using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using NUnit.Framework;
//using Proyecto_Pokemones_I;
//using Ucu.Poo.DiscordBot.Domain;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory3y4y5
{
    [Test]
    public void CheckearVida()
    {
        // Creación de los entrenadores para la batalla
        Entrenador user = new Entrenador("user", 1, null); ///"Crea al jugador principal con ID 1"
        Entrenador oponente = new Entrenador("oponente", 2, null); ///"Crea al oponente con ID 2"
        
        // Creación de los Pokémon que usará cada entrenador
        Pokemon p1 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, null, "id1"); ///"Pikachu con 35 de vida inicial y tipo ELÉCTRICO"
        Pokemon p2 = new Pokemon("POKE", "FUEGO", 35, 1.5, null, "id2"); ///"Otro Pokémon llamado POKE con tipo FUEGO"

        // Creación de un ataque para Pikachu
        Ataque ataque1 = new AtaqueBasico("IMPACTRUENO", "ELÉCTRICO", 40, 77); ///"Ataque IMPACTRUENO de tipo ELÉCTRICO con daño base 40"

        // Configuración inicial: cada entrenador selecciona su Pokémon y lo usa
        user.AñadirASeleccion(p1); ///"Añade Pikachu a la selección del jugador"
        oponente.AñadirASeleccion(p2); ///"Añade POKE a la selección del oponente"
        user.UsarPokemon(p1); ///"El jugador elige usar a Pikachu"
        oponente.UsarPokemon(p2); ///"El oponente elige usar a POKE"

        // Inicialización de la fachada y la batalla
        Fachada fachada = Fachada.Instance; ///"Obtiene la instancia de la fachada para manejar la interacción con el canal"
        Fachada.Reset(); ///"Resetea el estado de la fachada para que no conserve datos previos"
        Battle batalla = new Battle(user, oponente); ///"Inicializa una batalla entre el jugador y el oponente"
        batalla.EntrenadorConTurno = user; ///"Establece que el jugador tiene el primer turno"

        // Configuración para capturar la salida en consola
        CanalConsola consola = CanalConsola.Instance; ///"Obtiene la instancia del canal de consola"
        var consoleOutput = new StringWriter(); ///"StringWriter para capturar lo que se imprime en la consola"
        Console.SetOut(consoleOutput); ///"Redirige la salida estándar a `consoleOutput`"

        // Primera actualización de vidas y turno
        double vidaJugador = user.GetPokemonEnUso().GetVida(); ///"Obtiene la vida actual del Pokémon del jugador"
        double vidaOponente = oponente.GetPokemonEnUso().GetVida(); ///"Obtiene la vida actual del Pokémon del oponente"
        string mensaje = $"{vidaJugador}/{vidaOponente}\n" + ///"Construye el mensaje de vidas en formato numérico"
                         $"Es el turno de {batalla.EntrenadorConTurno.GetNombre()}\n"; ///"Indica de quién es el turno"
        fachada.EnviarACanal(consola, mensaje); ///"Envía el mensaje al canal de consola"

        // El Pokémon del oponente recibe daño
        oponente.GetPokemonEnUso().RecibirDaño(ataque1); ///"Aplica el daño del ataque IMPACTRUENO al Pokémon del oponente"
        batalla.EntrenadorConTurno = oponente; ///"Cambia el turno al oponente"

        // Segunda actualización de vidas y turno
        vidaJugador = user.GetPokemonEnUso().GetVida(); ///"Actualiza la vida del Pokémon del jugador (aunque no ha cambiado)"
        vidaOponente = oponente.GetPokemonEnUso().GetVida(); ///"Actualiza la vida del Pokémon del oponente después del ataque"
        mensaje = $"{vidaJugador}/{vidaOponente}\n" + ///"Construye el nuevo mensaje de vidas"
                  $"Ahora es el turno de {batalla.EntrenadorConTurno.GetNombre()}\n"; ///"Indica que ahora es el turno del oponente"
        fachada.EnviarACanal(consola, mensaje); ///"Envía el nuevo mensaje al canal de consola"

        // Definición de las posibles salidas esperadas
        string salidaEsperada1 = "35/35\nEs el turno de user\n\r\n35/-5\nAhora es el turno de oponente\n\r\n"; ///"Primera salida esperada si el daño reduce la vida del oponente a negativo"
        string salidaEsperada2 = "35/35\nEs el turno de user\n\r\n35/35\nAhora es el turno de oponente\n\r\n"; ///"Segunda salida esperada si el daño no afecta al oponente"
        
        // Comparación de la salida en consola con las posibles salidas esperadas
        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada1).Or.EqualTo(salidaEsperada2)); ///"Verifica que la salida en consola sea una de las dos esperadas"
        Fachada.Reset();
    }
}
