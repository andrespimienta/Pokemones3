using Discord.Commands;
using Discord.WebSocket;
using Ucu.Poo.DiscordBot.Commands;
using Ucu.Poo.DiscordBot.Domain;

public class LobbyCommands : ModuleBase<SocketCommandContext>
{
    /// <summary>
    /// Implementa el comando 'join'. Este comando une al jugador que envía el
    /// mensaje a la lista de jugadores esperando para jugar y los empareja automáticamente si hay al menos dos.
    /// </summary>
    [Command("join")]
    [Summary("Une el usuario que envía el mensaje a la lista de espera y empareja si hay otro jugador disponible")]
    public async Task JoinAsync()
    {
        // Obtiene los 3 atributos necesarios para crear un objeto Entrenador:
        ulong userID = Context.User.Id;  // Obtener el ID del usuario
        string displayName = CommandHelper.GetDisplayName(Context); // Obtiene el nombre del usuario/Entrenador
        SocketGuildUser? user = CommandHelper.GetUser(Context, displayName);
        
        // Solamente en esta instancia se puede obtener el canal. Por ende, es Expert
        ICanal canal = new CanalDeDiscord(Context.Channel);

        // Agrega al jugador a la lista de espera usando el método existente, fachada no queda acoplada a la plataforma que se use.
        Fachada.Instance.AddTrainerToWaitingList(userID, displayName,user,canal);
    }
    
    /// <summary>
    /// Implementa el comando 'leave' del bot. Este comando remueve el jugador
    /// que envía el mensaje de la lista de jugadores esperando para jugar.
    /// </summary>
    [Command("leave")]
    [Summary("Remueve el usuario que envía el mensaje de la lista de espera")]
    // ReSharper disable once UnusedMember.Global
    public async Task LeaveAsync()
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        ICanal canal = new CanalDeDiscord(Context.Channel);
        
        // Elimina al jugador de la lista de espera usando el método existente, fachada no queda acoplada a la plataforma que se use.
        Fachada.Instance.RemoveTrainerFromWaitingList(displayName,canal);
    }
    
    /// <summary>
    /// Implementa el comando 'waitinglist'. Este comando muestra la lista de
    /// jugadores esperando para jugar.
    /// </summary>
    [Command("waitingList")]
    [Summary("Muestra los usuarios en la lista de espera")]
    public async Task WaitingListAsync()
    {
        ICanal canal = new CanalDeDiscord(Context.Channel);
        
        // Muestra los jugadores en lista de espera, si es que los hay.
        Fachada.Instance.GetTrainersWaiting(canal);
    }
    
    /// <summary>
    /// Implementa el comando 'status'. Este comando muestra el estado
    /// de un jugador en específico (batallando, esperando o no esperando).
    /// </summary>
    [Command("status")]
    [Summary(
        """
        Devuelve el estado del usuario que se indica como parámetro o
        del usuario que envía el mensaje si no se indica otro usuario.
        """)]
    public async Task StatusAsync(
        [Remainder][Summary("El usuario del que tener información, opcional")] string? displayName = null)
    {
        ulong userID = Context.User.Id;
        ICanal canal = new CanalDeDiscord(Context.Channel);
        
        // Chequea que el jugador del que se quiere conseguir el status esté activo
        // (solo una clase 'Command' de Discord puede calcular eso, pedirselo a
        // la fachada sería romper la generalidad del programa y acoplarla a discord)
        if (displayName != null)
        {
            SocketGuildUser? user = CommandHelper.GetUser(Context, displayName);

            if (user == null)
            {
                string mensaje = $"{displayName} está desconectado o no forma parte de este servidor";
                Fachada.Instance.EnviarACanal(canal, mensaje);
            }
        }
        string userName = displayName ?? CommandHelper.GetDisplayName(Context);
        
        // Chequea el estatus del usuario indicado (si está en batalla, esperando o no)
        Fachada.Instance.TrainerStatus(userID, userName, canal);
    }
    
    /// <summary>
    /// Implementa el comando 'battle'. Este comando une al jugador que envía el
    /// mensaje a la lista de jugadores esperando para jugar.
    /// </summary>
    [Command("battle")]
    [Summary(
        """
        Une al jugador que envía el mensaje con el oponente que se recibe
        como parámetro, si lo hubiera, en una batalla; si no se recibe un
        oponente, lo une con cualquiera que esté esperando para jugar.
        """)]
    public async Task BattleAsync(
        [Remainder] [Summary("Display name del oponente, opcional")] string? opponentDisplayName = null)
    {
        string displayName = CommandHelper.GetDisplayName(Context);
        ICanal canal = new CanalDeDiscord(Context.Channel);
        
        // Intenta crear una batalla entre quien envió el comando y el oponente o alguien esperando para batallar
        Fachada.Instance.ChallengeTrainerToBattle(displayName, opponentDisplayName, canal);
    }
}
