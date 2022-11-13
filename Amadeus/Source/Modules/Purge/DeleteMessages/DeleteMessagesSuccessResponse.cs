namespace Amadeus.Modules.Purge.DeleteMessages;

internal sealed record DeleteMessagesSuccessResponse
{
    public static readonly DeleteMessagesSuccessResponse Instance = new();

    private DeleteMessagesSuccessResponse() { }
}
