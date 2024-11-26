namespace Proyecto_Pokemones_I;

public class VisitorPorTurno
{
    private static VisitorPorTurno instance;
    public static VisitorPorTurno GetInstancia()
    {
        if (instance == null)
        {
            instance = new VisitorPorTurno();
        }
        return instance;
    }
    private VisitorPorTurno(){}
    public void VisitarEntrenador(Entrenador entrenadorVisitado)
    {
        if (entrenadorVisitado.TurnosRecargaAtkEspecial > 0)    // Si el Entrenador tenía turnos de enfriamiento para los ataques especiales, le resta un turno
        {
            entrenadorVisitado.TurnosRecargaAtkEspecial -= 1;
        }

        foreach (Pokemon pokemon in entrenadorVisitado.GetSeleccion())  // Chequea si alguno de los pokemones del Entrenador tiene algún efecto
        {
            string statusPokemon = pokemon.EfectoActivo;
            if (statusPokemon != null)  // Solo va a hacer cosas si el pokemon tiene algún efecto activo
            {
                switch (statusPokemon)  // Lo que le va a hacer al pokemon dependerá del efecto
                {
                    case "DORMIDO":
                    {
                        // Si está dormido, reduce en 1 los turnos que falta para que despierte
                        pokemon.TurnosDuracionEfecto -= 1;
                        
                        // Si ya pasaron todos los turnos hasta que despierte, lo despierta (le quita el efecto y le permite atacar)
                        if (pokemon.TurnosDuracionEfecto == -1)
                        {
                            pokemon.EfectoActivo = null;
                            pokemon.PuedeAtacar = true;
                            Console.WriteLine($"El Pokemon {pokemon.GetNombre()} se ha despertado");
                        }
                        else
                        {
                            pokemon.PuedeAtacar = false;
                            Console.WriteLine($"El Pokemon {pokemon.GetNombre()} seguirá dormido {pokemon.TurnosDuracionEfecto} turnos más");
                        }
                        break;
                    }
                    case "PARALIZADO":
                    {
                        // Si está paralizado, hay un 33% de chance de que el pokemon paralizado pueda atacar en cada turno
                        pokemon.PuedeAtacar = false;
                        if (ProbabilityUtils.Probabilometro(33))    
                        {
                            pokemon.PuedeAtacar = true;
                            Console.WriteLine($"El Pokemon {pokemon.GetNombre()}, a pesar de estar paralizado, puede atacar");
                        }
                        else
                        {
                            Console.WriteLine($"El Pokemon {pokemon.GetNombre()}, está paralizado, no puede atacar");
                        }
                        break;
                    }
                    case "QUEMADO":
                    {
                        // Si está quemado, le resta el 10% de la vida en cada turno
                        pokemon.AlterarVida(-pokemon.GetVidaMax() * 0.1);
                        Console.WriteLine($"El Pokemon {pokemon.GetNombre()} perdió {pokemon.GetVidaMax() * 0.1} puntos de salud por quemadura");
                        break;
                    }
                    case "ENVENENADO":
                    {
                        // Si está envenenado, le resta el 5% de la vida en cada turno
                        pokemon.AlterarVida(-pokemon.GetVidaMax() * 0.05);
                        Console.WriteLine($"El Pokemon {pokemon.GetNombre()} perdió {pokemon.GetVidaMax() * 0.05} puntos de salud por envenenamiento");
                        break;
                    }
                }
            }
            
        }
    }
}