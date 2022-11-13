namespace Amadeus.Modules.Purge.DeleteMessages;

internal sealed record DeleteMessagesErrorResponse
{
    public required string Message { get; init; }
}
