using System.Collections.ObjectModel;
using Discord.WebSocket;
using Proyecto_Pokemones_I;

namespace Ucu.Poo.DiscordBot.Domain;

/// <summary>
/// Esta clase representa la lista de jugadores esperando para jugar.
/// </summary>
public class ListaDeEspera
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
            result += entrenador.GetNombre() + " "; // O el delimitador que prefieras
        }

        return result.Trim(); // Elimina el último espacio en blanco innecesario
    }

    
    /// <summary>
    /// Agrega un jugador a la lista de espera.
    /// </summary>
    /// <param name="displayName">El nombre de usuario de Discord en el servidor
    /// del bot a agregar.
    /// </param>
    /// <returns><c>true</c> si se agrega el usuario; <c>false</c> en caso
    /// contrario.</returns>
    public bool AgregarEntrenador(ulong userId ,string displayName, SocketGuildUser? user)
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
    public Entrenador? GetAlguienEsperando()
    {
        if (this.entrenadores.Count == 0)
        {
            return null;
        }

        return this.entrenadores[0];
    }
   
    
}
