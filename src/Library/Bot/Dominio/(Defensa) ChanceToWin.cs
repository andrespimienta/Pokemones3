using Library.Contenido_Parte_II;

namespace Library.Bot.Dominio;

public abstract class ChanceToWin
{
    // ESTE ES EL MÉTODO AÑADIDO EN LA DEFENSA (ESTEBAN DURÁN).
    // TIENE SU PROPIA IMPLEMENTACIÓN EN FACHADA, BIEN ABAJO, AL FINAL.
    public static int CalcularChanceToWin(Entrenador jugador)
    {
        int resultado = 0;
        bool pokemonAfectado = false;

        // Revisa la selección de Pokemones Vivos del jugador
        foreach (Pokemon pokemon in jugador.GetSeleccion())
        {
            double porcentajeVida = (pokemon.GetVida() * 100) / pokemon.GetVidaMax();
            
            // Asigna puntos según el porcentaje de vida
            switch (porcentajeVida)
            {
                // De 80% para arriba, 10 puntos
                case >= 80:
                {
                    resultado += 10;
                    break;
                }
                // De 60% para arriba, 8 puntos
                case >= 60:
                {
                    resultado += 8;
                    break;
                }
                // De 40% para arriba, 6 puntos
                case >= 40:
                {
                    resultado += 6;
                    break;
                }
                // De 20% para arriba, 4 puntos
                case >= 20:
                {
                    resultado += 4;
                    break;
                }
                // De 1% para arriba, 2 puntos
                case > 0:
                {
                    resultado += 2;
                    break;
                }
            }
            
            // Revisa si el Pokemon tiene efectos negativos
            if (pokemon.EfectoActivo != null)
            {
                pokemonAfectado = true;
            }
        }
        
        // Son 7 ítems en total, cada ítem pesa 4 puntos, por lo que el
        // puntaje máximo al tener todos los items será 28 (en lugar de 30)
        resultado += jugador.GetCantidadItem("Súper Poción") * 4;
        resultado += jugador.GetCantidadItem("Cura Total") * 4;
        resultado += jugador.GetCantidadItem("Revivir") * 4;

        // Si no encontró ningún Pokemon con efecto negativo, suma 10 puntos
        if (pokemonAfectado == false)
        {
            resultado += 10;
        }

        return resultado;
    }
}