using Discord.Commands;
using Discord.WebSocket;

namespace Library.Bot.Comandos;

public static class CommandHelper
{
    public static string GetDisplayName(
        SocketCommandContext context, 
        string? name = null)
    {
        if (name == null)
        {
            name = context.Message.Author.Username;
        }
        
        foreach (SocketGuildUser user in context.Guild.Users)
        {
            if (user.Username == name
                || user.DisplayName == name
                || user.Nickname == name
                || user.GlobalName == name)
            {
                return user.DisplayName;
            }
        }

        return name;
    }

    public static SocketGuildUser? GetUser(
        SocketCommandContext context,
        string? name)
    {
        
        foreach (SocketGuildUser user in context.Guild.Users)
        {
            if (user.Username == name
                || user.DisplayName == name
                || user.Nickname == name
                || user.GlobalName == name)
            {
                return user;
            }
        }

        return null;
    }
}