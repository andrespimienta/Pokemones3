using System.IO;
using System;
using System.Collections.Generic;
using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using NUnit.Framework;

public class LeerArchivoTests
{
    private const string TestCatalogPath = "TestCatalogo.txt";

    // Configuración inicial: Crear un archivo temporal para pruebas
    public LeerArchivoTests()
    {
        if (File.Exists(TestCatalogPath))
            File.Delete(TestCatalogPath);

        string catalogContent = @"[identificador],[nombre],[tipo],[vida],[velocidadAtaque],[nombreAtk1],[tipoAtk1],[dañoAtk1],[precisionAtk1],[nombreAtk2],[tipoAtk2],[dañoAtk2],[precisionAtk2],[nombreAtk3],[tipoAtk3],[dañoAtk3],[precisionAtk3],[nombreAtk4],[tipoAtk4],[dañoAtk4],[precisionAtk4],[efectoAtk4]
1,PIKACHU,ELÉCTRICO,140,1.5,IMPACTRUENO,ELÉCTRICO,40,77,RAYO,ELÉCTRICO,90,90,PUÑO TRUENO,LUCHA,75,90,ATAQUE RÁPIDO,NORMAL,40,90,PARALIZAR
2,CHARMANDER,FUEGO,146,1.5,LLAMARADA,FUEGO,110,85,PUÑO FUEGO,LUCHA,75,90,LÁTIGO CEPA,PLANTA,45,90,SÍSMICO,LUCHA,80,90,QUEMAR";
        File.WriteAllText(TestCatalogPath, catalogContent);

        LeerArchivo.RutaCatalogo = TestCatalogPath;
    }

    // Limpieza final
    ~LeerArchivoTests()
    {
        if (File.Exists(TestCatalogPath))
            File.Delete(TestCatalogPath);
    }

    
    [Test]
    public void ObtenerCatalogoProcesado_ArchivoInexistente_DeberiaDevolverMensajeDeError()
    {
        // Arrange
        LeerArchivo.RutaCatalogo = "ArchivoNoExistente.txt";

        // Act
        string resultado = LeerArchivo.ObtenerCatalogoProcesado();

        
        // Assert
        Assert.That(resultado, Is.EqualTo("No se pudo encontrar el catálogo."));

    }

    [Test]
    public void EncontrarPokemon_DeberiaDevolverPokemonCorrecto()
    {
        // Act
        Pokemon? pikachu = LeerArchivo.EncontrarPokemon("1");

        // Assert
        Assert.That(pikachu, Is.Not.Null);
        Assert.That(pikachu.GetNombre(), Is.EqualTo("PIKACHU"));
        Assert.That(pikachu.GetTipo(), Is.EqualTo("ELÉCTRICO"));
        Assert.That(pikachu.GetVida(), Is.EqualTo(140));
        Assert.That(pikachu.GetVelocidadAtaque(), Is.EqualTo(15));

// Verificar ataques
        Assert.That(pikachu.listadoAtaques.Count, Is.EqualTo(4));
        Assert.That(pikachu.listadoAtaques[0].GetNombre(), Is.EqualTo("IMPACTRUENO"));
        Assert.That(((AtaqueEspecial)pikachu.listadoAtaques[3]).GetEfecto(), Is.EqualTo("PARALIZAR"));

    }

    [Test]
    public void EncontrarPokemon_NoExistePokemon_DeberiaDevolverNull()
    {
        // Act
        Pokemon? inexistente = LeerArchivo.EncontrarPokemon("999");

        // Assert
        Assert.That(inexistente, Is.Null);

    }
}
