namespace Amadeus.Modules.Statistics.GetBotStats;

internal sealed record GetBotStatsRequest : IRequest<BotStatsResponse>
{
    public static readonly GetBotStatsRequest Instance = new();

    private GetBotStatsRequest() { }
}
