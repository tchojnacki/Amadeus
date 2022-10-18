﻿using Amadeus.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amadeus.Utils;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAmadeusServices(this IServiceCollection services, IConfiguration configuration)
    {
        var client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.None,
            AlwaysDownloadUsers = true,
        });

        services
            .AddSingleton(client)
            .AddSingleton(configuration)
            .AddSingleton<InteractionService>()
            .AddSingleton<InteractionHandlerService>()
            .AddSingleton<Bot>()
            .BuildServiceProvider();

        return services;
    }
}
