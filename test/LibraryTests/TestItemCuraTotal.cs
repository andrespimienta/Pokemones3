using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Items;
using NUnit.Framework;

namespace TestLibrary;

[TestFixture]
public class TestItemCuraTotal
{
    [Test]

    public void TestDescribirItem()
    {
        CuraTotal pocion = new CuraTotal();
        string mensaje = pocion.DescribirItem();
        
        Assert.That(mensaje, Is.EqualTo("_Remueve los efectos negativos (💤 DORMIDO, ✨ PARALIZADO, " +
                                        "🫧 ENVENENADO, ♨️ QUEMADO) del pokemon que reciba esta poción._"));
    }
    
    [Test]
    
    public void TestActivarItem()
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 100, 10978.5,null, "id");
        CuraTotal curaTotal = new CuraTotal();
        
        curaTotal.ActivarItem(unPokemon);
        Assert.That((unPokemon.EfectoActivo), Is.EqualTo(null));
        Assert.That((unPokemon.PuedeAtacar), Is.EqualTo(true));
        
    }

}