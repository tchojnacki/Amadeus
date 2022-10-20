using System.Globalization;
using System.Reflection;
using Amadeus.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amadeus.Utils;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmadeusServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.None,
            AlwaysDownloadUsers = true,
        });

        var interactionService = new InteractionService(client, new InteractionServiceConfig
        {
            LocalizationManager = new ResxLocalizationManager(
                "Amadeus.Resources.I18n",
                assembly,
                CultureInfo.GetCultureInfo("pl"))
        });

        services
            .AddSingleton(configuration)
            .AddMediatR(assembly)
            .AddSingleton(client)
            .AddSingleton(interactionService)
            .AddSingleton<IMessageBuilderService, MessageBuilderService>()
            .AddSingleton<IInteractionHandlerService, InteractionHandlerService>()
            .AddSingleton<Bot>()
            .BuildServiceProvider();

        return services;
    }
}
