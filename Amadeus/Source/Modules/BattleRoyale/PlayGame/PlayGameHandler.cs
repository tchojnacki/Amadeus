using System.Runtime.CompilerServices;
using Amadeus.Resources;
using JetBrains.Annotations;
using MediatR;

namespace Amadeus.Modules.BattleRoyale.PlayGame;

[UsedImplicitly]
internal sealed class PlayGameHandler : IStreamRequestHandler<PlayGameQuery, string>
{
    private const double TurnKillChance = 0.75;
    private static readonly Func<int, double> ParticipantCountWeight = c => Math.Pow(c, -2);

    private static readonly Random Random = new();

    private static TimeSpan RandomDelayDuration()
        => TimeSpan.FromMilliseconds(Random.NextDouble() * 1000);

    private sealed class KillAction
    {
        public IReadOnlyCollection<int> KillerIndices { get; init; } = default!;
        public IReadOnlyCollection<int> VictimIndices { get; init; } = default!;
    }

    private static int RollParticipantCount(int maxCountInclusive)
    {
        var items = Enumerable
            .Range(1, maxCountInclusive)
            .Select(c => new { Count = c, Weight = ParticipantCountWeight(c) })
            .ToList();

        var weightSum = items.Sum(i => i.Weight);

        var roll = Random.NextDouble() * weightSum;

        foreach (var item in items)
        {
            if (roll < item.Weight) return item.Count;
            roll -= item.Weight;
        }

        throw new InvalidOperationException();
    }

    private static KillAction RandomKillAction(int playerCount)
    {
        var players = Enumerable.Range(0, playerCount).ToList();

        var victimCount = RollParticipantCount(playerCount - 1);
        var killerCount = RollParticipantCount(playerCount - victimCount);

        var playerSample = players
            .OrderBy(_ => Random.Next())
            .Take(killerCount + victimCount)
            .ToList();

        return new KillAction
        {
            KillerIndices = playerSample.Take(killerCount).ToList(),
            VictimIndices = playerSample.Skip(killerCount).ToList()
        };
    }

    private static string FormatPlayerGroup(IEnumerable<string> players)
    {
        var list = players.ToList();
        if (list.Count == 1) return list.Single();
        return string.Join(I18n.BattleRoyale_PlayerListConnector, list.Take(list.Count - 1)) +
               I18n.BattleRoyale_PlayerListLastConnector + list.Last();
    }

    public async IAsyncEnumerable<string> Handle(PlayGameQuery request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var players = request.PlayerNames.ToList();

        while (players.Count > 1)
        {
            if (Random.NextDouble() < TurnKillChance)
            {
                var killAction = RandomKillAction(players.Count);
                var killers = killAction.KillerIndices.Select(i => players[i]);
                var victims = killAction.VictimIndices.Select(i => players[i]);

                yield return string.Format(
                    I18n.BattleRoyale_Eliminates,
                    FormatPlayerGroup(killers),
                    FormatPlayerGroup(victims));

                foreach (var victimIndex in killAction.VictimIndices.OrderByDescending(i => i))
                    players.RemoveAt(victimIndex);
            }
            else
            {
                yield return I18n.BattleRoyale_NothingHappens;
            }

            await Task.Delay(RandomDelayDuration(), cancellationToken);
        }

        yield return string.Format(I18n.BattleRoyale_Wins, players.Single());
    }
}
