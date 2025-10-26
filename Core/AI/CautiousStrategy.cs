using System.Collections.Generic;
using System.Linq;
using Zonk.Showcase.Scoring;
using Zonk.Showcase.TurnEngine;

namespace Zonk.Showcase.AI
{
    /// <summary>
    /// Cautious strategy:
    /// - greedy selection of non-overlapping combos by descending value.
    /// - banks when turn score is big enough or dice-left is small (<= 2).
    /// </summary>
    public sealed class CautiousStrategy : BaseAI
    {
        public override string Name => "Cautious";
        private readonly ScoringService _s = new ScoringService();

        public override Decision Decide(GameState state, int[] roll)
        {
            var combos = CombinationDetector.DetectAll(new DiceValues(roll))
                                            .OrderByDescending(c => c.Value)
                                            .ToList();

            var used = new Dictionary<int, int>();
            var picks = new List<ComboInfo>();

            bool CanTake(ComboInfo ci)
            {
                var need = ci.Dice.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
                foreach (var kv in need)
                {
                    int face = kv.Key, needCount = kv.Value;
                    int have = roll.Count(v => v == face);
                    int usedCount = used.TryGetValue(face, out var u) ? u : 0;
                    if (have - usedCount < needCount) return false;
                }
                return true;
            }

            void Take(ComboInfo ci)
            {
                foreach (var kv in ci.Dice.GroupBy(v => v))
                    used[kv.Key] = (used.TryGetValue(kv.Key, out var u) ? u : 0) + kv.Count();
                picks.Add(ci);
            }

            foreach (var ci in combos)
                if (CanTake(ci)) Take(ci);

            var keep = picks.SelectMany(p => p.Dice).ToArray();
            if (keep.Length == 0) return new Decision(System.Array.Empty<int>(), false);

            int gained = _s.Score(keep);
            int turnAfter = state.CurrentTurnScore + gained;
            int diceLeftAfter = (keep.Length == state.DiceLeft) ? 6 : (state.DiceLeft - keep.Length);

            bool bank = turnAfter >= 700 || diceLeftAfter <= 2;
            return new Decision(keep, bank);
        }
    }
}
