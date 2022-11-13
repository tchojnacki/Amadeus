using OneOf;

namespace Amadeus.Modules.BattleRoyale.SetupGame;

internal sealed record SetupGameRequest
    : IRequest<OneOf<GameSettingsResponse, GameSetupErrorResponse>>
{
    public required string RawPlayersArgument { get; init; }
    public required ulong ChannelId { get; init; }
}
