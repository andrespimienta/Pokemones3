using Proyecto_Pokemones_I;

namespace Library.Contenido_Parte_II.Items;

public class SuperPocion :Item
{
    public string Nombre { get; }

    public double VidaRegenerada { get; }

    public SuperPocion()
    {
        this.Nombre = "Súper Poción";
        this.VidaRegenerada = 70;
    }
    
    /// <summary>
    /// Devuelve una breve descripción del
    /// comportamiento de la poción.
    /// </summary>
    /// <returns></returns>
    public override string DescribirItem()
    {
        string mensaje = "_Recupera ❤️ 70 del pokemon que reciba esta poción._";
        return mensaje;
    }

    /// <summary>
    /// Solo si el pokemon está vivo, le aumenta
    /// la vida 70 puntos, o hasta la vida máxima.
    /// La restricción es para que esta poción no
    /// cumpla la función de 'Revivir' al sumarle vida
    /// a un pokemon muerto.
    /// </summary>
    /// <param name="pokemon"></param>
    public override void ActivarItem(Pokemon pokemon)
    {
        if (pokemon.GetVida() > 0)
        {
            // Si al sumarle 70 se pasa de la vida máxima, le suma solo lo que le falta
            if (pokemon.GetVida() + this.VidaRegenerada > pokemon.GetVidaMax())
            {
                double vidaFaltante = pokemon.GetVidaMax() - pokemon.GetVida();
                pokemon.AlterarVida(vidaFaltante);
            }
            // Sino, le suma 70 puntos de vida de manera normal
            else
            {
                pokemon.AlterarVida(this.VidaRegenerada);
            }
        }
    }

}