namespace Amadeus.Modules.BattleRoyale.PlayGame;

internal sealed record PlayGameQuery : IStreamRequest<string>
{
    public required IReadOnlyCollection<string> PlayerNames { get; init; }
}
