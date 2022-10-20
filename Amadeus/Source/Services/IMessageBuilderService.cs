using Discord;

namespace Amadeus.Services;

public interface IMessageBuilderService
{
    EmbedBuilder ResponseTemplate { get; }
}
