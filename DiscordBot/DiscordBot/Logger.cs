using Discord;
using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    internal class Logger
    {
        public Logger() { }

        public Task Log(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString());
            return Task.CompletedTask;
        }

        public static async Task<Logger> CreateAsync()
        {
            return await Task.Run(() => new Logger());
        }
    }
}
