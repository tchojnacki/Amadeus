using Amadeus.Common.Services;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;

namespace Amadeus.Modules.Purge;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public sealed class PurgeInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IMessageBuilderService _messageBuilder;
    private readonly IConfiguration _configuration;

    public PurgeInteractionModule(
        IMessageBuilderService messageBuilder,
        IConfiguration configuration
    )
    {
        _messageBuilder = messageBuilder;
        _configuration = configuration;
    }

    [EnabledInDm(false)]
    [DefaultMemberPermissions(GuildPermission.ManageMessages)]
    [SlashCommand("purge", "Remove recent messages.")]
    public async Task ExecutePurgeCommandAsync(
        [Summary("count", "Number of messages to delete")] int count
    )
    {
        var deletionLimit = _configuration.GetValue<int>("Modules:Purge:MaxMessagesPerPurge");
        if (count < 1 || count > deletionLimit)
        {
            await RespondAsync(
                embed: _messageBuilder.ErrorEmbed(
                    string.Format(I18n.Purge_IncorrectMessageCount, deletionLimit)
                ),
                ephemeral: true
            );
            return;
        }

        if (Context.Channel is not ITextChannel channel)
        {
            await RespondAsync(
                embed: _messageBuilder.ErrorEmbed(I18n.Purge_InvalidChannelType),
                ephemeral: true
            );
            return;
        }

        var messages = await channel.GetMessagesAsync(count).FlattenAsync();
        await channel.DeleteMessagesAsync(messages);

        await RespondAsync(
            embed: _messageBuilder.ResponseTemplate
                .WithTitle($"/{I18n.purge_name}")
                .WithDescription(string.Format(I18n.Purge_DeletedCount, count))
                .Build(),
            ephemeral: true
        );
    }
}
