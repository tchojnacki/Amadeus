namespace Amadeus.Modules.BattleRoyale.SetupGame;

internal sealed record SetupGameRequest : IRequest<SetupGameResponse>
{
    public required string RawPlayersArgument { get; init; }
    public required ulong ChannelId { get; init; }
}
