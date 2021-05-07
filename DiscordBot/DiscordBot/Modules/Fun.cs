using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Fun : ModuleBase
    {
        private readonly ILogger<Fun> Logger;

        public Fun(ILogger<Fun> logger) => Logger = logger;

        [Command("meme")]
        [Alias("reddit")]
        public async Task Meme(string subreddit = null)
        {
            var channel = Context.Channel as SocketTextChannel;

            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync($"https://reddit.com/r/{subreddit ?? "memes"}/random.json?limit=1");
            
            if(!result.StartsWith("["))
            {
                await channel.SendMessageAsync("There is no subreddit with this name!");
                return;
            }

            var array = JArray.Parse(result);
            var post = JObject.Parse(array[0]["data"]["children"][0]["data"].ToString());

            var builder = new EmbedBuilder()
                .WithImageUrl(post["url"].ToString())
                .WithColor(new Color(33, 176, 252))
                .WithTitle(post["title"].ToString())
                .WithUrl($"https://reddit.com{post["permalink"]}")
                .WithFooter($"💬 {post["num_comments"]} ⬆️ {post["ups"]}");
            
            var embed = builder.Build();

            await channel.SendMessageAsync(null, false, embed);
        }
    }
}
