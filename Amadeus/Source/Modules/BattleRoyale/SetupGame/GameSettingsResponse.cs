namespace Amadeus.Modules.BattleRoyale.SetupGame;

internal sealed record GameSettingsResponse
{
    public required IReadOnlyCollection<string> PlayerNames { get; init; }
    public required ITextChannel TextChannel { get; init; }
}
