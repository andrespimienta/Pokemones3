using Discord.Commands;
using Ucu.Poo.DiscordBot.Domain;

namespace Library.Bot.Comandos
{
    public class BattleCommands : ModuleBase<SocketCommandContext>
    {
       
        [Command("Atacar")]
        public async Task atacar([Remainder] [Summary("Si no es null, usa dicho ataque. De lo contrario muesta la lista de ataques.")]
            string? attackNumber = null)
        {
            ulong usuarioId = Context.User.Id;
            Fachada.Instance.Atacar(usuarioId,attackNumber);
        }
        
        [Command("CambiarPokemon")]
        public async Task cambiarpokemon([Remainder] [Summary("Si no es null, usa dicho pokemon. De lo contrario muesta la lista de pokemones.")]
            string? nombrePokemon = null)
        {
            ulong usuarioId = Context.User.Id;
            Fachada.Instance.CambiarPokemon(usuarioId,nombrePokemon);
        }
        
        
        
        
        
        
        [Command("Rendirse")]
        public async Task Rendirse()
        {
            ulong userID = Context.User.Id;  // Obtener el ID del usuario
        
            Fachada.Instance.Rendirse(userID);
        }
    }
}




