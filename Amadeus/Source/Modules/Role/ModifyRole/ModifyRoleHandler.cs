using Microsoft.Extensions.Configuration;

namespace Amadeus.Modules.Role.ModifyRole;

[UsedImplicitly]
internal sealed class ModifyRoleHandler : IRequestHandler<ModifyRoleRequest, ModifyRoleResponse>
{
    private readonly IReadOnlyCollection<ulong> _allowedRoles;

    public ModifyRoleHandler(IConfiguration configuration) =>
        _allowedRoles =
            configuration.GetSection("Modules:Role:AllowedRoles").Get<ulong[]>()
            ?? Array.Empty<ulong>();

    public async Task<ModifyRoleResponse> Handle(
        ModifyRoleRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!_allowedRoles.Contains(request.Role.Id))
            return DisallowedRoleError.Instance;

        if (request.Member.Roles.Contains(request.Role) == request.ShouldBeOwned)
            return request.ShouldBeOwned ? AlreadyOwnedError.Instance : NotOwnedError.Instance;

        switch (request.ShouldBeOwned)
        {
            case true:
                await request.Member.AddRoleAsync(request.Role);
                return GivenSuccessfully.Instance;
            default:
                await request.Member.RemoveRoleAsync(request.Role);
                return TakenSuccessfully.Instance;
        }
    }
}
