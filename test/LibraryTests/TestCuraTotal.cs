using NUnit.Framework;
using Proyecto_Pokemones_I;
using Proyecto_Pokemones_I.Items;

namespace TestLibrary;

[TestFixture]
public class TestCuraTotal
{
    [Test]
    
    public void TestDescribirItem()
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 100, 10978.5,null, "id");
        CuraTotal curaTotal = new CuraTotal();
        curaTotal.ActivarItem(unPokemon);
        Assert.That((unPokemon.EfectoActivo), Is.EqualTo(null));
        Assert.That((unPokemon.PuedeAtacar), Is.EqualTo(true));
        
    }

}