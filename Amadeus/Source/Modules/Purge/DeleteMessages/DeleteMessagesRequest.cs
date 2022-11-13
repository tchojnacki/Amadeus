using Discord.WebSocket;

namespace Amadeus.Modules.Purge.DeleteMessages;

internal sealed record DeleteMessagesRequest : IRequest<DeleteMessagesResponse>
{
    public required int Count { get; init; }
    public required ISocketMessageChannel Channel { get; init; }
}
