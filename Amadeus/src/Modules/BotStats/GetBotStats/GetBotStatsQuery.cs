using MediatR;

namespace Amadeus.Modules.BotStats.GetBotStats;

internal sealed record GetBotStatsQuery : IRequest<BotStatsDto>;
