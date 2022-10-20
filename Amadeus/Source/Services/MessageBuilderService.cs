using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Amadeus.Services;

internal sealed class MessageBuilderService : IMessageBuilderService
{
    private static readonly Color SuccessColor = Color.Green;

    private readonly DiscordSocketClient _client;
    private readonly IConfiguration _configuration;

    public MessageBuilderService(DiscordSocketClient client, IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    public EmbedBuilder ResponseTemplate
        => new EmbedBuilder()
            .WithColor(SuccessColor)
            .WithFooter(
                new EmbedFooterBuilder()
                    .WithText(_configuration.GetValue<string>("Bot:Name"))
                    .WithIconUrl(_client.CurrentUser.GetAvatarUrl())
            )
            .WithCurrentTimestamp();
}
