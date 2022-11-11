using OneOf;

namespace Amadeus.Modules.BattleRoyale.SetupGame;

internal sealed class SetupGameQuery : IRequest<OneOf<GameSettingsResponse, GameSetupErrorResponse>>
{
    public required string RawPlayersArgument { get; init; }
    public required ulong ChannelId { get; init; }
}
