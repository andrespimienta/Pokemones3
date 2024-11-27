using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using Library.Contenido_Parte_II.Items;
using NUnit.Framework;

namespace TestLibrary;

[TestFixture]
public class TestPokemon
{
    [Test]
    [TestCase("charmander", "fuego", 1, 10.5,null, "id")]
    [TestCase("pikachu", "electrico", 10000, 10978.5,null, "id")]
    
    public void ConstructorTest(string pokeNombre, string pokeTipo, double pokeVida, double pokeVelAtaque, List<Ataque> ataques,string Identificador)
    {
        Pokemon unPokemon = new Pokemon(pokeNombre, pokeTipo, pokeVida, pokeVelAtaque, ataques, Identificador);

        Assert.That(unPokemon.GetNombre(), Is.EqualTo(pokeNombre));
        Assert.That(unPokemon.GetTipo(), Is.EqualTo(pokeTipo));
        Assert.That(unPokemon.GetVida(), Is.EqualTo(pokeVida));
        Assert.That(unPokemon.GetVelocidadAtaque(), Is.EqualTo(pokeVelAtaque));
        Assert.That(unPokemon.GetAtaques(), Is.EqualTo(ataques));
        Assert.That(unPokemon.NumeroIdentificador, Is.EqualTo(Identificador));

    }
    
    [Test]
    [TestCase(50)]
    [TestCase(1500)]
    
    public void TestAlterarVida(double hp)
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 10000, 10978.5,null, "id");
        unPokemon.AlterarVida(hp);
        Assert.That((unPokemon.GetVida()), Is.EqualTo((10000 + hp)));
        
    }
    
    [Test]
    [TestCase("TIERRA")]
    [TestCase("VOLADOR")]
    [TestCase("NORMAL")]
    
    public void TestRecibirDaño(string tipoAtaque)
    {
        Pokemon pikachu = new Pokemon("pikachu", "ELÉCTRICO", 10000, 10978.5,null, "id");
        Ataque unAtaque = new AtaqueBasico("unAtaque", tipoAtaque, 10, 100);
        pikachu.RecibirDaño(unAtaque);
        if (tipoAtaque == "TIERRA")
        {
            Assert.That((pikachu.GetVida()), Is.LessThan(9980.1));           
        } else if (tipoAtaque == "VOLADOR")
        {
            Assert.That((pikachu.GetVida()), Is.LessThan(9995.1)); 
        } else if (tipoAtaque == "NORMAL")
        {
            Assert.That((pikachu.GetVida()), Is.LessThan(9990.1));
        }
    }
    
    [Test]

    public void AceptarItem()
    {
        Pokemon unPokemon = new Pokemon("nombre", "PLANTA",150, 1.5, null, "id");
        unPokemon.AlterarVida(-80);
        SuperPocion pocion = new SuperPocion();
        unPokemon.AceptarItem(pocion);
        Assert.That(unPokemon.GetVida(), Is.EqualTo(140));
    }
}