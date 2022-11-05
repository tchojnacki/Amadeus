using Discord;
using Discord.WebSocket;

namespace Amadeus.Services;

internal sealed class MessageBuilderService : IMessageBuilderService
{
    private static readonly Color SuccessColor = Color.Green;
    private static readonly Color ErrorColor = Color.Red;

    private readonly DiscordSocketClient _client;

    public MessageBuilderService(DiscordSocketClient client) => _client = client;

    private EmbedFooterBuilder FooterBuilder =>
        new EmbedFooterBuilder()
            .WithText(_client.CurrentUser.Username)
            .WithIconUrl(_client.CurrentUser.GetAvatarUrl());

    public EmbedBuilder ResponseTemplate =>
        new EmbedBuilder().WithColor(SuccessColor).WithFooter(FooterBuilder).WithCurrentTimestamp();

    public Embed ErrorEmbed(string message) =>
        new EmbedBuilder()
            .WithColor(ErrorColor)
            .WithTitle("Error")
            .WithDescription(message)
            .WithFooter(FooterBuilder)
            .WithCurrentTimestamp()
            .Build();
}
