using System.Threading.Tasks;

namespace DiscordBot
{
    internal class Program
    {
        public static async Task Main(string[] args) => await Startup.RunAsync(args);
    }
}
