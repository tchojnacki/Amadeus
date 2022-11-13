namespace Amadeus.Modules.Statistics.GetBotStats;

internal sealed record GetBotStatsRequest : IRequest<GetBotStatsResponse>
{
    public static readonly GetBotStatsRequest Instance = new();

    private GetBotStatsRequest() { }
}
