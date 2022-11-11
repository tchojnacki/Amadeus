using Amadeus.Services;
using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;

namespace Amadeus.Modules.Role;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
[Group("role", "Manage your roles.")]
public sealed class RoleInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IReadOnlyCollection<ulong> _allowedRoles;
    private readonly IMessageBuilderService _messageBuilder;

    public RoleInteractionModule(
        IConfiguration configuration,
        IMessageBuilderService messageBuilder
    )
    {
        _messageBuilder = messageBuilder;
        _allowedRoles =
            configuration.GetSection("Modules:Role:AllowedRoles").Get<ulong[]>()
            ?? Array.Empty<ulong>();
    }

    private async Task<bool> CheckRoleAsync(ulong roleId)
    {
        if (_allowedRoles.Contains(roleId))
            return true;

        await RespondAsync(
            embed: _messageBuilder.ErrorEmbed(I18n.Role_DisallowedRole),
            ephemeral: true
        );
        return true;
    }

    [EnabledInDm(false)]
    [SlashCommand("give", "Add a role to your account.")]
    public async Task ExecuteRoleGiveCommandAsync(
        [Summary("role", "The role which should be added")] IRole role
    )
    {
        if (!await CheckRoleAsync(role.Id))
            return;

        var guildUser = Context.Guild.GetUser(Context.User.Id);

        if (guildUser.Roles.Contains(role))
        {
            await RespondAsync(
                embed: _messageBuilder.ErrorEmbed(I18n.Role_YouAlreadyHaveTheRole),
                ephemeral: true
            );
            return;
        }

        await guildUser.AddRoleAsync(role);

        await RespondAsync(
            embed: _messageBuilder.ResponseTemplate
                .WithTitle(I18n.Role_MessageHeader)
                .WithDescription(
                    string.Format(
                        I18n.Role_RoleGrantedSuccessfully,
                        guildUser.Mention,
                        role.Mention
                    )
                )
                .Build(),
            ephemeral: true
        );
    }

    [EnabledInDm(false)]
    [SlashCommand("take", "Remove a role from your account.")]
    public async Task ExecuteRoleTakeCommandAsync(
        [Summary("role", "The role which should be removed")] IRole role
    )
    {
        if (!await CheckRoleAsync(role.Id))
            return;

        var guildUser = Context.Guild.GetUser(Context.User.Id);

        if (!guildUser.Roles.Contains(role))
        {
            await RespondAsync(
                embed: _messageBuilder.ErrorEmbed(I18n.Role_YouDontHaveTheRole),
                ephemeral: true
            );
            return;
        }

        await guildUser.RemoveRoleAsync(role);

        await RespondAsync(
            embed: _messageBuilder.ResponseTemplate
                .WithTitle(I18n.Role_MessageHeader)
                .WithDescription(
                    string.Format(I18n.Role_RoleTakenSuccessfully, guildUser.Mention, role.Mention)
                )
                .Build(),
            ephemeral: true
        );
    }
}
