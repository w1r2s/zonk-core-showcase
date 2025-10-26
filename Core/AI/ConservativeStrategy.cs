using System.Collections.Generic;
using System.Linq;
using Zonk.Showcase.Scoring;
using Zonk.Showcase.TurnEngine;

namespace Zonk.Showcase.AI
{
    /// <summary>
    /// Conservative strategy:
    /// - target is 300 initially; after re-roll target becomes (roundScore + 150).
    /// - tends to bank early when few dice remain (<= 2).
    /// - priority: sets (3+) > one 1 > one 5.
    /// </summary>
    public sealed class ConservativeStrategy : BaseAI
    {
        public override string Name => "Conservative";
        private readonly ScoringService _s = new ScoringService();

        public override Decision Decide(GameState state, int[] roll)
        {
            var combos = CombinationDetector.DetectAll(new DiceValues(roll));
            var picks = new List<ComboInfo>();
            var used = new Dictionary<int, int>();

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

            // Prefer sets first
            foreach (var ci in combos.Where(c =>
                    c.Combination == DiceCombination.SixOfAKind
                 || c.Combination == DiceCombination.FiveOfAKind
                 || c.Combination == DiceCombination.FourOfAKind
                 || c.Combination == DiceCombination.ThreeOfAKind)
                 .OrderByDescending(c => c.Value))
            {
                if (CanTake(ci)) Take(ci);
            }

            // Then minimal singles (one 1, then one 5)
            foreach (var ci in combos.Where(c => c.Combination == DiceCombination.One).Take(1))
                if (CanTake(ci)) Take(ci);

            foreach (var ci in combos.Where(c => c.Combination == DiceCombination.Five).Take(1))
                if (CanTake(ci)) Take(ci);

            var keep = picks.SelectMany(p => p.Dice).ToArray();
            if (keep.Length == 0) return new Decision(System.Array.Empty<int>(), false);

            int gained = _s.Score(keep);
            int turnAfter = state.CurrentTurnScore + gained;
            int diceLeftAfter = (keep.Length == state.DiceLeft) ? 6 : (state.DiceLeft - keep.Length);

            int target = (state.CurrentTurnScore > 0) ? (state.CurrentTurnScore + 150) : 300;

            bool bank = turnAfter >= target || diceLeftAfter <= 2;
            return new Decision(keep, bank);
        }
    }
}
