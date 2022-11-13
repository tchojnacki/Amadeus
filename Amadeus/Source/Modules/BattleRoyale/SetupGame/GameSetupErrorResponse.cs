namespace Amadeus.Modules.BattleRoyale.SetupGame;

internal sealed record GameSetupErrorResponse
{
    public required string Message { get; init; }
}
