﻿using Amadeus.Modules.Statistics.GetBotStats;
using Amadeus.Resources;
using Amadeus.Services;
using Discord.Interactions;
using JetBrains.Annotations;
using MediatR;

namespace Amadeus.Modules.Statistics;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class StatisticsInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IMediator _mediator;
    private readonly IMessageBuilderService _messageBuilder;

    public StatisticsInteractionModule(IMediator mediator, IMessageBuilderService messageBuilder)
    {
        _mediator = mediator;
        _messageBuilder = messageBuilder;
    }

    [SlashCommand("statistics", "Show bot's statistics.")]
    public async Task ExecuteStatisticsCommandAsync()
    {
        var query = new GetBotStatsQuery();
        var response = await _mediator.Send(query);

        await RespondAsync(
            embed: _messageBuilder.ResponseTemplate
                .WithTitle(I18n.Statistics_StatsHeader)
                .AddField(I18n.Statistics_PingLabel, $"{response.LatencyMs} ms", true)
                .AddField(I18n.Statistics_VideoLabel, response.VideoCount, true)
                .AddField(I18n.Statistics_QuoteLabel, response.QuoteCount, true)
                .AddField(I18n.Statistics_GrantedExpLabel, response.GrantedExpSum, true)
                .Build());
    }
}