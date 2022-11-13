using Amadeus.Modules.BattleRoyale.PlayGame;
using Discord.Interactions;
using Amadeus.Common.Services;
using Amadeus.Modules.BattleRoyale.SetupGame;

namespace Amadeus.Modules.BattleRoyale;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class BattleRoyaleInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private const string ThreadName = "battle-royale";

    private readonly IMediator _mediator;
    private readonly IMessageBuilderService _messageBuilder;

    public BattleRoyaleInteractionModule(IMediator mediator, IMessageBuilderService messageBuilder)
    {
        _mediator = mediator;
        _messageBuilder = messageBuilder;
    }

    [EnabledInDm(true)]
    [SlashCommand("br", "Play a game of Battle Royale.")]
    public async Task ExecuteBattleRoyaleCommandAsync(
        [Summary(name: "players", description: "Players separated by a space")] string rawPlayers
    )
    {
        var setupGameRequest = new SetupGameRequest
        {
            ChannelId = Context.Channel.Id,
            RawPlayersArgument = rawPlayers
        };
        var setupGameResponse = await _mediator.Send(setupGameRequest);

        if (!setupGameResponse.IsT0)
        {
            await RespondAsync(
                embed: _messageBuilder.ErrorEmbed(
                    setupGameResponse.Match(
                        default,
                        _ => I18n.BattleRoyale_NotEnoughPlayers,
                        _ => I18n.BattleRoyale_MustBeUsedInTextChannel
                    )
                ),
                ephemeral: true
            );
            return;
        }

        var gameSettings = setupGameResponse.AsT0!;

        await RespondAsync(
            embed: _messageBuilder.ResponseTemplate
                .WithTitle(I18n.BattleRoyale_Title)
                .WithDescription(I18n.BattleRoyale_StartingGame)
                .AddField(
                    I18n.BattleRoyale_Players,
                    string.Join(I18n.BattleRoyale_PlayerListConnector, gameSettings.PlayerNames)
                )
                .Build()
        );

        var message = await GetOriginalResponseAsync();

        var thread = await gameSettings.TextChannel.CreateThreadAsync(
            ThreadName,
            autoArchiveDuration: ThreadArchiveDuration.OneHour,
            message: message
        );

        var playGameRequest = new PlayGameRequest { PlayerNames = gameSettings.PlayerNames };
        var playGameResponse = _mediator.CreateStream(playGameRequest, CancellationToken.None);

        await foreach (var step in playGameResponse)
            await thread.SendMessageAsync(step.Text);
    }
}
