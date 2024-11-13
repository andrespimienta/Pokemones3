using Proyecto_Pokemones_I.Items;

namespace Proyecto_Pokemones_I;

public class Entrenador
{
    // Atributos:
    private string nombre;
    private Pokemon? pokemonEnUso;
    private List<Pokemon> seleccionPokemones;
    private int pokemonesVivos;
    private List<IItems> listItems;
    public int TurnosRecargaAtkEspecial { get; set; }

    // Getters:
    public string GetNombre()
    {
        return this.nombre;
    }
    public Pokemon GetPokemonEnUso()
    {
        return this.pokemonEnUso;
    }
    public List<Pokemon> GetSeleccion()
    {
        return this.seleccionPokemones;
    }
    public int GetPokemonesVivos()
    {
        pokemonesVivos = 0;
        foreach (Pokemon pokemon in seleccionPokemones)
        {
            if(pokemon.GetVida() > 0)
            {
                pokemonesVivos += 1;
            }
        }
        return pokemonesVivos;
    }
    public string GetListaDeItems()
    {
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();
    
        // Contar la cantidad de cada item en listItems
        foreach (IItems item in listItems)
        {
            if (itemCounts.ContainsKey(item.Nombre))
            {
                itemCounts[item.Nombre]++;
            }
            else
            {
                itemCounts[item.Nombre] = 1;
            }
        }

        // Crear la cadena de resultado
        string resultado = "";
        foreach (var entry in itemCounts)
        {
            resultado += $"{entry.Key} (x{entry.Value}) ";
            Console.Write($"{entry.Key} (x{entry.Value}) / ");
        }
        return resultado.Trim(); // Elimina el último espacio extra al final de la cadena
    }
  
    // Constructor:
    public Entrenador(string suNombre)
    {
        this.nombre = suNombre;
        this.pokemonEnUso = null;
        this.seleccionPokemones = new List<Pokemon>();
        this.pokemonesVivos = 6;
        this.TurnosRecargaAtkEspecial = 2;      // Decidimos que por defecto no se pueda usar el ataque especial en los primeros dos turnos
        this.listItems = new List<IItems>();
        this.RecargarItems();
    }

    // Métodos:
    public void RecargarItems()
    {
        for (int i = 0; i < 4; i++)    // Agrega 4 Super Poción
        {
            SuperPociones pocion = new SuperPociones();
            listItems.Add(pocion);
        }
        
        Revivir resusitacion = new Revivir();   // Agrega 1 Revivir
        listItems.Add(resusitacion);

        for (int i = 0; i < 2; i++)
        {
            CuraTotal curaTotal = new CuraTotal();  // Agrega 2 Cura Total
            listItems.Add(curaTotal);
        }
    }

    public void AñadirASeleccion(Pokemon elPokemon)
    {
        this.seleccionPokemones.Add(elPokemon);
    }

    public void GuardarPokemon()
    {
        this.pokemonEnUso = null;
    }

    public void UsarPokemon(Pokemon pokemonAUsar)
    {
        this.pokemonEnUso = pokemonAUsar;
    }

    public string ListaDePokemones()
    {
        string resultado = "";

        foreach (Pokemon pokemon in seleccionPokemones)
        {
            if (pokemon.GetVida() > 0)// Tengo que especificar esto para cuando sean vencidos, no lo vuelvan a listas
            {
                string nombre = pokemon.GetNombre();
                Console.Write(nombre + " "); // Imprime cada nombre seguido de un espacio
                resultado += nombre + " "; // Agrega cada nombre a la cadena `resultado` seguido de un espacio
            }
        }

        Console.WriteLine();
        return resultado.Trim(); // Elimina el último espacio extra al final de la cadena
    }

    public bool UsarItem(string nombreItem)
    {
        bool result = false;
        nombreItem = nombreItem.ToUpper();    // Evito errores por mayúsculas o minúsculas en el parámetro
        
        IItems item = listItems.FirstOrDefault(i => i.Nombre.Equals(nombreItem, StringComparison.OrdinalIgnoreCase));
    
        if (item != null)
        {   item.DescribirItem();
            //Console.WriteLine("Desea usarlo: s/n");
            //string aux = Console.ReadLine();
            string aux = "S"; //aux.ToUpper();// Evito errores por mayúsculas o minúsculas en el parámetro
            if (aux =="S")
            {
                // eliminar el ítem de la lista después de usarlo
                listItems.Remove(item);
                item.ActivarItem(this.pokemonEnUso);
                Console.WriteLine($"El ítem '{item.Nombre}' ha sido usado");
                result = true;
            }
            else result = false;
        }
        else
        {
            Console.WriteLine($"El ítem '{nombreItem}' no se encontró en la lista.");
        }

        return result;
    }

    public void AceptarVisitorPorTurno(VisitorPorTurno visitor)
    {
        visitor.VisitarEntrenador(this);
    }
}