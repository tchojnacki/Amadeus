using Amadeus.Common.Services;
using Amadeus.Modules.Role.ModifyRole;
using Discord.Interactions;

namespace Amadeus.Modules.Role;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
[Group("role", "Manage your roles.")]
public sealed class RoleInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private Embed ResponseEmbed(
        ModifyRoleResponse modifyRoleResponse,
        IGuildUser member,
        IRole role
    ) =>
        modifyRoleResponse.Match(
            _ =>
                _messageBuilder.ResponseTemplate
                    .WithTitle(I18n.Role_MessageHeader)
                    .WithDescription(
                        string.Format(
                            I18n.Role_RoleGrantedSuccessfully,
                            member.Mention,
                            role.Mention
                        )
                    )
                    .Build(),
            _ =>
                _messageBuilder.ResponseTemplate
                    .WithTitle(I18n.Role_MessageHeader)
                    .WithDescription(
                        string.Format(
                            I18n.Role_RoleGrantedSuccessfully,
                            member.Mention,
                            role.Mention
                        )
                    )
                    .Build(),
            _ => _messageBuilder.ErrorEmbed(I18n.Role_DisallowedRole),
            _ => _messageBuilder.ErrorEmbed(I18n.Role_YouAlreadyHaveTheRole),
            _ => _messageBuilder.ErrorEmbed(I18n.Role_YouDontHaveTheRole)
        );

    private readonly IMessageBuilderService _messageBuilder;
    private readonly IMediator _mediator;

    public RoleInteractionModule(IMessageBuilderService messageBuilder, IMediator mediator)
    {
        _messageBuilder = messageBuilder;
        _mediator = mediator;
    }

    [EnabledInDm(false)]
    [SlashCommand("give", "Add a role to your account.")]
    public async Task ExecuteRoleGiveCommandAsync(
        [Summary("role", "The role which should be added")] IRole role
    )
    {
        var member = Context.Guild.GetUser(Context.User.Id);

        var request = new ModifyRoleRequest
        {
            Member = member,
            Role = role,
            ShouldBeOwned = true
        };
        var response = await _mediator.Send(request);

        await RespondAsync(embed: ResponseEmbed(response, member, role), ephemeral: true);
    }

    [EnabledInDm(false)]
    [SlashCommand("take", "Remove a role from your account.")]
    public async Task ExecuteRoleTakeCommandAsync(
        [Summary("role", "The role which should be removed")] IRole role
    )
    {
        var member = Context.Guild.GetUser(Context.User.Id);

        var request = new ModifyRoleRequest
        {
            Member = member,
            Role = role,
            ShouldBeOwned = false
        };
        var response = await _mediator.Send(request);

        await RespondAsync(embed: ResponseEmbed(response, member, role), ephemeral: true);
    }
}
