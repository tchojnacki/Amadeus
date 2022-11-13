using Microsoft.Extensions.Configuration;
using OneOf;

namespace Amadeus.Modules.Role.ModifyRole;

[UsedImplicitly]
internal sealed class ModifyRoleHandler
    : IRequestHandler<ModifyRoleRequest, OneOf<ModifyRoleSuccessResponse, ModifyRoleErrorResponse>>
{
    private readonly IReadOnlyCollection<ulong> _allowedRoles;

    public ModifyRoleHandler(IConfiguration configuration) =>
        _allowedRoles =
            configuration.GetSection("Modules:Role:AllowedRoles").Get<ulong[]>()
            ?? Array.Empty<ulong>();

    public async Task<OneOf<ModifyRoleSuccessResponse, ModifyRoleErrorResponse>> Handle(
        ModifyRoleRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!_allowedRoles.Contains(request.Role.Id))
            return new ModifyRoleErrorResponse { Message = I18n.Role_DisallowedRole };

        if (request.Member.Roles.Contains(request.Role) == request.ShouldBeOwned)
            return new ModifyRoleErrorResponse
            {
                Message = request.ShouldBeOwned
                    ? I18n.Role_YouAlreadyHaveTheRole
                    : I18n.Role_YouDontHaveTheRole
            };

        if (request.ShouldBeOwned)
            await request.Member.AddRoleAsync(request.Role);
        else
            await request.Member.RemoveRoleAsync(request.Role);

        return ModifyRoleSuccessResponse.Instance;
    }
}
