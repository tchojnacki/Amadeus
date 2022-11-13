using Microsoft.Extensions.Configuration;
using OneOf;

namespace Amadeus.Modules.Purge.DeleteMessages;

[UsedImplicitly]
internal sealed class DeleteMessagesHandler
    : IRequestHandler<
        DeleteMessagesRequest,
        OneOf<DeletedSuccessfullyResponse, DeleteErrorResponse>
    >
{
    private readonly int _deletionLimit;

    public DeleteMessagesHandler(IConfiguration configuration) =>
        _deletionLimit = configuration.GetValue<int>("Modules:Purge:MaxMessagesPerPurge");

    public async Task<OneOf<DeletedSuccessfullyResponse, DeleteErrorResponse>> Handle(
        DeleteMessagesRequest request,
        CancellationToken cancellationToken
    )
    {
        if (request.Count < 1 || request.Count > _deletionLimit)
            return new DeleteErrorResponse
            {
                Message = string.Format(I18n.Purge_IncorrectMessageCount, _deletionLimit)
            };

        if (request.Channel is not ITextChannel channel)
            return new DeleteErrorResponse { Message = I18n.Purge_InvalidChannelType };

        var messages = await channel.GetMessagesAsync(request.Count).FlattenAsync();
        await channel.DeleteMessagesAsync(messages);

        return DeletedSuccessfullyResponse.Instance;
    }
}
