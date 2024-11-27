using NUnit.Framework;
using Proyecto_Pokemones_I;
using Proyecto_Pokemones_I.Items;

namespace TestLibrary;

[TestFixture]
public class TestItemRevivir
{
    [Test]

    public void TestDescribirItem()
    {
        Revivir pocion = new Revivir();
        string mensaje = pocion.DescribirItem();
        
        Assert.That(mensaje, Is.EqualTo("_Revive con el 50% de la vida ❤️ total al pokemon que reciba esta poción._"));
    }

    [Test]
    public void TestActivarItem()
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 100, 10978.5,null, "id");
        Revivir pocion = new Revivir();
        
        unPokemon.AlterarVida(-100);
        unPokemon.EfectoActivo = "PARALIZADO";
        unPokemon.PuedeAtacar = false;
        
        unPokemon.AceptarItem(pocion);
        Assert.That((unPokemon.GetVida() ), Is.EqualTo(unPokemon.GetVidaMax()/2));
        Assert.That(unPokemon.EfectoActivo, Is.EqualTo(null));
        Assert.That(unPokemon.PuedeAtacar, Is.EqualTo(true));
    }
}