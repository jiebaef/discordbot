using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    internal class CommandHandler
    {
        public static IServiceProvider Provider;
        private readonly DiscordSocketClient Client;
        private readonly CommandService Commands;
        private readonly IConfigurationRoot Configuration;
        private readonly Logger Logger;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService commands, IConfigurationRoot configuration, Logger logger)
        {
            Provider = provider;
            Client = client;
            Commands = commands;
            Configuration = configuration;
            Logger = logger;

            Client.Ready += OnReady;
            Client.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message.Author.IsBot)
                return;

            var context = new SocketCommandContext(Client, message);
            var prefixPosition = 0;

            if (message.HasStringPrefix(Configuration["prefix"], ref prefixPosition) || message.HasMentionPrefix(Client.CurrentUser, ref prefixPosition))
            {
                var result = await Commands.ExecuteAsync(context, prefixPosition, Provider);


                if (!result.IsSuccess)
                {
                    var reason = result.Error;

                    await context.Channel.SendMessageAsync($"The following error occured:\n{reason}");
                    await Logger.Log(new LogMessage(LogSeverity.Error, nameof(CommandHandler), reason.ToString()));
                }
            }
        }

        private async Task OnReady()
        {
            await Logger.Log(new LogMessage(LogSeverity.Debug, nameof(CommandHandler), $"Ready as {Client.CurrentUser.Username}#{Client.CurrentUser.Discriminator}"));
        }
    }
}
