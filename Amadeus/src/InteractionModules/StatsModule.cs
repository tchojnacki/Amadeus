using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using JetBrains.Annotations;

namespace Amadeus.InteractionModules;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class StatsModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly DiscordSocketClient _client;

    public StatsModule(DiscordSocketClient client) => _client = client;

    [SlashCommand("statystyki", "Sprawdź statystyki bota.")]
    public async Task ShowStatsAsync()
        => await RespondAsync( // TODO
            embed: new EmbedBuilder()
                .WithColor(Color.Green)
                .WithFooter(
                    new EmbedFooterBuilder()
                        .WithText("Amadeus")
                        .WithIconUrl(_client.CurrentUser.GetAvatarUrl()))
                .WithCurrentTimestamp()
                .WithTitle("Statystyki 📊")
                .AddField("Ping ⌛", $"{_client.Latency} ms", true)
                .AddField("Filmy 🎥", "0", true)
                .AddField("Cytaty 📜", "0", true)
                .AddField("Przyznany XP ⚔️", "0", true)
                .Build());
}
