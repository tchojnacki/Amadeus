namespace Amadeus.Modules.Statistics.GetBotStats;

internal sealed record BotStatsDto
{
    public required int LatencyMs { get; init; }
    public required int VideoCount { get; init; }
    public required int QuoteCount { get; init; }
    public required int GrantedExpSum { get; init; }
}
