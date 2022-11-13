using Discord.WebSocket;

namespace Amadeus.Modules.Statistics.GetBotStats;

[UsedImplicitly]
internal sealed class GetBotStatsHandler : IRequestHandler<GetBotStatsRequest, GetBotStatsResponse>
{
    private readonly DiscordSocketClient _client;

    public GetBotStatsHandler(DiscordSocketClient client) => _client = client;

    public Task<GetBotStatsResponse> Handle(
        GetBotStatsRequest request,
        CancellationToken cancellationToken
    ) =>
        Task.FromResult(
            new GetBotStatsResponse
            {
                LatencyMs = _client.Latency,
                VideoCount = 0,
                QuoteCount = 0,
                GrantedExpSum = 0
            }
        );
}
