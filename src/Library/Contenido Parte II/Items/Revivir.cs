namespace Library.Contenido_Parte_II.Items;

public class Revivir:Item
{
    public string Nombre { get; }

    public Revivir()
    {
        this.Nombre = "Revivir";
    }
    
    /// <summary>
    /// Devuelve una breve descripción del
    /// comportamiento de la poción.
    /// </summary>
    /// <returns></returns>
    public override string DescribirItem()
    {
        string mensaje = "_Revive con el 50% de la vida ❤️ total al pokemon que reciba esta poción._";
        return mensaje;
    }

    /// <summary>
    /// Solo si está muerto, aumenta la vida del
    /// pokemon al 50% de su vida máxima,
    /// permitiéndole atacar y removiéndole los
    /// efectos negativos viejos. La restricción
    /// es para que no cumpla la función de 'Cura Total'
    /// aplicándosela a pokemones vivos.
    /// </summary>
    /// <param name="pokemon"></param>
    public override void ActivarItem(Pokemon pokemon)
    {
        // Solo si está muerto
        if (pokemon.GetVida() <= 0)
        {
            pokemon.AlterarVida(pokemon.GetVidaMax() / 2);
            pokemon.EfectoActivo = null;
            pokemon.PuedeAtacar = true;
        }
    }

}