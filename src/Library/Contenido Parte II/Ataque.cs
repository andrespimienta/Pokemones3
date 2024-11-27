namespace Library.Contenido_Parte_II;

public abstract class Ataque
{
    // Atributos:
    private string nombre;
    private string tipo;
    private double daño;
    private int precision;
    protected bool esEspecial;
    protected string? efectoNegativo;

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
    public bool GetEsEspecial()
    {
        return this.esEspecial;
    }
    public string? GetEfecto()
    {
        return this.efectoNegativo;
    }

    // Constructor:
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nombreAtaque"></param>
    /// <param name="tipoAtaque"></param>
    /// <param name="dañoAtaque"></param>
    /// <param name="precisionAtaque"></param>
    public Ataque(string nombreAtaque, string tipoAtaque, double dañoAtaque, int precisionAtaque)
    {
        this.nombre = nombreAtaque;
        this.tipo = tipoAtaque;
        this.daño = dañoAtaque;
        this.precision = precisionAtaque;
    }

    
}