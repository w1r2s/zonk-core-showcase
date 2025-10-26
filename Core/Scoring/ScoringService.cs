using System.Linq;

namespace Zonk.Showcase.Scoring
{
    /// <summary>
    /// Implements Zonk/Farkle scoring rules.
    /// Singles: 1=100, 5=50.
    /// Sets: 1 -> 1000/2000/3000/4000; others f -> f*100/f*200/f*300/f*400.
    /// Straight 1..6 = 1500.
    /// </summary>
    public sealed class ScoringService
    {
        public int Score(int[] dice)
        {
            var vals = new DiceValues(dice).Values.ToArray();

            if (CombinationDetector.IsStraight(vals)) return 1500;

            int total = 0;
            foreach (var g in vals.GroupBy(x => x))
                if (g.Count() >= 3)
                    total += CombinationDetector.SetScoreLikeOriginal(g.Key, g.Count());

            int c1 = vals.Count(v => v == 1);
            int c5 = vals.Count(v => v == 5);
            if (c1 < 3) total += 100 * c1;
            if (c5 < 3) total += 50 * c5;

            return total;
        }

        public bool IsZonkStrict(int[] dice)
        {
            var vals = new DiceValues(dice).Values.ToArray();
            if (CombinationDetector.IsStraight(vals)) return false;
            if (vals.GroupBy(x => x).Any(g => g.Count() >= 3)) return false;
            if (vals.Any(v => v == 1 || v == 5)) return false;
            return true;
        }

        public bool IsZonk(int[] dice) => Score(dice) == 0;

        public bool AllUsedScoring(int[] kept, int diceLeftBeforeKeep)
        {
            if (kept == null || kept.Length == 0) return false;
            if (Score(kept) <= 0) return false;
            return kept.Length == diceLeftBeforeKeep;
        }
    }
}
