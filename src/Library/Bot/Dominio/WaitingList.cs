using Discord.WebSocket;
using Library.Contenido_Parte_II;
using Proyecto_Pokemones_I;

namespace Library.Bot.Dominio;

/// <summary>
/// Esta clase representa la lista de jugadores esperando para jugar.
/// </summary>
public class WaitingList
{
    public readonly List<Entrenador> entrenadores = new List<Entrenador>();

    public int Count
    {
        get { return this.entrenadores.Count; }
    }

    public string GetListaDeEspera()
    {
        string result = "";
        foreach (Entrenador entrenador in entrenadores)
        {
            result += entrenador.GetNombre() + ", "; // Separa los nombres con una coma y un espacio
        }
        result = result.Substring(0,result.Length-2); // Elimina la última coma y el espacio al final.
        return result; 
    }

    
    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre de usuario de Discord en el servidor
    /// del bot a agregar.
    /// </param>
    /// <returns><c>true</c> si se agrega el usuario; <c>false</c> en caso
    /// contrario.</returns>
    public bool AgregarEntrenador(ulong userId ,string displayName, SocketGuildUser user)
    {
        if (string.IsNullOrEmpty(displayName))
        {
            throw new ArgumentException(nameof(displayName));
        }
        
        if (EncontrarEntrenador(displayName) != null) return false;
        entrenadores.Add(new Entrenador(displayName,userId,user));
        return true;

    }

    /// <summary>
    /// Remueve un jugador de la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre de usuario de Discord en el servidor
    /// del bot a remover.
    /// </param>
    /// <returns><c>true</c> si se remueve el usuario; <c>false</c> en caso
    /// contrario.</returns>
    public bool EliminarEntrenador(string displayName)
    {
        Entrenador? trainer = this.EncontrarEntrenador(displayName);
        if (trainer == null) return false;
        entrenadores.Remove(trainer);
        return true;
    }

    /// <summary>
    /// Busca un jugador por el nombre de usuario de Discord en el servidor del
    /// bot.
    /// </summary>
    /// <param name="displayName">El nombre de usuario de Discord en el servidor
    /// del bot a buscar.
    /// </param>
    /// <returns>El jugador encontrado o <c>null</c> en caso contrario.
    /// </returns>
    public Entrenador? EncontrarEntrenador(string displayName)
    {
        Entrenador result = null;
        foreach (Entrenador trainer in this.entrenadores)
        {
            if (trainer.GetNombre() == displayName)
            { 
                result = trainer;
            }
        }
        return result;
    }

    /// <summary>
    /// Retorna un jugador cualquiera esperando para jugar. En esta
    /// implementación provista no es cualquiera, sino el primero. En la
    /// implementación definitiva, debería ser uno aleatorio.
    /// 
    /// </summary>
    /// <returns></returns>
    public Entrenador? GetAlguienEsperando(string nombreDeQuienBusca)
    {
        // Si no hay nadie esperando, retorna null
        if (this.entrenadores.Count == 0)
        {
            return null;
        }
        // Sino, busca quienes están esperando
        foreach (Entrenador trainer in this.entrenadores)
        {
            // Si el entrenador encontrado no es el mismo que envió el comando, lo devuelve
            if (trainer.GetNombre() != nombreDeQuienBusca)
            {
                return trainer;
            }
        }
        // Si el único entrenador en lista de espera es quien envió el comando, devuelve null
        return null;
    }
   
    
}
