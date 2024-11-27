using Library.Contenido_Parte_II;
using NUnit.Framework;
using Proyecto_Pokemones_I;
using Ucu.Poo.DiscordBot.Domain;

namespace TestLibrary;

using System;
using System.IO;
using NUnit.Framework;

public class TestUserStory6
{
    [Test]

    public void CheckearFinalDePantalla()
    {
        Entrenador j1 = new Entrenador("a", 3, null);
        Entrenador j2 = new Entrenador("b", 1, null);
        Battle batalla = new Battle(j1, j2);
    }
}