using Library.Contenido_Parte_II;
using NUnit.Framework;

namespace TestLibrary;

[TestFixture]
public class TestAtaqueEspecial
{
    [Test]
    [TestCase("Giro tornado", "Viento", 10.5 ,10, "Dormir")]
    [TestCase("Patada fuego", "Fuego", 10000 ,-10, "Paralizar")]
    
    public void ConstructorConParametrosValidos(string nombreAtaque, string tipoAtaque, double dañoAtaque, int precisionAtaque, string efecto)
    {
        AtaqueEspecial unAtaqueEspecial = new AtaqueEspecial(nombreAtaque, tipoAtaque, dañoAtaque, precisionAtaque, efecto);
        
        Assert.That(unAtaqueEspecial.GetNombre(), Is.EqualTo(nombreAtaque));
        Assert.That(unAtaqueEspecial.GetTipo(), Is.EqualTo(tipoAtaque));
        Assert.That(unAtaqueEspecial.GetDaño(), Is.EqualTo(dañoAtaque));
        Assert.That(unAtaqueEspecial.GetPrecision(), Is.EqualTo(precisionAtaque));
        Assert.That(unAtaqueEspecial.GetEfecto(), Is.EqualTo(efecto));

    }
}