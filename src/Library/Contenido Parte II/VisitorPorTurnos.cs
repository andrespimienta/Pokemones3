using Library.Bot.Dominio;

namespace Library.Contenido_Parte_II;

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
    public async Task VisitarEntrenador(Entrenador entrenadorVisitado)
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
                            await Fachada.Instance.EnviarACanal(CanalConsola.Instance, $"El Pokemon **{pokemon.GetNombre()}** se ha despertado\n");
                            await Fachada.Instance.EnviarAUsuario(entrenadorVisitado.GetSocketGuildUser(), $"El Pokemon **{pokemon.GetNombre()}** se ha despertado\n");
                        }
                        else
                        {
                            pokemon.PuedeAtacar = false;
                            await Fachada.Instance.EnviarACanal(CanalConsola.Instance, $"El Pokemon **{pokemon.GetNombre()}** seguirá  💤  dormido {pokemon.TurnosDuracionEfecto} turnos más\n");
                            await Fachada.Instance.EnviarAUsuario(entrenadorVisitado.GetSocketGuildUser(), $"El Pokemon **{pokemon.GetNombre()}** seguirá  💤  dormido {pokemon.TurnosDuracionEfecto} turnos más\n");
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
                            await Fachada.Instance.EnviarACanal(CanalConsola.Instance,$"El Pokemon **{pokemon.GetNombre()}**, a pesar de estar  ✨  paralizado, podrá atacar en este turno\n");
                            await Fachada.Instance.EnviarAUsuario(entrenadorVisitado.GetSocketGuildUser(), $"El Pokemon **{pokemon.GetNombre()}**, a pesar de estar  ✨  paralizado, podrá atacar en este turno\n");
                        }
                        else
                        {
                            Fachada.Instance.EnviarACanal(CanalConsola.Instance,$"El Pokemon **{pokemon.GetNombre()}** está  ✨  paralizado, no podrá atacar en este turno\n");
                            Fachada.Instance.EnviarAUsuario(entrenadorVisitado.GetSocketGuildUser(),$"El Pokemon **{pokemon.GetNombre()}** está  ✨  paralizado, no podrá atacar en este turno\n");
                        }
                        break;
                    }
                    case "QUEMADO":
                    {
                        // Si está quemado, le resta el 10% de la vida en cada turno
                        pokemon.AlterarVida(-pokemon.GetVidaMax() * 0.1);
                        Fachada.Instance.EnviarACanal(CanalConsola.Instance,$"El Pokemon **{pokemon.GetNombre()}** perdió  ❤️ {pokemon.GetVidaMax() * 0.1} por  ♨️  quemadura\n");
                        Fachada.Instance.EnviarAUsuario(entrenadorVisitado.GetSocketGuildUser(),$"El Pokemon **{pokemon.GetNombre()}** perdió  ❤️ {pokemon.GetVidaMax() * 0.1} por  ♨️  quemadura\n");
                        break;
                    }
                    case "ENVENENADO":
                    {
                        // Si está envenenado, le resta el 5% de la vida en cada turno
                        pokemon.AlterarVida(-pokemon.GetVidaMax() * 0.05);
                        Fachada.Instance.EnviarACanal(CanalConsola.Instance,$"El Pokemon **{pokemon.GetNombre()}** perdió  ❤️ {pokemon.GetVidaMax() * 0.05} por  🫧  envenenamiento\n");
                        Fachada.Instance.EnviarAUsuario(entrenadorVisitado.GetSocketGuildUser(), $"El Pokemon **{pokemon.GetNombre()}** perdió  ❤️ {pokemon.GetVidaMax() * 0.05} por  🫧  envenenamiento\n");
                        break;
                    }
                }
            }
            
        }
    }
}