namespace Proyecto_Pokemones_I.Items;

public class Revivir:IItems
{
    public string Nombre { get; } = "Revivir";

    public Revivir()
    {
       
    }

    public void DescribirItem()
    {
        Console.WriteLine("Revive a un Pokémon con el 50% de su HP total.");
    }

    public void ActivarItem(Pokemon pokemon)
    {
        pokemon.Revivir();
    }

}