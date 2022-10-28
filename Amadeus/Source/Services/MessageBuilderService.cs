using Discord;
using Discord.WebSocket;

namespace Amadeus.Services;

internal sealed class MessageBuilderService : IMessageBuilderService
{
    private static readonly Color SuccessColor = Color.Green;

    private readonly DiscordSocketClient _client;

    public MessageBuilderService(DiscordSocketClient client) => _client = client;

    public EmbedBuilder ResponseTemplate
        => new EmbedBuilder()
            .WithColor(SuccessColor)
            .WithFooter(
                new EmbedFooterBuilder()
                    .WithText(_client.CurrentUser.Username)
                    .WithIconUrl(_client.CurrentUser.GetAvatarUrl())
            )
            .WithCurrentTimestamp();
}
