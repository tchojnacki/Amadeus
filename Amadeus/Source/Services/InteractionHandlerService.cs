using System.Reflection;
using Amadeus.Utils;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Amadeus.Services;

internal sealed class InteractionHandlerService : IInteractionHandlerService
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    private readonly ILogger<InteractionHandlerService> _logger;

    public InteractionHandlerService(DiscordSocketClient client, InteractionService interactionService, IServiceProvider services, IConfiguration configuration, ILogger<InteractionHandlerService> logger)
    {
        _client = client;
        _interactionService = interactionService;
        _services = services;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _interactionService.Log += DiscordLoggingAdapter.BuildAsyncLogger(_logger);

        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        _client.InteractionCreated += HandleInteraction;
    }

    private async Task ReadyAsync()
    {
        #if DEBUG
            await _interactionService.RegisterCommandsToGuildAsync(
                _configuration.GetValue<ulong>("Bot:MainGuild"));
        #else
            await _interactionService.RegisterCommandsGloballyAsync();
        #endif
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(_client, interaction);
            await _interactionService.ExecuteCommandAsync(context, _services);
        }
        catch (Exception exception)
        {
            _logger.LogError("Error occurred while handling interaction! {Exception}", exception);
        }
    }
}
