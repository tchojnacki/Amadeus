namespace Amadeus.Modules.Purge.DeleteMessages;

internal sealed record DeleteErrorResponse
{
    public required string Message { get; init; }
}
