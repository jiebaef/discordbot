using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    internal class CommandHandler : InitializedService
    {
        public static IServiceProvider Provider;
        private readonly DiscordSocketClient Client;
        private readonly CommandService CommandService;
        private readonly IConfiguration Configuration;
        private readonly ILogger<CommandHandler> Logger;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService commandService, IConfiguration configuration, ILogger<CommandHandler> logger)
        {
            Provider = provider;
            Client = client;
            CommandService = commandService;
            Configuration = configuration;
            Logger = logger;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            Client.MessageReceived += OnMessageReceived;
            CommandService.CommandExecuted += OnCommandExecuted;

            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), Provider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!(command.IsSpecified && !result.IsSuccess))
                return;
            
            var channel = context.Channel as SocketTextChannel;
            var reason = result.Error;

            await channel.SendMessageAsync($"The following error occured:\n{reason}");
            Logger.LogError(reason.ToString());
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage))
                return;
            
            var message = arg as SocketUserMessage;
            if (message.Source != MessageSource.User)
                return;

            var prefixPosition = 0;
            if (!message.HasStringPrefix(Configuration["prefix"], ref prefixPosition) && !message.HasMentionPrefix(Client.CurrentUser, ref prefixPosition)) 
                return;

            var context = new SocketCommandContext(Client, message);
            await CommandService.ExecuteAsync(context, prefixPosition, Provider);
        }
    }
}
