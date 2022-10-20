using dotenv.net;
using Microsoft.Extensions.Configuration;

namespace Amadeus.Utils;

internal static class ConfigUtils
{
    private const string EnvVariablePrefix = "AMADEUS_";

    public static IConfiguration LoadConfiguration()
    {
        DotEnv.Fluent().WithProbeForEnv().Load();

        return new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: EnvVariablePrefix)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
    }
}
