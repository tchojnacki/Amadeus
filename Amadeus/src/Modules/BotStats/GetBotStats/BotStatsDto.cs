namespace Amadeus.Modules.BotStats.GetBotStats;

internal sealed record BotStatsDto
{
    public int LatencyMs { get; init; }
    public int VideoCount { get; init; }
    public int QuoteCount { get; init; }
    public int GrantedExpSum { get; init; }
}
