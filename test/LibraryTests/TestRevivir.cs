using NUnit.Framework;
using Proyecto_Pokemones_I;
using Proyecto_Pokemones_I.Items;

namespace TestLibrary;

[TestFixture]
public class TestRevivir
{
    [Test]
    
    public void TestActivarItem()
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 100, 10978.5,null, "id");
        Revivir revivir = new Revivir();
        revivir.ActivarItem(unPokemon);
        Assert.That((unPokemon.GetVida() ), Is.EqualTo(unPokemon.GetVidaMax()/2));
        
    }
}