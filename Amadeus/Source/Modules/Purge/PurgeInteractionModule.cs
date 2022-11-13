using Amadeus.Common.Services;
using Amadeus.Modules.Purge.DeleteMessages;
using Discord.Interactions;

namespace Amadeus.Modules.Purge;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class PurgeInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IMessageBuilderService _messageBuilder;
    private readonly IMediator _mediator;

    public PurgeInteractionModule(IMessageBuilderService messageBuilder, IMediator mediator)
    {
        _messageBuilder = messageBuilder;
        _mediator = mediator;
    }

    [EnabledInDm(false)]
    [DefaultMemberPermissions(GuildPermission.ManageMessages)]
    [SlashCommand("purge", "Remove recent messages.")]
    public async Task ExecutePurgeCommandAsync(
        [Summary("count", "Number of messages to delete")] int count
    )
    {
        var request = new DeleteMessagesRequest { Channel = Context.Channel, Count = count, };
        var response = await _mediator.Send(request);

        await RespondAsync(
            embed: response.Match(
                _ =>
                    _messageBuilder.ResponseTemplate
                        .WithTitle($"/{I18n.purge_name}")
                        .WithDescription(string.Format(I18n.Purge_DeletedCount, count))
                        .Build(),
                error =>
                    _messageBuilder.ErrorEmbed(
                        string.Format(I18n.Purge_IncorrectMessageCount, error.DeletionLimit)
                    ),
                _ => _messageBuilder.ErrorEmbed(I18n.Purge_InvalidChannelType)
            ),
            ephemeral: true
        );
    }
}
