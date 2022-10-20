using Amadeus.Modules.BotStats.GetBotStats;
using Amadeus.Services;
using Discord.Interactions;
using JetBrains.Annotations;
using MediatR;

namespace Amadeus.Modules.BotStats;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class BotStatsInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IMediator _mediator;
    private readonly IMessageBuilderService _messageBuilder;

    public BotStatsInteractionModule(IMediator mediator, IMessageBuilderService messageBuilder)
    {
        _mediator = mediator;
        _messageBuilder = messageBuilder;
    }

    [SlashCommand("statystyki", "Sprawdź statystyki bota.")]
    public async Task ShowStatsAsync()
    {
        var query = new GetBotStatsQuery();
        var response = await _mediator.Send(query);

        await RespondAsync(
            embed: _messageBuilder.ResponseTemplate
                .WithTitle("Statystyki 📊")
                .AddField("Ping ⌛", $"{response.LatencyMs} ms", true)
                .AddField("Filmy 🎥", response.VideoCount, true)
                .AddField("Cytaty 📜", response.QuoteCount, true)
                .AddField("Przyznany XP ⚔️", response.GrantedExpSum, true)
                .Build());
    }
}
