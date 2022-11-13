using System.Text.RegularExpressions;
using Discord.WebSocket;
using OneOf;

namespace Amadeus.Modules.BattleRoyale.SetupGame;

[UsedImplicitly]
internal sealed class SetupGameHandler
    : IRequestHandler<SetupGameRequest, OneOf<SetupGameSuccessResponse, SetupGameErrorResponse>>
{
    private static readonly Regex PlayerNamePattern =
        new("(?:[^\\s\"]+|\"[^\"]*\")+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly DiscordSocketClient _discordSocketClient;

    public SetupGameHandler(DiscordSocketClient discordSocketClient) =>
        _discordSocketClient = discordSocketClient;

    public async Task<OneOf<SetupGameSuccessResponse, SetupGameErrorResponse>> Handle(
        SetupGameRequest request,
        CancellationToken cancellationToken
    )
    {
        var playerNames = PlayerNamePattern
            .Matches(request.RawPlayersArgument)
            .Select(p => p.Value.Trim('"'))
            .ToList();

        if (playerNames.Count < 2)
        {
            return new SetupGameErrorResponse { Message = I18n.BattleRoyale_NotEnoughPlayers };
        }

        var channel = await _discordSocketClient.GetChannelAsync(request.ChannelId);

        if (channel is not ITextChannel textChannel)
        {
            return new SetupGameErrorResponse
            {
                Message = I18n.BattleRoyale_MustBeUsedInTextChannel
            };
        }

        return new SetupGameSuccessResponse
        {
            PlayerNames = playerNames,
            TextChannel = textChannel
        };
    }
}
