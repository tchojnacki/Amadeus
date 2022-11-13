namespace Amadeus.Modules.BattleRoyale.SetupGame;

internal sealed record SetupGameErrorResponse
{
    public required string Message { get; init; }
}
