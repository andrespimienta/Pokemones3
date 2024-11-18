#nullable enable
namespace Proyecto_Pokemones_I;

public interface IAtaque
{
    string GetNombre();
    string GetTipo();
    double GetDaño();
    int GetPrecision();
    bool GetEsEspecial();
    string? GetEfecto();
}