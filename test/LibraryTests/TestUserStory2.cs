using Library.Bot.Dominio;
using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;


namespace TestLibrary;

public class TestUserStory2
{/* Historia de Usuario 2:
   Como jugador, quiero ver los ataques disponibles de mis Pokémons para poder elegir cuál usar en cada turno.
   Criterios de aceptación:
   - Se muestran los ataques disponibles para el turno actual.
*/
    public void VerAtaquesDePokemones()
    {
        // Configuración inicial
        Fachada fachada1 = Fachada.Instance; // Instancia única de la fachada
        Pokemon p1 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, null, "id1"); // Se crea un Pokémon

        // Se crean una lista de ataques que el Pokémon tendrá disponibles
        List<Ataque> ataquesPikachu = new List<Ataque>();
        Ataque ataque1 = new AtaqueBasico("IMPACTRUENO", "ELÉCTRICO", 40, 77);
        Ataque ataque2 = new AtaqueBasico("RAYO", "ELÉCTRICO", 90, 90);
        Ataque ataque3 = new AtaqueBasico("PUÑO TRUENO", "LUCHA", 75, 90);
        Ataque ataque4 = new AtaqueEspecial("ATAQUE RÁPIDO", "NORMAL", 40, 90, "PARALIZAR");

        // Los ataques se añaden al Pokémon
        ataquesPikachu.Add(ataque1);
        ataquesPikachu.Add(ataque2);
        ataquesPikachu.Add(ataque3);
        ataquesPikachu.Add(ataque4);

        // Se asigna el Pokémon al entrenador
        Entrenador j1 = new Entrenador("a", 3, null); 
        j1.UsarPokemon(p1); // El entrenador selecciona a Pikachu como su Pokémon activo

        // Capturar la salida de la consola
        CanalConsola consola = CanalConsola.Instance; // Canal para enviar información al jugador
        var consoleOutput = new StringWriter(); // Objeto para capturar la salida de la consola
        Console.SetOut(consoleOutput); // Redirigir la consola

        // Enviar los nombres de los ataques del Pokémon al canal de consola
        foreach (Ataque ataque in ataquesPikachu)
        {
            fachada1.EnviarACanal(consola, ataque.GetNombre());
        }

        // Assert: Verificar que la salida de la consola es la esperada
        string salidaEsperada = "IMPACTRUENO\r\nRAYO\r\nPUÑO TRUENO\r\nATAQUE RÁPIDO\r\n"; // Salida esperada en formato línea por línea
        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada), "Los ataques mostrados no coinciden con los esperados.");

        // Reset: Limpiar la instancia de Fachada para no afectar otros tests
        Fachada.Reset();
    }
}