namespace Proyecto_Pokemones_I;

public class AtaqueEspecial:IAtaque
{
    // Atributos:
    private string nombre;
    private string tipo;
    private double daño;
    private int precision;
    private string efectoNegativo;
    private bool esEspecial;

    // Getters:
    public string GetNombre()
    {
        return this.nombre;
    }
    public string GetTipo()
    {
        return this.tipo;
    }
    public double GetDaño()
    {
        return this.daño;
    }
    public int GetPrecision()
    {
        return this.precision;
    }
    public string GetEfecto()
    {
        return this.efectoNegativo;
    }
    public bool GetEsEspecial()
    {
        return this.esEspecial;
    }

    public bool GetEsCritico()
    {
        List<bool>critic = new List<bool> {true,false,false,false,false,false,false,false,false,false};
        Random rand = new Random();
        int indiceAleatorio = rand.Next(critic.Count);
        if (critic[indiceAleatorio] == true)
        {
            Console.WriteLine("El ataque será crítico");
        }
        return critic[indiceAleatorio];
    } //Devuelve si el crítico se da
    
    public bool GetEsPreciso()
    {
        List<bool>preciso = new List<bool> {true,false};
        Random rand = new Random();
        int indiceAleatorio = rand.Next(preciso.Count);
        if (preciso[indiceAleatorio] == true)
        {
            Console.WriteLine("El ataque será preciso");
        }
        return preciso[indiceAleatorio];
    }//Devuelve si el preciso se da

    
    
    //Constructor:
    public AtaqueEspecial(string nombreAtaque, string tipoAtaque, double dañoAtaque, int precisionAtaque, string efecto)
    {
        this.nombre = nombreAtaque;
        this.tipo = tipoAtaque;
        this.daño = dañoAtaque;
        this.precision = precisionAtaque;
        this.efectoNegativo = efecto;
        this.esEspecial = true;
    }
}