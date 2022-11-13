using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Amadeus.Common.Utils;

internal static class DiscordLoggingAdapter
{
    public static Func<LogMessage, Task> BuildAsyncLogger<T>(ILogger<T> logger) =>
        message =>
        {
            logger.Log(
                MapSeverity(message.Severity),
                "[{Source}] {Message}",
                message.Source,
                message.Message
            );
            return Task.CompletedTask;
        };

    private static LogLevel MapSeverity(LogSeverity discordSeverity) =>
        discordSeverity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Debug,
            LogSeverity.Debug => LogLevel.Trace,
            _ => throw new UnreachableException()
        };
}
