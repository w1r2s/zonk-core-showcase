using System.Collections.Generic;
using System.Linq;
using Zonk.Showcase.Scoring;
using Zonk.Showcase.TurnEngine;

namespace Zonk.Showcase.AI
{
    /// <summary>
    /// Aggressive strategy:
    /// - target is 600 initially; after re-roll target becomes (roundScore + 400).
    /// - priority: Six > Straight > Five > Four (4x2=400 only if total combos are 1 or 3) > Three (3x2=200 only if it is the only combo).
    /// - singles (1/5) are used only if no set was picked.
    /// - if all remaining dice can be covered by scoring combos, take all to trigger hot dice.
    /// </summary>
    public sealed class AggressiveStrategy : BaseAI
    {
        public override string Name => "Aggressive";
        private readonly ScoringService _s = new ScoringService();

        public override Decision Decide(GameState state, int[] roll)
        {
            var all = CombinationDetector.DetectAll(new DiceValues(roll));
            var picks = new List<ComboInfo>();

            var six = all.Where(c => c.Combination == DiceCombination.SixOfAKind).OrderByDescending(c => c.Value).ToList();
            var stra = all.Where(c => c.Combination == DiceCombination.Straight).ToList();
            var five = all.Where(c => c.Combination == DiceCombination.FiveOfAKind).OrderByDescending(c => c.Value).ToList();
            var four = all.Where(c => c.Combination == DiceCombination.FourOfAKind).OrderByDescending(c => c.Value).ToList();
            var tri = all.Where(c => c.Combination == DiceCombination.ThreeOfAKind).OrderByDescending(c => c.Value).ToList();
            var ones = all.Where(c => c.Combination == DiceCombination.One).ToList();
            var fivs = all.Where(c => c.Combination == DiceCombination.Five).ToList();

            int totalCombos = six.Count + stra.Count + five.Count + four.Count + tri.Count + ones.Count + fivs.Count;
            var usedIdx = new List<int>();

            bool Take(ComboInfo ci)
            {
                var need = ci.Dice.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
                var usedCounts = usedIdx.GroupBy(i => roll[i]).ToDictionary(g => g.Key, g => g.Count());
                foreach (var kv in need)
                {
                    int face = kv.Key, needCount = kv.Value;
                    int haveTotal = roll.Count(v => v == face);
                    int already = usedCounts.TryGetValue(face, out var u) ? u : 0;
                    if (haveTotal - already < needCount) return false;
                }
                foreach (var kv in need)
                {
                    int face = kv.Key, needCount = kv.Value;
                    for (int i = 0; i < roll.Length && needCount > 0; i++)
                    {
                        if (!usedIdx.Contains(i) && roll[i] == face)
                        {
                            usedIdx.Add(i);
                            needCount--;
                        }
                    }
                }
                picks.Add(ci);
                return true;
            }

            // Priority
            foreach (var ci in six) Take(ci);
            foreach (var ci in stra) Take(ci);
            foreach (var ci in five) Take(ci);

            foreach (var ci in four)
            {
                bool isFourTwos = ci.Dice.Count == 4 && ci.Dice.All(v => v == 2);
                if (isFourTwos && !(totalCombos == 1 || totalCombos == 3)) continue;
                Take(ci);
            }

            foreach (var ci in tri)
            {
                bool isTripleTwos = ci.Dice.Count == 3 && ci.Dice.All(v => v == 2);
                if (isTripleTwos && totalCombos != 1) continue;
                Take(ci);
            }

            bool anySetChosen = picks.Any(p =>
                   p.Combination == DiceCombination.ThreeOfAKind
                || p.Combination == DiceCombination.FourOfAKind
                || p.Combination == DiceCombination.FiveOfAKind
                || p.Combination == DiceCombination.SixOfAKind
                || p.Combination == DiceCombination.Straight);

            if (!anySetChosen)
            {
                foreach (var ci in ones) Take(ci);
                foreach (var ci in fivs) Take(ci);
            }

            // Try to cover all dice to trigger hot dice
            if (usedIdx.Count != roll.Length && anySetChosen)
            {
                foreach (var ci in ones.Concat(fivs))
                {
                    if (Take(ci) && usedIdx.Count == roll.Length) break;
                }
            }

            var keep = picks.SelectMany(p => p.Dice).ToArray();
            if (keep.Length == 0) return new Decision(System.Array.Empty<int>(), false);

            int gained = _s.Score(keep);
            int turnAfter = state.CurrentTurnScore + gained;

            int target = (state.CurrentTurnScore > 0) ? (state.CurrentTurnScore + 400) : 600;
            int diceLeftAfter = (keep.Length == state.DiceLeft) ? 6 : (state.DiceLeft - keep.Length);

            bool bank = (turnAfter >= target && diceLeftAfter >= 3)
                        || (diceLeftAfter <= 2 && turnAfter >= 600);

            return new Decision(keep, bank);
        }
    }
}
