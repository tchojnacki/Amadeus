using Amadeus.Common;
using Amadeus.Common.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = ConfigUtils.LoadConfiguration();

var services = new ServiceCollection().AddAmadeusServices(configuration).BuildServiceProvider();

var bot = services.GetRequiredService<Bot>();
await bot.RunAsync(configuration.GetValue<string>("DiscordToken"));
