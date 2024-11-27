using Discord.Commands;
using Library.Bot.Dominio;

namespace Library.Bot.Comandos
{
    public class BattleCommands : ModuleBase<SocketCommandContext>
    {
       
        [Command("Atacar")]
        public async Task AtacarAsync([Remainder] [Summary("Si no es null, usa dicho ataque. De lo contrario muesta la lista de ataques.")]
            string? attackNumber = null)
        {
            ulong usuarioId = Context.User.Id;
            Fachada.Instance.Atacar(usuarioId,attackNumber);
        }
        
        [Command("CambiarPokemon")]
        public async Task CambiarPokemonAsync([Remainder] [Summary("Si no es null, usa dicho ataque. De lo contrario muesta la lista de ataques.")]
            string? nombrePokemon = null)
        {
            ulong usuarioId = Context.User.Id;
            Fachada.Instance.CambiarPokemon(usuarioId,nombrePokemon);
        }
        
        [Command("UsarPocion")]
        public async Task UsarPocionAsync([Remainder] [Summary("Si no es null, usa dicho ataque. De lo contrario muesta la lista de ataques.")]
            string? nombrePokemon = null)
        {
            ulong usuarioId = Context.User.Id;
            Fachada.Instance.UsarPocion(usuarioId,nombrePokemon);
        }
    }
}




