using Discord.WebSocket;
using Moq;
using NUnit.Framework;
using Proyecto_Pokemones_I;

namespace TestLibrary;
z
[TestFixture]
public class TestEntrenador
{
    [Test]
    [TestCase("Ash Ketchum", 12345678901234567890)]
    [TestCase("Misty Gonzalez", 12345678901299999999)]
    public void ConstructorConParametrosValidos(string nombre, ulong id)
    {
        // Crear un mock de SocketGuildUser
        var mockGuildUser = new Mock<SocketGuildUser>();

        // Configurar las propiedades simuladas que puedan ser necesarias
        mockGuildUser.Setup(user => user.Username).Returns(nombre);
        mockGuildUser.Setup(user => user.Id).Returns(id);

        // Obtener el objeto simulado
        var guild = mockGuildUser.Object;

        // Crear una instancia de Entrenador
        Entrenador unEntrenador = new Entrenador(nombre, id, guild);

        // Verificar que las propiedades se configuraron correctamente
        Assert.That(unEntrenador.GetNombre(), Is.EqualTo(nombre));
        Assert.That(unEntrenador.Id, Is.EqualTo(id));
        Assert.That(unEntrenador.GetSocketGuildUser(), Is.EqualTo(guild));
    }
}