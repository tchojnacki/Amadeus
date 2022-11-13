using OneOf;

namespace Amadeus.Modules.Role.ModifyRole;

public sealed record GivenSuccessfully
{
    public static readonly GivenSuccessfully Instance = new();

    private GivenSuccessfully() { }
}

public sealed record TakenSuccessfully
{
    public static readonly TakenSuccessfully Instance = new();

    private TakenSuccessfully() { }
}

public sealed record DisallowedRoleError
{
    public static readonly DisallowedRoleError Instance = new();

    private DisallowedRoleError() { }
}

public sealed record AlreadyOwnedError
{
    public static readonly AlreadyOwnedError Instance = new();

    private AlreadyOwnedError() { }
}

public sealed record NotOwnedError
{
    public static readonly NotOwnedError Instance = new();

    private NotOwnedError() { }
}

[GenerateOneOf]
public sealed partial class ModifyRoleResponse
    : OneOfBase<
        GivenSuccessfully,
        TakenSuccessfully,
        DisallowedRoleError,
        AlreadyOwnedError,
        NotOwnedError
    > { }
