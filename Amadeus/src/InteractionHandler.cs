using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Amadeus;

internal class InteractionHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _handler;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public InteractionHandler(DiscordSocketClient client, InteractionService handler, IServiceProvider services, IConfiguration configuration)
    {
        _client = client;
        _handler = handler;
        _services = services;
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        _client.Ready += ReadyAsync;
        _handler.Log += LogAsync;
        
        await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        
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
            await _handler.RegisterCommandsToGuildAsync(_configuration.GetValue<ulong>("Bot:MainGuild"));
        #else
            await _handler.RegisterCommandsGloballyAsync();
        #endif
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            var context = new SocketInteractionContext(_client, interaction);
            await _handler.ExecuteCommandAsync(context, _services);
        }
        catch
        {
            Console.WriteLine("Error occurred while handling interaction!");
        }
    }
}
