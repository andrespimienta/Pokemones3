namespace Proyecto_Pokemones_I.Items;

public class CuraTotal:IItems
{
    public string Nombre { get; } = "Cura total";

    public CuraTotal()
    {
        
    }
    
    public void DescribirItem()
    {
        Console.WriteLine("Cura a un Pokémon de efectos de ataques especiales, dormido, paralizado, envenenado, o quemado. "); 
    }

    public void ActivarItem(Pokemon pokemon)
    {
        pokemon.EfectoActivo = null;
        pokemon.PuedeAtacar = true;
    }
    
}