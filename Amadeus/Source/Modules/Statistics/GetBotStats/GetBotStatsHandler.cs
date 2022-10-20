using Discord.WebSocket;
using JetBrains.Annotations;
using MediatR;

namespace Amadeus.Modules.Statistics.GetBotStats;

[UsedImplicitly]
internal sealed class GetBotStatsHandler : IRequestHandler<GetBotStatsQuery, BotStatsDto>
{
    private readonly DiscordSocketClient _client;

    public GetBotStatsHandler(DiscordSocketClient client) => _client = client;

    public Task<BotStatsDto> Handle(GetBotStatsQuery query, CancellationToken cancellationToken)
    {
        var dto = new BotStatsDto
        {
            LatencyMs = _client.Latency,
            VideoCount = 0,
            QuoteCount = 0,
            GrantedExpSum = 0
        };

        return Task.FromResult(dto);
    }
}
