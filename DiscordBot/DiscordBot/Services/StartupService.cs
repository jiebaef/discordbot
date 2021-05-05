using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    internal class StartupService
    {
        public static IServiceProvider Provider;
        private readonly DiscordSocketClient Client;
        private readonly CommandService Commands;
        private readonly IConfigurationRoot Configuration;
        private readonly Logger Logger;

        public StartupService(IServiceProvider provider, DiscordSocketClient client, CommandService commands, IConfigurationRoot configuration, Logger logger)
        {
            Provider = provider;
            Client = client;
            Commands = commands;
            Configuration = configuration;
            Logger = logger;
        }

        public async Task StartAsync()
        {
            var token = Configuration["tokens:discord"];
            Client.Log += Logger.Log;
            
            if(string.IsNullOrWhiteSpace(token))
            {
                await Logger.Log(new LogMessage(LogSeverity.Error, nameof(StartupService), "Please provide your Discord token in 'config.yml'."));
                return;
            }
            
            await Client.LoginAsync(TokenType.Bot, token);
            await Client.StartAsync();

            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Provider);
        }
    }
}
