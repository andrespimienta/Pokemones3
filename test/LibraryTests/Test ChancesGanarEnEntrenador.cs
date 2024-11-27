using System.Numerics;
using Library.Contenido_Parte_II;
using NUnit.Framework;

namespace TestLibrary;

[TestFixture]
public class Test_ChancesGanarEnFachada
{
    public class TestChancesDeGanar
    {
        [Test]
        public void ChancesDeGanar()
        {
            Entrenador player1 = new Entrenador("player1", 1, null);
            Pokemon poke1 = new Pokemon("fueguito", "FUEGO", 100, 10, null, "id1");
            Pokemon poke2 = new Pokemon("fueguitox", "FUEGO", 100, 10, null, "id2");
            player1.AñadirASeleccion(poke1);
            int chances = player1.ChancesGanar();
            Assert.That(chances, Is.EqualTo(50)); //10ptos por poke 1 vivo, 30 por 7 items up y 10 por ningun envenenado
            player1.AñadirASeleccion(poke2); //sumo 10 ptos mas
            chances = player1.ChancesGanar();
            Assert.That(chances, Is.EqualTo(60));
            poke1.EfectoActivo = "unEfecto"; //resto 10 porque esta infectavo el poke 1
            chances = player1.ChancesGanar();
            Assert.That(chances, Is.EqualTo(50));
            poke2.EfectoActivo = "otroEfecto"; //no resto porque ya habia otro infectado
            chances = player1.ChancesGanar();
            Assert.That(chances, Is.EqualTo(50));
            player1.RemoverItem("Revivir"); // resto 5 por un item menos
            chances = player1.ChancesGanar();
            Assert.That(chances, Is.EqualTo(45));
            
        }
    }
}