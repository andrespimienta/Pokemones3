namespace Library.Contenido_Parte_II.Items;

public class CuraTotal : Item
{
    public CuraTotal()
    {
        this.Nombre = "Cura Total";
    }
    
    /// <summary>
    /// Devuelve una breve descripción del
    /// comportamiento de la poción.
    /// </summary>
    /// <returns></returns>
    public override string DescribirItem()
    {
        string mensaje = "_[Remueve los efectos negativos  **(** 💤, ✨, 🫧, ♨️ **)**  " +
                         "del pokemon que reciba esta poción]_";
        return mensaje;
    }

    /// <summary>
    /// Solo si el pokemon está vivo, le remueve los efectos
    /// y lo habilita para atacar. La restricción es para que
    /// no se pueda hacer que un pokemon muerto pueda atacar.
    /// </summary>
    /// <param name="pokemon"></param>
    public override void ActivarItem(Pokemon pokemon)
    {
        if (pokemon.GetVida() > 0)
        {
            pokemon.EfectoActivo = null;
            pokemon.PuedeAtacar = true;
        }
    }
}