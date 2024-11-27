using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;


namespace TestLibrary;

public class TestUserStory2
{
    [Test]
    // "2. Como jugador, quiero ver los ataques disponibles de mis Pokémons para poder elegir cuál usar en cada turno."
    public void VerAtaquesDePokemones()
    {
        Fachada fachada1 = Fachada.Instance;
        Pokemon p1 = new Pokemon("PIKACHU", "ELÉCTRICO", 35, 1.5, null, "id1");
        List<Ataque> ataquesPikachu = new List<Ataque>();
        Ataque ataque1 = new AtaqueBasico("IMPACTRUENO","ELÉCTRICO",40,77);
        Ataque ataque2 = new AtaqueBasico("RAYO","ELÉCTRICO",90,90);
        Ataque ataque3 = new AtaqueBasico("PUÑO TRUENO","LUCHA",75,90);
        Ataque ataque4 = new AtaqueEspecial("ATAQUE RÁPIDO","NORMAL",40,90, "PARALIZAR");
        ataquesPikachu.Add(ataque1);
        ataquesPikachu.Add(ataque2);
        ataquesPikachu.Add(ataque3);
        ataquesPikachu.Add(ataque4);
        Entrenador j1 = new Entrenador("a", 3, null);
        j1.UsarPokemon(p1);
        //List <Ataque>ataques  = p1.GetAtaques();
        CanalConsola consola = CanalConsola.Instance;
        var consoleOutput = new StringWriter();
        Console.SetOut(consoleOutput);
        foreach (Ataque ataque in ataquesPikachu)
        {
            fachada1.EnviarACanal(consola, ataque.GetNombre());
        }
        string salidaEsperada = "IMPACTRUENO\r\nRAYO\r\nPUÑO TRUENO\r\nATAQUE RÁPIDO\r\n"; // \n representa cada línea
        Assert.That(consoleOutput.ToString(), Is.EqualTo(salidaEsperada));
        Fachada.Reset();
        
    }
}