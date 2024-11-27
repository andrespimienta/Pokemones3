using Discord.WebSocket;
using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
//using Proyecto_Pokemones_I.Items;

namespace TestLibrary;

[TestFixture]
public class TestEntrenador
{

    [Test]
    [TestCase("Ash Ketchup", 12345678901234567890, null)]
    [TestCase("Misty gonzalez", 12345678901234567890, null)]
    
    public void ConstructorTest(string nombre, ulong id, SocketGuildUser guild)
    {
        Entrenador unEntrenador = new Entrenador(nombre, id, guild);

        Assert.That(unEntrenador.GetNombre(), Is.EqualTo(nombre));
        Assert.That(unEntrenador.Id, Is.EqualTo(id));
        Assert.That(unEntrenador.GetSocketGuildUser(), Is.EqualTo(guild));

    }
    
    [Test]
    [TestCase("Ash Ketchup", 12345678901234567890, null)]
    [TestCase("Misty gonzalez", 12345678901234567890, null)]

    public void RecargarItemsTest(string nombre, ulong id, SocketGuildUser guild)
    {
        Entrenador unEntrenador = new Entrenador(nombre, id, guild);
        unEntrenador.RecargarItems();
        Assert.That(unEntrenador.GetListaItems().Count(), Is.EqualTo(14));
    }
    

    
    [Test]
    [TestCase("charmander")]
    [TestCase("pikachu")]
    

    public void AñadirASeleccionTest(string namePoke)
    {
        Entrenador unEntrenador = new Entrenador("nombre", 23123131, null);
        Pokemon unPokemon = new Pokemon(namePoke, "FUEGO", 100, 10978.5,null, "id");
        unEntrenador.AñadirASeleccion(unPokemon);
        Assert.That(unEntrenador.GetListaDePokemones(), Is.EqualTo(namePoke));

    }
    
    
    [Test]
    [TestCase(7)]
    [TestCase(3)]
    [TestCase(6)]
    [TestCase(0)]
    public void TestDeListaDePokemones(int cantPokesElegir)
    {
        Entrenador unEntrenador = new Entrenador("Misty gonzalez", 12345678901234567890, null);
        int originalCantPokes = cantPokesElegir;
        while (cantPokesElegir > 0)
        {
            Pokemon unPokemon = new Pokemon($"{cantPokesElegir}", "FUEGO", 100, 10978.5,null, "id");
            unEntrenador.AñadirASeleccion(unPokemon);
            cantPokesElegir--;
        }

        List <Pokemon> seleccionFinal = unEntrenador.GetSeleccion();

        switch (originalCantPokes)
        {
            case 0:
                Assert.That(unEntrenador.GetListaDePokemones(), Is.EqualTo(""));
                break;
            case 3:
                Assert.That(unEntrenador.GetListaDePokemones(), Is.EqualTo("3 2 1"));
                break;
            case 6:
                Assert.That(unEntrenador.GetListaDePokemones(), Is.EqualTo("6 5 4 3 2 1"));
                break;
            case 7:
                Assert.That(unEntrenador.GetListaDePokemones(), Is.EqualTo("7 6 5 4 3 2"));
                break;
            
        }
        
    }
    
    [Test]

    public void AceptarVisitorPorTurnoTest()
    {
        Entrenador unEntrenador = new Entrenador("nombre", 23123131, null);
        unEntrenador.AceptarVisitorPorTurno(VisitorPorTurno.GetInstancia());
        Assert.That(unEntrenador.TurnosRecargaAtkEspecial, Is.EqualTo(1));
    }
    
}