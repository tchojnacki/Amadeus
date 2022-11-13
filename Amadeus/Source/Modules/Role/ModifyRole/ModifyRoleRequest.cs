using Discord.WebSocket;

namespace Amadeus.Modules.Role.ModifyRole;

internal sealed record ModifyRoleRequest : IRequest<ModifyRoleResponse>
{
    public required SocketGuildUser Member { get; init; }
    public required IRole Role { get; init; }
    public required bool ShouldBeOwned { get; init; }
}
