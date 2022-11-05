using Amadeus.Services;
using Amadeus.Utils;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace Amadeus;

internal sealed class Bot
{
    private readonly DiscordSocketClient _client;
    private readonly IInteractionHandlerService _interactionHandlerService;
    private readonly ILogger<Bot> _logger;

    public Bot(
        DiscordSocketClient client,
        IInteractionHandlerService interactionHandlerService,
        ILogger<Bot> logger
    )
    {
        _client = client;
        _interactionHandlerService = interactionHandlerService;
        _logger = logger;
    }

    public async Task RunAsync(string token)
    {
        _client.Log += DiscordLoggingAdapter.BuildAsyncLogger(_logger);

        await _interactionHandlerService.InitializeAsync();

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(Timeout.Infinite);
    }
}
