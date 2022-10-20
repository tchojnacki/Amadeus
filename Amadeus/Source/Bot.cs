using Amadeus.Services;
using Discord;
using Discord.WebSocket;

namespace Amadeus;

internal sealed class Bot
{
    private readonly DiscordSocketClient _client;
    private readonly IInteractionHandlerService _interactionHandlerService;

    public Bot(DiscordSocketClient client, IInteractionHandlerService interactionHandlerService)
    {
        _client = client;
        _interactionHandlerService = interactionHandlerService;
    }

    public async Task RunAsync(string token)
    {
        _client.Log += LogAsync;
        
        await _interactionHandlerService.InitializeAsync();
        
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        
        await Task.Delay(Timeout.Infinite);
    }

    private static Task LogAsync(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}
