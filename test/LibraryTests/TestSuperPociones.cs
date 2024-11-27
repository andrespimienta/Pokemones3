using NUnit.Framework;
using Proyecto_Pokemones_I;
using Proyecto_Pokemones_I.Items;

namespace TestLibrary;

[TestFixture]
public class TestSuperPociones
{
    
    
    [Test]
    
    public void TestActivarItem()
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 100, 10978.5,null, "id");
        SuperPociones superPocion = new SuperPociones();
        superPocion.ActivarItem(unPokemon);
        Assert.That((unPokemon.GetVida() ), Is.EqualTo(170));
        
    }
    
    
}