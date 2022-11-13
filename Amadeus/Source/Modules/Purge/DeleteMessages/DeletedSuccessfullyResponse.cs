namespace Amadeus.Modules.Purge.DeleteMessages;

internal sealed record DeletedSuccessfullyResponse
{
    public static readonly DeletedSuccessfullyResponse Instance = new();

    private DeletedSuccessfullyResponse() { }
}
