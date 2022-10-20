using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Amadeus.Services;

internal sealed class InteractionHandlerService : IInteractionHandlerService
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public InteractionHandlerService(DiscordSocketClient client, InteractionService interactionService, IServiceProvider services, IConfiguration configuration)
    {
        _client = client;
        _interactionService = interactionService;
        _services = services;
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _interactionService.Log += LogAsync;

        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        _client.InteractionCreated += HandleInteraction;
    }

    private static Task LogAsync(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
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
        catch
        {
            Console.WriteLine("Error occurred while handling interaction!");
        }
    }
}
