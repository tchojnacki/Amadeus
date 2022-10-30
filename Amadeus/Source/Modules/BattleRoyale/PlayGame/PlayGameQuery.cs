namespace Amadeus.Modules.BattleRoyale.PlayGame;

internal sealed record PlayGameQuery : IStreamRequest<string>
{
    public IReadOnlyCollection<string> PlayerNames { get; init; } = default!;
}
