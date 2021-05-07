using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Moderation : ModuleBase
    {
        private readonly ILogger<Moderation> Logger;

        public Moderation(ILogger<Moderation> logger) => Logger = logger;

        [Command("purge")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task Purge(int amount)
        {
            var channel = Context.Channel as SocketTextChannel;

            var messages = await channel.GetMessagesAsync(amount + 1).FlattenAsync();
            await channel.DeleteMessagesAsync(messages);

            var message = await channel.SendMessageAsync($"{messages.Count()} messages were deleted successfully!");
            await Task.Delay((int)2.5 * 1000);
            await message.DeleteAsync();
        }
    }
}
