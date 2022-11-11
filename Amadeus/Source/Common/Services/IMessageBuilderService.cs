namespace Amadeus.Common.Services;

public interface IMessageBuilderService
{
    EmbedBuilder ResponseTemplate { get; }
    Embed ErrorEmbed(string message);
}
