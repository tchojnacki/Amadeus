using System.Globalization;
using System.Reflection;
using Amadeus.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Amadeus.Utils;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmadeusServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var discordSocketConfig = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.None,
            AlwaysDownloadUsers = true,
        };

        var interactionServiceConfig = new InteractionServiceConfig
        {
            LocalizationManager = new ResxLocalizationManager(
                "Amadeus.Resources.I18n",
                assembly,
                CultureInfo.GetCultureInfo("pl"))
        };

        services
            .AddSingleton(configuration)
            .AddLogging(builder => builder.AddConsole())
            .AddMediatR(assembly)
            .AddSingleton(discordSocketConfig)
            .AddSingleton(interactionServiceConfig)
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton<InteractionService>()
            .AddSingleton<IMessageBuilderService, MessageBuilderService>()
            .AddSingleton<IInteractionHandlerService, InteractionHandlerService>()
            .AddSingleton<Bot>()
            .BuildServiceProvider();

        return services;
    }
}
