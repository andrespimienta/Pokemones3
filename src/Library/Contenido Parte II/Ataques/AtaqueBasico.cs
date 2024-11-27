#nullable enable
namespace Library.Contenido_Parte_II.Ataques;

/// <inheritdoc />
public class AtaqueBasico:Ataque
{
    // Constructor:
    /// <summary>
    /// Hereda de la clase abstracta "Ataque", sin efectos y esEspecial = false
    /// </summary>
    /// <param name="nombreAtaque"></param>
    /// <param name="tipoAtaque"></param>
    /// <param name="dañoAtaque"></param>
    /// <param name="precisionAtaque"></param>
    public AtaqueBasico(string nombreAtaque, string tipoAtaque, double dañoAtaque, int precisionAtaque)
        :base(nombreAtaque,tipoAtaque,dañoAtaque,precisionAtaque)
        {
            this.efectoNegativo = null;
            this.esEspecial = false;
        }
}