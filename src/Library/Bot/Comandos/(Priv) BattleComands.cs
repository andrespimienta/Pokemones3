using Discord.Commands;
using Discord.WebSocket;
using Proyecto_Pokemones_I;
using System.Threading.Tasks;
using Discord;
using Ucu.Poo.DiscordBot.Domain;



namespace Ucu.Poo.DiscordBot.Commands
{
    public class BattleCommands : ModuleBase<SocketCommandContext>
    {
       
        [Command("Atacar")]
        public async Task atacar([Remainder] [Summary("Si no es null, usa dicho ataque. De lo contrario muesta la lista de ataques.")]
            string? attackName = null)
        {
            ulong usuarioId = Context.User.Id;
            Fachada.Instance.Atacar(usuarioId,attackName);
        }
    }
}




