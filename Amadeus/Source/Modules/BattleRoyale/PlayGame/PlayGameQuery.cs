namespace Amadeus.Modules.BattleRoyale.PlayGame;

internal sealed record PlayGameQuery : IStreamRequest<GameStepResponse>
{
    public required IReadOnlyCollection<string> PlayerNames { get; init; }
}
