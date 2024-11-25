using Discord.WebSocket;
using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
using Proyecto_Pokemones_I.Items;

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
        Assert.That(unEntrenador.GetListaDeItems(), Is.EqualTo("Super Pocion (x4) / Revivir (x1) / Cura total (x2) /"));


    }
    

    
    /*[Test]
    [TestCase(charmander)]
    [TestCase(pikachu)]
    

    public void AñadirASeleccionTest(Pokemon pokemon)
    {
        Entrenador unEntrenador = new Entrenador("nombre", 23123131, null);
        unEntrenador.AñadirASeleccion(pokemon);
        Assert.That(unEntrenador.ListaDePokemones(), Is.EqualTo(pokemon.GetNombre()));

    }*/
    [Test]
    [TestCase("Ash Ketchup", 12345678901234567890, null)]
    [TestCase("Misty gonzalez", 12345678901234567890, null)]

    public void TestDeListaDePokemones(string nombre, ulong id, SocketGuildUser guild)
    {
        Entrenador unEntrenador = new Entrenador(nombre, id, guild);
        Assert.That(unEntrenador.GetListaDeItems(), Is.EqualTo("Super Pocion (x4) / Revivir (x1) / Cura total (x2) /"));


    }
}