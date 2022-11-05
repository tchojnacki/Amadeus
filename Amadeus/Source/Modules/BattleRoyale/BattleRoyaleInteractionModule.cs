using Amadeus.Modules.BattleRoyale.PlayGame;
using Amadeus.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Text.RegularExpressions;

namespace Amadeus.Modules.BattleRoyale;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class BattleRoyaleInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private const string ThreadName = "battle-royale";
    private static readonly Regex PlayerNamePattern =
        new("(?:[^\\s\"]+|\"[^\"]*\")+", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly IMediator _mediator;
    private readonly IMessageBuilderService _messageBuilder;
    private readonly DiscordSocketClient _discordSocketClient;

    public BattleRoyaleInteractionModule(
        IMediator mediator,
        IMessageBuilderService messageBuilder,
        DiscordSocketClient discordSocketClient
    )
    {
        _mediator = mediator;
        _messageBuilder = messageBuilder;
        _discordSocketClient = discordSocketClient;
    }

    [EnabledInDm(true)]
    [SlashCommand("br", "Play a game of Battle Royale.")]
    public async Task ExecuteBattleRoyaleCommandAsync(
        [Summary(name: "players", description: "Players separated by a space")] string rawPlayers
    )
    {
        var playerNames = PlayerNamePattern
            .Matches(rawPlayers)
            .Select(p => p.Value.Trim('"'))
            .ToList();

        if (playerNames.Count < 2)
        {
            await RespondAsync(
                embed: _messageBuilder.ErrorEmbed(I18n.BattleRoyale_NotEnoughPlayers),
                ephemeral: true
            );
            return;
        }

        var channel = Context.Interaction.ChannelId is { } channelId
            ? (await _discordSocketClient.GetChannelAsync(channelId))
            : null;

        if (channel is not ITextChannel textChannel)
        {
            await RespondAsync(
                embed: _messageBuilder.ErrorEmbed(I18n.BattleRoyale_MustBeUsedInTextChannel),
                ephemeral: true
            );
            return;
        }

        var query = new PlayGameQuery { PlayerNames = playerNames };
        var response = _mediator.CreateStream(query, CancellationToken.None);

        await RespondAsync(
            embed: _messageBuilder.ResponseTemplate
                .WithTitle(I18n.BattleRoyale_Title)
                .WithDescription(I18n.BattleRoyale_StartingGame)
                .AddField(
                    I18n.BattleRoyale_Players,
                    string.Join(I18n.BattleRoyale_PlayerListConnector, playerNames)
                )
                .Build()
        );

        var message = await GetOriginalResponseAsync();

        var thread = await textChannel.CreateThreadAsync(
            ThreadName,
            autoArchiveDuration: ThreadArchiveDuration.OneHour,
            message: message
        );

        await foreach (var prompt in response)
        {
            await thread.SendMessageAsync(prompt);
        }
    }
}
