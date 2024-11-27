namespace Proyecto_Pokemones_I;

public abstract class Item
{
    public string Nombre { get; }

    public Item()
    {
    }
    /// <summary>
    /// Las clases que heredan de Item se comportan
    /// como visitor para aplicar su comportamiento.
    /// El pokemon llama al método AceptarItem y se
    /// pasa a sí mismo por parámetro, para que el
    /// ítem ejecute su comportamiento.
    /// </summary>
    /// <returns></returns>
    public abstract string DescribirItem();
    public abstract void ActivarItem(Pokemon pokemon);
}