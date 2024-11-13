namespace Proyecto_Pokemones_I.Items;

public class SuperPociones :IItems
{
    public double Vida { get; private set; }
   
    public string Nombre { get; } = "Super Pocion";

    public SuperPociones()
    {
        Vida = 70;
       
    }

    public void DescribirItem()
    {
        Console.WriteLine("Recupera 70 puntos de HP.");
    }

    public void ActivarItem(Pokemon pokemon)
    {
        pokemon.AlterarVida(Vida);
    }

}