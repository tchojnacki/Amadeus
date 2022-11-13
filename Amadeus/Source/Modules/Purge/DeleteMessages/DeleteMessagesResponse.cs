using OneOf;

namespace Amadeus.Modules.Purge.DeleteMessages;

public sealed record DeleteMessagesSuccessResponse
{
    public static readonly DeleteMessagesSuccessResponse Instance = new();

    private DeleteMessagesSuccessResponse() { }
}

public sealed record InvalidMessageCountError
{
    public required int DeletionLimit { get; init; }
}

public sealed record MustUseInTextChannelError
{
    public static readonly MustUseInTextChannelError Instance = new();

    private MustUseInTextChannelError() { }
}

[GenerateOneOf]
public partial class DeleteMessagesResponse
    : OneOfBase<
        DeleteMessagesSuccessResponse,
        InvalidMessageCountError,
        MustUseInTextChannelError
    > { }
