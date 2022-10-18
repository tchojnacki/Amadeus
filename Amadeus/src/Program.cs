using Discord.WebSocket;
using Discord;
using Discord.Interactions;
using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amadeus;

internal class Program
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _services;

    private readonly DiscordSocketConfig _socketConfig = new()
    {
        GatewayIntents = GatewayIntents.None,
        AlwaysDownloadUsers = true,
    };

    public Program()
    {
        _configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "AMADEUS_")
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        _services = new ServiceCollection()
            .AddSingleton(_configuration)
            .AddSingleton(_socketConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .BuildServiceProvider();
    }

    public static void Main(string[] args)
    {
        DotEnv.Fluent().WithProbeForEnv().Load();

        new Program().RunAsync()
            .GetAwaiter()
            .GetResult();
    }

    public async Task RunAsync()
    {
        var client = _services.GetRequiredService<DiscordSocketClient>();

        client.Log += LogAsync;
        
        await _services
            .GetRequiredService<InteractionHandler>()
            .InitializeAsync();
        
        await client.LoginAsync(TokenType.Bot, _configuration.GetValue<string>("DiscordToken"));
        await client.StartAsync();
        
        await Task.Delay(Timeout.Infinite);
    }

    private static Task LogAsync(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}
