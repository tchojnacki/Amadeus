using OneOf;

namespace Amadeus.Modules.BattleRoyale.SetupGame;

public sealed record Success
{
    public required IReadOnlyCollection<string> PlayerNames { get; init; }
    public required ITextChannel TextChannel { get; init; }
}

public sealed record NotEnoughPlayersError
{
    public static readonly NotEnoughPlayersError Instance = new();

    private NotEnoughPlayersError() { }
}

public sealed record MustUseInTextChannelError
{
    public static readonly MustUseInTextChannelError Instance = new();

    private MustUseInTextChannelError() { }
}

[GenerateOneOf]
public sealed partial class SetupGameResponse
    : OneOfBase<Success, NotEnoughPlayersError, MustUseInTextChannelError> { }
