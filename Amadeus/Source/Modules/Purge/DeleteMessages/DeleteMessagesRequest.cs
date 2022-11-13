using Discord.WebSocket;
using OneOf;

namespace Amadeus.Modules.Purge.DeleteMessages;

internal sealed record DeleteMessagesRequest
    : IRequest<OneOf<DeleteMessagesSuccessResponse, DeleteMessagesErrorResponse>>
{
    public required int Count { get; init; }
    public required ISocketMessageChannel Channel { get; init; }
}
