namespace Proyecto_Pokemones_I;

public static class DiccionarioTipos // Clase estática que contiene los datos sobre los tipos
{
    private static Dictionary<string, List<string>> diccDebilContra = new Dictionary<string, List<string>>();
    private static Dictionary<string, List<string>> diccResistenteContra= new Dictionary<string, List<string>>();
    private static Dictionary<string, List<string>> diccInmuneContra= new Dictionary<string, List<string>>();
    
    // Métodos:
    public static List<string> GetDebilContra(string tipo)
    {
        return diccDebilContra[tipo];
    }
    public static List<string> GetResistenteContra(string tipo)
    {
        return diccResistenteContra[tipo];
    }
    public static List<string> GetInmuneContra(string tipo)
    {
        return diccInmuneContra[tipo];
    }

    // Constructor:
    static DiccionarioTipos()
    {
        // Formato: (Clave <Tipo>) Débil/Resistente/Inmune contra (Valor <Lista Tipos>)
        // Inmunidades:
        diccInmuneContra.Add("AGUA", [""]);
        diccInmuneContra.Add("BICHO", [""]);
        diccInmuneContra.Add("DRAGÓN", [""]);
        diccInmuneContra.Add("ELÉCTRICO", ["ELÉCTRICO"]);
        diccInmuneContra.Add("FANTASMA", [""]);
        diccInmuneContra.Add("FUEGO", [""]);
        diccInmuneContra.Add("HIELO", [""]);
        diccInmuneContra.Add("LUCHA", [""]);
        diccInmuneContra.Add("NORMAL", ["FANTASMA"]);
        diccInmuneContra.Add("PLANTA", [""]);
        diccInmuneContra.Add("PSÍQUICO", [""]);
        diccInmuneContra.Add("ROCA", [""]);
        diccInmuneContra.Add("TIERRA", [""]);
        diccInmuneContra.Add("VENENO", [""]);
        diccInmuneContra.Add("VOLADOR", [""]);
        
        // Resistencias:
        diccResistenteContra.Add("AGUA", ["AGUA", "FUEGO", "HIELO"]);
        diccResistenteContra.Add("BICHO", ["LUCHA", "PLANTA", "TIERRA"]);
        diccResistenteContra.Add("DRAGÓN", ["AGUA", "ELÉCTRICO", "FUEGO", "PLANTA"]);
        diccResistenteContra.Add("ELÉCTRICO", ["VOLADOR"]);
        diccResistenteContra.Add("FANTASMA", ["VENENO", "LUCHA", "NORMAL"]);
        diccResistenteContra.Add("FUEGO", ["BICHO", "FUEGO", "PLANTA"]);
        diccResistenteContra.Add("HIELO", ["HIELO"]);
        diccResistenteContra.Add("LUCHA", [""]);
        diccResistenteContra.Add("NORMAL", [""]);
        diccResistenteContra.Add("PLANTA", ["AGUA", "ELÉCTRICO", "PLANTA", "TIERRA"]);
        diccResistenteContra.Add("PSÍQUICO", [""]);
        diccResistenteContra.Add("ROCA", ["FUEGO", "NORMAL", "VENENO", "VOLADOR"]);
        diccResistenteContra.Add("TIERRA", ["ELÉCTRICO"]);
        diccResistenteContra.Add("VENENO", ["PLANTA", "VENENO"]);
        diccResistenteContra.Add("VOLADOR", ["BICHO", "LUCHA", "PLANTA", "TIERRA"]);
        
        // Debilidades:
        diccDebilContra.Add("AGUA", ["ELÉCTRICO", "PLANTA"]);
        diccDebilContra.Add("BICHO", ["FUEGO", "ROCA", "VOLADOR", "VENENO"]);
        diccDebilContra.Add("DRAGÓN", ["DRAGÓN", "HIELO"]);
        diccDebilContra.Add("ELÉCTRICO", ["TIERRA"]);
        diccDebilContra.Add("FANTASMA", ["FANTASMA"]);
        diccDebilContra.Add("FUEGO", ["AGUA", "ROCA", "TIERRA"]);
        diccDebilContra.Add("HIELO", ["FUEGO", "LUCHA", "ROCA"]);
        diccDebilContra.Add("LUCHA", ["PSÍQUICO", "VOLADOR", "BICHO", "ROCA"]);
        diccDebilContra.Add("NORMAL", ["LUCHA"]);
        diccDebilContra.Add("PLANTA", ["BICHO", "FUEGO", "HIELO", "VENENO", "VOLADOR"]);
        diccDebilContra.Add("PSÍQUICO", ["BICHO", "LUCHA", "FANTASMA"]);
        diccDebilContra.Add("ROCA", ["AGUA", "LUCHA", "PLANTA", "TIERRA"]);
        diccDebilContra.Add("TIERRA", ["AGUA", "HIELO", "PLANTA", "ROCA", "VENENO"]);
        diccDebilContra.Add("VENENO", ["BICHO", "PSÍQUICO", "TIERRA", "LUCHA", "PLANTA"]);
        diccDebilContra.Add("VOLADOR", ["ELÉCTRICO", "HIELO", "ROCA"]);
    }
}