using MediatR;

namespace Amadeus.Modules.Statistics.GetBotStats;

internal sealed record GetBotStatsQuery : IRequest<BotStatsDto>;
