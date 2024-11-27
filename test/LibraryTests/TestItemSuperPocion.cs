using Library.Contenido_Parte_II.Items;
using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;

[TestFixture]
public class TestSuperPocion
{
    [Test]
    
    public void TestDescribirItem()
    {
        SuperPocion pocion = new SuperPocion();
        string mensaje = pocion.DescribirItem();
        
        Assert.That(mensaje, Is.EqualTo("_Recupera ❤️ 70 del pokemon que reciba esta poción._"));
    }
    
    [Test]
    
    public void TestActivarItem()
    {
        Pokemon unPokemon = new Pokemon("pikachu", "electrico", 100, 10978.5,null, "id");
        unPokemon.AlterarVida(-80);
        SuperPocion superPocion = new SuperPocion();
        superPocion.ActivarItem(unPokemon);
        Assert.That((unPokemon.GetVida() ), Is.EqualTo(90));
        
    }
    
    
}