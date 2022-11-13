using Discord.WebSocket;
using OneOf;

namespace Amadeus.Modules.Role.ModifyRole;

internal sealed record ModifyRoleRequest
    : IRequest<OneOf<ModifyRoleSuccessResponse, ModifyRoleErrorResponse>>
{
    public required SocketGuildUser Member { get; init; }
    public required IRole Role { get; init; }
    public required bool ShouldBeOwned { get; init; }
}
