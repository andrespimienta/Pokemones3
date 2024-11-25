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
                       $"➡️ {datos[0]}, 🏷️ {datos[1]}, 🔖 {datos[2]}, ❤️ {datos[3]}, ⚡ {datos[4]}\n";
        }
        catalogo += "----------------------------------------------------------------------";  // Añadir línea final

        return catalogo;  // Devolver el catálogo como un string
    }


    
   public static Pokemon? EncontrarPokemon(string numeroIdentificador)
    {
        // Lee todas las líneas del archivo
        string[] lineas = File.ReadAllLines(RutaCatalogo);
        int indice = -1;

         for (int i = 0; i < lineas.Length; i++)
         {
             // Separar la línea actual en datos usando ','
             string[] datos = lineas[i].Split(',');

             // Verificar que el primer dato (el identificador) sea igual al número identificador buscado
             if (datos[0].Trim() == numeroIdentificador.Trim())
             {
                 indice = i; // Guarda el índice de la línea donde se encontró el Pokémon
                 break; // Salir del bucle si se encuentra el Pokémon
             }
         }

         // Si encontró dicha línea, la separa dato por dato
         if (indice != -1)
         {
             string[] datos = lineas[indice].Split(',');

             if (datos.Length == 22) // Ahora la longitud esperada es 22 porque incluye el número identificador
             {
                 //Encuentra y asigna los atributos del Pokemon:
                 string identificador = datos[0];
                 string pokeNombre = datos[1];
                 string pokeTipo = datos[2];
                 double pokeVida = double.Parse(datos[3]);
                 double pokeVelAtaque = double.Parse(datos[4]);

                 // Encuentra y crea el Ataque 1:
                 string ataqueNombre = datos[5];
                 string ataqueTipo = datos[6];
                 double ataqueDaño = double.Parse(datos[7]);
                 int ataquePrecision = int.Parse(datos[8]);
                 AtaqueBasico ataque1 = new AtaqueBasico(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision);

                 // Encuentra y crea el Ataque 2:
                 ataqueNombre = datos[9];
                 ataqueTipo = datos[10];
                 ataqueDaño = double.Parse(datos[11]);
                 ataquePrecision = int.Parse(datos[12]);
                 AtaqueBasico ataque2 = new AtaqueBasico(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision);

                 // Encuentra y crea el Ataque 3:
                 ataqueNombre = datos[13];
                 ataqueTipo = datos[14];
                 ataqueDaño = double.Parse(datos[15]);
                 ataquePrecision = int.Parse(datos[16]);
                 AtaqueBasico ataque3 = new AtaqueBasico(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision);

                 // Encuentra y crea el Ataque 4:
                 ataqueNombre = datos[17];
                 ataqueTipo = datos[18];
                 ataqueDaño = double.Parse(datos[19]);
                 ataquePrecision = int.Parse(datos[20]);
                 string ataqueEfecto = datos[21];
                 AtaqueEspecial ataque4 = new AtaqueEspecial(ataqueNombre, ataqueTipo, ataqueDaño, ataquePrecision, ataqueEfecto);

                 // Define el atributo Lista de Ataques del Pokemon:
                 List<Ataque> pokeAtaques = new List<Ataque>
                 {
                     ataque1,
                     ataque2,
                     ataque3,
                     ataque4
                 };

                 // Instancia al Pokemon y lo devuelve:
                 Pokemon pokemon = new Pokemon(pokeNombre, pokeTipo, pokeVida, pokeVelAtaque, pokeAtaques, identificador);
                 return pokemon;
             }
             else
             {
                 Console.WriteLine("Revise el catálogo, faltan datos sobre el Pokemon especificado");
                 return null;
             }
         }
         else
         {
             Console.WriteLine("No se ha encontrado el Pokemon con el número identificador especificado");
             return null;
         }
    }


}