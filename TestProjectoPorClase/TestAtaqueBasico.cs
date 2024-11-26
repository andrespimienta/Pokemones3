using Library.Contenido_Parte_II;
using NUnit.Framework;

namespace TestLibrary;

[TestFixture]
public class TestAtaqueBasico
{
    [Test]
    [TestCase("Giro tornado", "Viento", 10.5 ,10)]
    public void ConstructorConParametrosValidos(string nombreAtaque, string tipoAtaque, double dañoAtaque, int precisionAtaque)
    {
        AtaqueBasico UnAtaqueBasico = new AtaqueBasico(nombreAtaque, tipoAtaque, dañoAtaque, precisionAtaque);
        
        Assert.That(UnAtaqueBasico.GetNombre(), Is.EqualTo(nombreAtaque));
        Assert.That(UnAtaqueBasico.GetTipo(), Is.EqualTo(tipoAtaque));
        Assert.That(UnAtaqueBasico.GetDaño(), Is.EqualTo(dañoAtaque));
        Assert.That(UnAtaqueBasico.GetPrecision(), Is.EqualTo(precisionAtaque));

    }
}