using Discord.WebSocket;
using Library.Contenido_Parte_II.Items;

namespace Library.Contenido_Parte_II;

public class Entrenador
{
    // Atributos:
    public ulong Id { get; set; } // ID de Discord para identificar al jugador
    private string nombre;
    private Pokemon? pokemonEnUso;
    private List<Pokemon> seleccionPokemonesVivos;
    private List<Pokemon> listaPokemonesMuertos;
    private List<Item> listaItems;
    public int TurnosRecargaAtkEspecial { get; set; }
    public bool EstaListo { get; set; } // Agregar el flag para saber si está listo
    private SocketGuildUser userds1;


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
        return this.seleccionPokemonesVivos;
    }

    public List<Pokemon> GetListaMuertos()
    {
        return this.listaPokemonesMuertos;
    }

    public int GetPokemonesVivos()
    {
        int pokemonesVivos = 0;
        foreach (Pokemon pokemon in seleccionPokemonesVivos)
        {
            if (pokemon.GetVida() > 0)
            {
                pokemonesVivos += 1;
            }
        }

        return pokemonesVivos;
    }

    public List<Item> GetListaItems()
    {
        return this.listaItems;
    }

    public string GetMensajeListaDeItems()
    {
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();

        // Contar la cantidad de cada item en listItems
        foreach (Item item in listaItems)
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
            resultado += $"{entry.Key} (x{entry.Value}) / ";
            //Console.Write($"{entry.Key} (x{entry.Value}) / ");
        }

        return resultado.Trim(); // Elimina el último espacio extra al final de la cadena
    }
    public SocketGuildUser GetSocketGuildUser()
    {
        return this.userds1;
    }


    // Constructor:
    public Entrenador(string suNombre, ulong id, SocketGuildUser guild )
    {
        this.Id = id;
        this.nombre = suNombre;
        this.pokemonEnUso = null;
        this.seleccionPokemonesVivos = new List<Pokemon>();
        this.listaPokemonesMuertos = new List<Pokemon>();
        this.TurnosRecargaAtkEspecial = 2; // Decidimos que por defecto no se pueda usar el ataque especial en los primeros dos turnos
        this.listaItems = new List<Item>();
        this.RecargarItems();
        this.userds1 = guild;
        
        this.EstaListo = false; // Inicialmente no está listo

    }

    // Métodos:
    public void RecargarItems()
    {
        // Agrega 4 Super Poción
        SuperPocion superPocion1 = new SuperPocion();
        SuperPocion superPocion2 = new SuperPocion();
        SuperPocion superPocion3 = new SuperPocion();
        SuperPocion superPocion4 = new SuperPocion();
        listaItems.Add(superPocion1);
        listaItems.Add(superPocion2);
        listaItems.Add(superPocion3);
        listaItems.Add(superPocion4);

        // Agrega 1 Revivir
        Revivir revivir = new Revivir(); 
        listaItems.Add(revivir);

        // Agrega 2 Cura Total
        CuraTotal curaTotal1 = new CuraTotal(); 
        CuraTotal curaTotal2 = new CuraTotal();
        listaItems.Add(curaTotal1);
        listaItems.Add(curaTotal2);

    }

    // Método para añadir Pokémon y comprobar si está listo
    public void AñadirASeleccion(Pokemon pokemon)
    {
        if (seleccionPokemonesVivos.Count < 6)
        {
            seleccionPokemonesVivos.Add(pokemon);
        }
    }

    public void AgregarAListaMuertos(Pokemon pokemon)
    {
            this.seleccionPokemonesVivos.Remove(pokemon);
            this.listaPokemonesMuertos.Add(pokemon);
    }

    public void RemoverDeListaMuertos(Pokemon pokemon)
    {
            this.listaPokemonesMuertos.Remove(pokemon);
            this.seleccionPokemonesVivos.Add(pokemon);
    }

    public void UsarPokemon(Pokemon pokemonAUsar)
    {
        pokemonEnUso = null;
        if (this.seleccionPokemonesVivos.Contains(pokemonAUsar))
        {
            pokemonEnUso = pokemonAUsar;
        }
    }

    public string GetListaDePokemones()
    {
        string resultado = "";

        foreach (Pokemon pokemon in seleccionPokemonesVivos)
        {
            if (pokemon.GetVida() > 0) // Tengo que especificar esto para cuando sean vencidos, no lo vuelvan a listas
            {
                string nombre = pokemon.GetNombre();
                Console.Write(nombre + " "); // Imprime cada nombre seguido de un espacio
                resultado += nombre + " "; // Agrega cada nombre a la cadena `resultado` seguido de un espacio
            }
        }

        Console.WriteLine();
        return resultado.Trim(); // Elimina el último espacio extra al final de la cadena
    }
    

    public string? GetStatusPokemonEnUso()
    {
        Pokemon pokemon = GetPokemonEnUso();
        return pokemon.EfectoActivo;
    }

/*
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
*/
    public void AceptarVisitorPorTurno(VisitorPorTurno visitor)
    {
        visitor.VisitarEntrenador(this);
    }
}

