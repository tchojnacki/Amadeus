namespace Amadeus.Modules.BattleRoyale.SetupGame;

internal sealed record SetupGameSuccessResponse
{
    public required IReadOnlyCollection<string> PlayerNames { get; init; }
    public required ITextChannel TextChannel { get; init; }
}
