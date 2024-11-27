using Discord.WebSocket;
using Library.Bot.Dominio;
using Library.Contenido_Parte_II;
using Library.Contenido_Parte_II.Ataques;
using NUnit.Framework;
//using Proyecto_Pokemones_I;
//using Proyecto_Pokemones_I.Items;

namespace TestLibrary;

public class TestDefensa27novAndresPimienta
{
    
    
    [Test]
    public void ProbabilidadGanadorTest()
    {
        //Creamos los entrenadores y la batalla
        Entrenador unEntrenador = new Entrenador("Player1", 23123131, null);
        Entrenador unEntrenador2 = new Entrenador("Player2", 23123132, null);
        Battle batalla = new Battle(unEntrenador, unEntrenador2);

        //Creamos los pokemones y los asignamos
        Pokemon unPokemon = new Pokemon("Charmander", "FUEGO", 100, 10978.5,null, "id");
        Pokemon unPokemon2 = new Pokemon("Charmander", "FUEGO", 100, 10978.5,null, "id");
        Pokemon unPokemon3 = new Pokemon("Charmander", "FUEGO", 100, 10978.5,null, "id");
        Pokemon unPokemon4 = new Pokemon("Charmander", "FUEGO", 100, 10978.5,null, "id");
        Pokemon unPokemon5 = new Pokemon("Charmander", "FUEGO", 100, 10978.5,null, "id");
        Pokemon unPokemon6 = new Pokemon("Charmander", "FUEGO", 100, 10978.5,null, "id");
        Pokemon unPokemon7 = new Pokemon("Charmander", "FUEGO", 100, 10978.5,null, "id");

        unEntrenador.AñadirASeleccion(unPokemon);
        unEntrenador.AñadirASeleccion(unPokemon2);
        unEntrenador.AñadirASeleccion(unPokemon3);
        unEntrenador.AñadirASeleccion(unPokemon4);
        unEntrenador.AñadirASeleccion(unPokemon5);
        unEntrenador.AñadirASeleccion(unPokemon6);
        
        //Como solo el player 1 tiene pokemones, deberia devolver como que tiene mayor probabilidad
        Assert.That(batalla.CalcularProbGanador(), Is.EqualTo("El jugador más probable de ganar es Player1"));

        unEntrenador2.AñadirASeleccion(unPokemon);
        unEntrenador2.AñadirASeleccion(unPokemon2);
        unEntrenador2.AñadirASeleccion(unPokemon3);
        unEntrenador2.AñadirASeleccion(unPokemon4);
        unEntrenador2.AñadirASeleccion(unPokemon5);
        unEntrenador2.AñadirASeleccion(unPokemon7);
        
        //Como ambos jugadores tienen lo mismo, deberia devolver que ambos tienen la misma probabilidad
        Assert.That(batalla.CalcularProbGanador(), Is.EqualTo("Ambos jugadores tienen la misma probabilidad de ganar"));

        unEntrenador.RecargarItems();
        unEntrenador2.RecargarItems();
        
        unEntrenador.RemoverItem("Revivir");
        //Como el jugador 1 tiene una pocion menos, deberia devovler al 2 como ganador
        Assert.That(batalla.CalcularProbGanador(), Is.EqualTo("El jugador más probable de ganar es Player2"));
        unEntrenador.RecargarItems();

        
        AtaqueEspecial ataqueEspecial = new AtaqueEspecial("Giro tornado", "Viento", 10.5, 100, "Dormir");
        unPokemon7.RecibirDaño(ataqueEspecial);
        
        //Como a un pokemon del jugador 2 tiene un ataque especial, deberia devovler al jugador 1 como ganador
        Assert.That(batalla.CalcularProbGanador(), Is.EqualTo("El jugador más probable de ganar es Player1"));
    }
}
