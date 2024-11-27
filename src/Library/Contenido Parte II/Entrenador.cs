using Discord.WebSocket;
using Library.Bot.Dominio;
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
    public List<Item> GetListaItems()
    {
        return this.listaItems;
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
    
    // Relacionados a Items:
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

    public Item? RemoverItem(string nombrePocion)
    {
        Item result = null;
        foreach (Item pocion in this.listaItems)
        {
            if (pocion.Nombre == nombrePocion)
            {
                result = pocion;
                this.listaItems.Remove(pocion);
                break;
            }
        }

        return result;
    }
    
    public int GetCantidadItem(string nombreItem)
    {
        int cantidadTotal = 0;
        foreach (Item pocion in this.listaItems)
        {
            if (pocion.Nombre == nombreItem)
            {
                cantidadTotal += 1;
            }
        }

        return cantidadTotal;
    }
    
    
    // Relacionados a Pokemones:
    public void AñadirASeleccion(Pokemon pokemon)
    {
        if (seleccionPokemonesVivos.Count < 6)
        {
            seleccionPokemonesVivos.Add(pokemon);
        }
    }
    
    public int GetCantidadPokemonesVivos()
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

    public Pokemon? GetPokemonEnListaVivos(string nombrePokemon)
    {
        Pokemon result = null;
        foreach (Pokemon pokemon in this.seleccionPokemonesVivos)
        {
            if (pokemon.GetNombre() == nombrePokemon.ToUpper())
            {
                result = pokemon;
            }
        }

        return result;
    }
    
    public Pokemon? GetPokemonEnListaMuertos(string nombrePokemon)
    {
        Pokemon result = null;
        foreach (Pokemon pokemon in this.listaPokemonesMuertos)
        {
            if (pokemon.GetNombre() == nombrePokemon.ToUpper())
            {
                result = pokemon;
            }
        }

        return result;
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
    
    public async Task AceptarVisitorPorTurno(VisitorPorTurno visitor)
    {
        await visitor.VisitarEntrenador(this);
    }
    
    public int ChancesGanar() //lo hago por cada jugador por separado, osea que si quisira fijarme las chances de los dos jugadores tendria que llamar a la funcion dos veces
    {//items 30ptos, envenenado 10 o 0, pokesvivos 60 dies por cada uno
        //int chancesDeGanarl = 0;
        int pokesVivos = this.GetCantidadPokemonesVivos();
        int chancesDeGanarl = pokesVivos * 10;
        int cantItems = 0;
        List<Item> listaItem = this.GetListaItems();
        foreach (Item item in listaItem)
        {
            cantItems++;
        }

        if (cantItems == 7)//cada entrenador deberia de empezar con 7 items, 4 superPOsicones 2 Posiones y 1 Revivir
        {
            chancesDeGanarl += 30;
        }else if (cantItems == 6)
        {
            chancesDeGanarl += 25;
        }else if (cantItems == 5)
        {
            chancesDeGanarl += 20;
        }else if (cantItems == 4)
        {
            chancesDeGanarl += 15;
        }else if (cantItems == 3)
        {
            chancesDeGanarl += 10;
        }else if (cantItems == 2)
        {
            chancesDeGanarl += 5;
        }else if (cantItems == 1)
        {
            chancesDeGanarl += 0;
        }else if (cantItems == 0)
        {
            chancesDeGanarl -= 1;
        }

        //bool algunoEnvenenado = false;
        List<Pokemon> listaPokemonesSelect = this.GetSeleccion();
        int chancesPorInfeccion = 10;
        foreach (Pokemon poke in listaPokemonesSelect)
        {
            if (poke.EfectoActivo != null)
            {
                chancesPorInfeccion = 0;
            }
        }
        chancesDeGanarl += chancesPorInfeccion;
        //Fachada facha = Fachada.Instance; esto en verdad lo hice para mandarle un msj al usuario por privado las chances que tiene pero si lo hago aca lo muestro en cualqueier momento
        //facha.EnviarAUsuario(this.GetSocketGuildUser(), $"Tus chances de ganar actualmente son de:{chancesDeGanarl}");
        return chancesDeGanarl;
    }
}

