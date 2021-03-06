using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class General : ModuleBase
    {
        private readonly ILogger<General> Logger;

        public General(ILogger<General> logger) => Logger = logger;

        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
        }

        [Command("info")]
        public async Task Info(SocketGuildUser user = null)
        {
            if (user == null)
                user = Context.User as SocketGuildUser;
            var channel = Context.Channel as SocketTextChannel;

            var name = user.Username;


            var builder = new EmbedBuilder()
                .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                .WithDescription($"In this message you can see some information about {name}!")
                .WithColor(new Color(33, 176, 252))
                .AddField("User ID", user.Id, true)
                .AddField("Created at", user.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Joined at", user.JoinedAt.Value.ToString("dd/MM/yyyy"), true)
                .AddField("Roles", string.Join(" ", user.Roles.Select(role => role.Mention)))
                .WithCurrentTimestamp();

            var embed = builder.Build();

            await channel.SendMessageAsync(null, false, embed);
        }

        [Command("server")]
        public async Task Server()
        {
            var guild = Context.Guild as SocketGuild;
            var channel = Context.Channel as SocketTextChannel;

            var builder = new EmbedBuilder()
                .WithThumbnailUrl(guild.IconUrl)
                .WithDescription("In this message you can see some information about the current server.")
                .WithTitle($"{guild.Name} Information")
                .WithColor(new Color(33, 176, 252))
                .AddField("Created at", guild.CreatedAt.ToString("dd/MM/yyyy"), true)
                .AddField("Membercount", $"{guild.MemberCount} members", true)
                .AddField("Online users", $"{guild.Users.Where(user => user.Status != UserStatus.Offline).Count()} members", true);

            var embed = builder.Build();

            await channel.SendMessageAsync(null, false, embed);
        }

        [Command("test")]
        public async Task Test()
        {
            var user = Context.User as SocketGuildUser;

            await ReplyAsync("Testing this function");
            Logger.LogInformation($"{user.Username} tested this function");
        }
    }
}
