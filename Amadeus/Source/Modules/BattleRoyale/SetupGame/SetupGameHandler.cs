using System.Text.RegularExpressions;
using Discord.WebSocket;

namespace Amadeus.Modules.BattleRoyale.SetupGame;

[UsedImplicitly]
internal sealed class SetupGameHandler : IRequestHandler<SetupGameRequest, SetupGameResponse>
{
    private static readonly Regex PlayerNamePattern =
        new("(?:[^\\s\"]+|\"[^\"]*\")+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly DiscordSocketClient _discordSocketClient;

    public SetupGameHandler(DiscordSocketClient discordSocketClient) =>
        _discordSocketClient = discordSocketClient;

    public async Task<SetupGameResponse> Handle(
        SetupGameRequest request,
        CancellationToken cancellationToken
    )
    {
        var playerNames = PlayerNamePattern
            .Matches(request.RawPlayersArgument)
            .Select(p => p.Value.Trim('"'))
            .ToList();

        if (playerNames.Count < 2)
            return NotEnoughPlayersError.Instance;

        var channel = await _discordSocketClient.GetChannelAsync(request.ChannelId);

        if (channel is not ITextChannel textChannel)
            return MustUseInTextChannelError.Instance;

        return new Success { PlayerNames = playerNames, TextChannel = textChannel };
    }
}
