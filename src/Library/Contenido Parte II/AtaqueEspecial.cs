using Proyecto_Pokemones_I;

namespace Library.Contenido_Parte_II;

/// <inheritdoc />
public class AtaqueEspecial:Ataque
{
    //Constructor:
    /// <summary>
    /// Hereda de la clase abstracta "Ataque", con un efecto y esEspecial = true
    /// </summary>
    /// <param name="nombreAtaque"></param>
    /// <param name="tipoAtaque"></param>
    /// <param name="dañoAtaque"></param>
    /// <param name="precisionAtaque"></param>
    /// <param name="efecto"></param>
    public AtaqueEspecial(string nombreAtaque, string tipoAtaque, double dañoAtaque, int precisionAtaque, string efecto)
        :base(nombreAtaque,tipoAtaque,dañoAtaque,precisionAtaque)
        {
            this.efectoNegativo = efecto;
            this.esEspecial = true;
        }
}