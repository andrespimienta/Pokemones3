using System.Globalization;
using Library.Contenido_Parte_II;

namespace Proyecto_Pokemones_I;

public static class LeerArchivo
{

    public static string RutaCatalogo = "/Users/andrespimienta/Desktop/Facultad/Programación 2/Proyecto/Pokemones3/src/Program/Catalogo";

    public static string ObtenerCatalogoProcesado()
    {
        string catalogo = "";  // Variable para almacenar el catálogo formateado

        if (!File.Exists(RutaCatalogo))
        {
            Console.WriteLine("Error: El archivo no existe en la ruta especificada.");
            return "No se pudo encontrar el catálogo.";
        }

        string[] lineas = File.ReadAllLines(RutaCatalogo);
        for (int indice = 2; indice < lineas.Length; indice++)
        {
            string[] datos = lineas[indice].Split(',');

            // Construir el mensaje del catálogo, agregando los datos de cada Pokémon
            catalogo += "----------------------------------------------------------------------\n" +
                        $"Nombre: {datos[0]}, Tipo: {datos[1]},\n" +
                        $"Vida: {datos[2]}, Velocidad de Ataque: {datos[3]}\n";
        }
        catalogo += "----------------------------------------------------------------------";  // Añadir línea final

        return catalogo;  // Devolver el catálogo como un string
    }


    
    public static Pokemon? EncontrarPokemon(string nombrePokemon)
    {
        nombrePokemon = nombrePokemon.ToUpper(); // Evito errores por mayúsculas o minúsculas en el parámetro

        // Lee todas las líneas del archivo hasta encontrar la línea que contenga al Pokemon indicado:
        string[] lineas = File.ReadAllLines(RutaCatalogo);
        int indice = -1;
        for (int i = 0; i < lineas.Length; i++)
        {
            // Comprueba si la línea contiene el nombre del Pokemon
            if (lineas[i].Contains(nombrePokemon, StringComparison.OrdinalIgnoreCase))
            {
                string[] datos = lineas[i].Split(',');
                if (datos[0] == nombrePokemon) // Encontro la palabra, pero puede que no sea el pokemon
                {
                    indice = i; // Guarda el índice de la línea donde se encontró el Pokémon
                    break; // Salir del bucle si se encuentra el Pokémon
                }
            }
        }
        // Si encontró dicha linea, la separa dato por dato
        if (indice != -1)
        {
            string[] datos = lineas[indice].Split(',');
            if (datos.Length == 21)
            {
                //Encuentra y asigna los atributos del Pokemon:
                string pokeNombre = datos[0];
                string pokeTipo = datos[1];
                double pokeVida = double.Parse(datos[2]);
                double pokeVelAtaque = double.Parse(datos[3]);

                //Encuentra y crea el Ataque 1:
                string ataqueNombre = datos[4];
                string ataqueTipo = datos[5];
                double ataqueDaño = double.Parse(datos[6]);
                int ataquePrecision = int.Parse(datos[7]);
                AtaqueBasico ataque1 = new AtaqueBasico(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision);

                //Encuentra y crea el Ataque 2:
                ataqueNombre = datos[8];
                ataqueTipo = datos[9];
                ataqueDaño = double.Parse(datos[10]);
                ataquePrecision = int.Parse(datos[11]);
                AtaqueBasico ataque2 = new AtaqueBasico(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision);

                //Encuentra y crea el Ataque 3:
                ataqueNombre = datos[12];
                ataqueTipo = datos[13];
                ataqueDaño = double.Parse(datos[14]);
                ataquePrecision = int.Parse(datos[15]);
                AtaqueBasico ataque3 = new AtaqueBasico(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision);

                //Encuentra y crea el Ataque 4:
                ataqueNombre = datos[16];
                ataqueTipo = datos[17];
                ataqueDaño = double.Parse(datos[18]);
                ataquePrecision = int.Parse(datos[19]);
                string ataqueEfecto = datos[20];
                AtaqueEspecial ataque4 = new AtaqueEspecial(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision, ataqueEfecto);

                //Define el atributo Lista de Ataques del Pokemon:
                List<Ataque> pokeAtaques = new List<Ataque>();
                pokeAtaques.Add(ataque1);
                pokeAtaques.Add(ataque2);
                pokeAtaques.Add(ataque3);
                pokeAtaques.Add(ataque4);

                //Instancia al Pokemon y lo devuelve:
                Pokemon pokemon = new Pokemon(pokeNombre, pokeTipo, pokeVida, pokeVelAtaque, pokeAtaques);
                return pokemon;
            }
            // Si faltan datos en la línea, devuelve el error:
            else
            {
                Console.WriteLine("Revise el catálogo, faltan datos sobre el Pokemon especificado");
                return null;
            }
        }
        // Si no encontró dicha linea, imprime el error:
        else
        {
            Console.WriteLine("No se ha encontrado el Pokemon especificado");
            return null;
        }
    }
}