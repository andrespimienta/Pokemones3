using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using NUnit.Framework;

namespace TestLibrary;

[TestFixture]
public class TestVisitorPorTurno
{
    [Test]
    
    public void TestVisitarEntrenador()
    {
        Entrenador unEntrenador = new Entrenador("Misty gonzalez", 12345678901234567890, null);
        Pokemon pokeFuego = new Pokemon("charmander", "FUEGO", 100, 2,null, "id");
        Pokemon pokeElectrico = new Pokemon("pikachu", "ELÉCTRICO", 100, 2,null, "id1");
        Pokemon pokeVolador = new Pokemon("pichoto", "VOLADOR", 100, 2,null, "id2");
        Pokemon pokeRoca = new Pokemon("rocudo", "ROCA", 100, 2,null, "id3");
        unEntrenador.AñadirASeleccion(pokeElectrico);
        unEntrenador.AñadirASeleccion(pokeFuego);
        unEntrenador.AñadirASeleccion(pokeVolador);
        unEntrenador.AñadirASeleccion(pokeRoca);
        AtaqueEspecial envenenador = new AtaqueEspecial("envenenador", "PLANTA", 10, 100, ("ENVENENAR"));
        AtaqueEspecial paralizador = new AtaqueEspecial("paralizador", "ELÉCTRICO", 10, 100, ("PARALIZAR"));
        AtaqueEspecial quemador = new AtaqueEspecial("quemador", "FUEGO", 10, 100, ("QUEMAR"));
        AtaqueEspecial adormecedor = new AtaqueEspecial("adormecedor", "PLANTA", 10, 100, ("DORMIR"));
        pokeElectrico.RecibirDaño(paralizador);
        Assert.That(pokeElectrico.EfectoActivo, Is.EqualTo(null));
        pokeElectrico.RecibirDaño(adormecedor);
        Assert.That(pokeElectrico.EfectoActivo, Is.EqualTo("DORMIDO"));
        pokeFuego.RecibirDaño(envenenador);
        Assert.That(pokeFuego.EfectoActivo, Is.EqualTo("ENVENENADO"));
        pokeRoca.RecibirDaño(paralizador);
        Assert.That(pokeRoca.EfectoActivo, Is.EqualTo("PARALIZADO"));
        pokeVolador.RecibirDaño(quemador);
        pokeVolador.RecibirDaño(paralizador); //deberia de estas solo quemado y no hacer efecto el paralizar
        Assert.That(pokeVolador.EfectoActivo, Is.EqualTo("QUEMADO"));
        unEntrenador.AceptarVisitorPorTurno(VisitorPorTurno.GetInstancia());
        Assert.That(unEntrenador.TurnosRecargaAtkEspecial, Is.EqualTo(1));
        Assert.That(pokeElectrico.TurnosDuracionEfecto, Is.EqualTo(2));
        
    }
}