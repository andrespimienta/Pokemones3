using System.Runtime.CompilerServices;

namespace Proyecto_Pokemones_I;

public class Pokemon
{
    private string nombre;
    private string tipo;
    private double vida;
    private double vidaMax;
    private double velocidadAtaque;
    private List<IAtaque> listadoAtaques;
    public string EfectoActivo { get; set;}
    public bool PuedeAtacar { get; set; }
    public int TurnosDuracionEfecto { get; set; }
    
    //Getters:
    public string GetNombre()
    {
        return this.nombre;
    }
    public string GetTipo()
    {
        return this.tipo;
    }
    public double GetVida()
    {
        return this.vida;
    }
    public double GetVidaMax()
    {
        return this.vidaMax;
    }
    public double GetVelocidadAtaque()
    {
        return this.velocidadAtaque;
    }
    public List<IAtaque> GetAtaques()
    {
        return this.listadoAtaques;
    }

    

    //Constructor:
    public Pokemon(string pokeNombre, string pokeTipo, double pokeVida, double pokeVelAtaque, List<IAtaque> ataques)
    {
        this.nombre = pokeNombre;
        this.tipo = pokeTipo;
        this.vida = pokeVida;
        this.vidaMax = pokeVida;
        this.velocidadAtaque = pokeVelAtaque;
        this.listadoAtaques = ataques;
        this.EfectoActivo = null;
        this.PuedeAtacar = true;
        this.TurnosDuracionEfecto = 0;
    }
    
    // Métodos:
    public void DañoPorTurno(double dañoEspecial)
    {
        this.vida = vida-dañoEspecial;
    }
    
    public void RecibirDaño(IAtaque ataqueRecibido)
    {
        DiccionarioTipos.GetInstancia();
        List<string> listaDebilidades = DiccionarioTipos.GetDebilContra(this.tipo);
        List<string> listaResistencias = DiccionarioTipos.GetResistenteContra(this.tipo);
        List<string> listaInmunidades = DiccionarioTipos.GetInmuneContra(this.tipo);
        double dañoTotal = 0;

        // Si el ataque fue preciso (resultado aplicar el Probabilometro a la Precision), calculará el daño:
        if (ProbabilityUtils.Probabilometro(ataqueRecibido.GetPrecision()))   
        {
            if (listaInmunidades.Contains(ataqueRecibido.GetTipo()))    // Si el tipo del ataque está en los tipos a los que es inmune, Daño x0
            {
                dañoTotal = ataqueRecibido.GetDaño() * 0;
                Console.WriteLine($"{this.nombre} es inmune a los ataques de tipo {ataqueRecibido.GetTipo()}, no recibió daño");
            }
            else if (listaResistencias.Contains(ataqueRecibido.GetTipo()))  // Si el tipo del ataque está en los tipos a los que es resistente, Daño x0.5
            {
                dañoTotal = ataqueRecibido.GetDaño() * 0.5;
                Console.WriteLine($"{this.nombre} es resistente a los ataques de tipo {ataqueRecibido.GetTipo()}, recibió la mitad del daño");
            }
            else if (listaDebilidades.Contains(ataqueRecibido.GetTipo()))   // Si el tipo del ataque está en los tipos a los que es débil, Daño x2
            {
                dañoTotal = ataqueRecibido.GetDaño() * 2;
                Console.WriteLine($"{this.nombre} es débil a los ataques de tipo {ataqueRecibido.GetTipo()}, recibió el doble del daño");
            }
            else    // Si el tipo del ataque no pertenece a los tipos a los que es inmune, resistente, ni débil, Daño x1
            {
                dañoTotal = ataqueRecibido.GetDaño();
            }
            
            
            // Si no es inmune al ataque, fue preciso y además crítico (aplica Probabilomtero al 10% de chance), agrega un 20% extra de daño:
            if (ProbabilityUtils.Probabilometro(10) && !listaInmunidades.Contains(ataqueRecibido.GetTipo()))
            {
                dañoTotal = dañoTotal * 1.20;
                Console.WriteLine($"¡El ataque fue crítico, {this.nombre} recibió daño extra!");
            }
            this.vida -= dañoTotal; // Cuando se calculó finalmente el daño, se lo resta a la vida
            

            // Si no es inmune al ataque, fue preciso y además era un ataque Especial, intentará aplicarle el efecto:
            if (ataqueRecibido.GetEsEspecial() && !listaInmunidades.Contains(ataqueRecibido.GetTipo()))
            {
                string efectoAtaque = ataqueRecibido.GetEfecto();
                if (EfectoActivo == null)
                {
                    EfectoActivo = efectoAtaque.Substring(0,efectoAtaque.Length - 1) + "DO";
                    // Aclaración: "Dormi" + "do" | "Paraliza" + "do" | "Envenena" + "do" | "Quema" + "do"
                    Console.WriteLine($"{this.nombre} ahora está {EfectoActivo}");
                    
                    // Si el efecto era Dormir, genera una duración de efecto de entre 1 y 4 turnos
                    if (ataqueRecibido.GetEfecto() == "DORMIR")
                    {
                        this.TurnosDuracionEfecto = ProbabilityUtils.DuracionEfecto(1,4);
                    }
                }
                else
                {
                    Console.WriteLine($"El pokemon ya está {EfectoActivo}");
                }
            }
        }
        // Si el ataque no fue preciso, no hace nada (no resta vida ni provoca efecto, lo erra)
        else
        {
            Console.WriteLine($"¡El ataque fue impreciso, no impactó!");
        }
    }

    public void AlterarVida(double hp)
    {
        this.vida += hp;
    }

    public void Revivir()
    {
        this.vida = this.vidaMax / 2;
    }
}