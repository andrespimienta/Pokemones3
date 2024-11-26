using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;

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
    
    public void TestDañoPorTurno(double dañoEspecial)
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 10000, 10978.5,null, "id");
        unPokemon.DañoPorTurno(dañoEspecial);
        Assert.That((unPokemon.GetVida()), Is.EqualTo((10000 - dañoEspecial)));
        
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
    
    public void TestRevivir()
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 10000, 10978.5,null, "id");
        unPokemon.Revivir();
        Assert.That((unPokemon.GetVida() ), Is.EqualTo((unPokemon.GetVidaMax()/2)));
        
    }
    
    [Test]
    [TestCase("TIERRA")]
    [TestCase("VOLADOR")]
    [TestCase("NORMAL")]
    
    public void TestRecibirDaño(string tipoAtaque)
    {
        Pokemon pikachu = new Pokemon("pikachu", "ELÉCTRICO", 10000, 10978.5,null, "id");
        Ataque unAtaque = new AtaqueBasico("unAtaque", tipoAtaque, 10, 10);
        pikachu.RecibirDaño(unAtaque);
        if (tipoAtaque == "TIERRA")
        {
            Assert.That((pikachu.GetVida()), Is.EqualTo(10000.0));           
        } else if (tipoAtaque == "VOLADOR")
        {
            Assert.That((pikachu.GetVida()), Is.EqualTo(9995.0)); 
        } else if (tipoAtaque == "NORMAL")
        {
            Assert.That((pikachu.GetVida()), Is.EqualTo(9995.0));
        }
    }
}