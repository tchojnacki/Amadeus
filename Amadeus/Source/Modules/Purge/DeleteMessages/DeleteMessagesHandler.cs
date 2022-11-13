using Microsoft.Extensions.Configuration;

namespace Amadeus.Modules.Purge.DeleteMessages;

[UsedImplicitly]
internal sealed class DeleteMessagesHandler
    : IRequestHandler<DeleteMessagesRequest, DeleteMessagesResponse>
{
    private readonly int _deletionLimit;

    public DeleteMessagesHandler(IConfiguration configuration) =>
        _deletionLimit = configuration.GetValue<int>("Modules:Purge:MaxMessagesPerPurge");

    public async Task<DeleteMessagesResponse> Handle(
        DeleteMessagesRequest request,
        CancellationToken cancellationToken
    )
    {
        if (request.Count < 1 || request.Count > _deletionLimit)
            return new InvalidMessageCountError { DeletionLimit = _deletionLimit };

        if (request.Channel is not ITextChannel channel)
            return MustUseInTextChannelError.Instance;

        var messages = await channel.GetMessagesAsync(request.Count).FlattenAsync();
        await channel.DeleteMessagesAsync(messages);

        return DeleteMessagesSuccessResponse.Instance;
    }
}
