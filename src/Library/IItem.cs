namespace Proyecto_Pokemones_I;

public interface IItems
{
    public void DescribirItem();
    public string Nombre { get; }
    public void ActivarItem(Pokemon pokemon);
}