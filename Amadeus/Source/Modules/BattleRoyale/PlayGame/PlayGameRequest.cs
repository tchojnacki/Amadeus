namespace Amadeus.Modules.BattleRoyale.PlayGame;

internal sealed record PlayGameRequest : IStreamRequest<GameStepResponse>
{
    public required IReadOnlyCollection<string> PlayerNames { get; init; }
}
