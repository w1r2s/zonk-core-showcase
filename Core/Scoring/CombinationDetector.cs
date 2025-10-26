using System;
using System.Collections.Generic;
using System.Linq;

namespace Zonk.Showcase.Scoring
{
    /// <summary>
    /// Detects all valid scoring combinations for a given dice roll.
    /// No "three pairs" rule. Supports straights, n-of-a-kind, and single 1/5.
    /// </summary>
    public static class CombinationDetector
    {
        public static IReadOnlyList<ComboInfo> DetectAll(DiceValues roll)
        {
            var list = new List<ComboInfo>();
            var vals = roll.Values.ToArray();

            if (IsStraight(vals))
                list.Add(new ComboInfo(DiceCombination.Straight, vals, 1500));

            foreach (var g in vals.GroupBy(x => x).OrderBy(x => x.Key))
            {
                if (g.Count() >= 3)
                {
                    int face = g.Key;
                    int cnt = g.Count();
                    int value = SetScoreLikeOriginal(face, cnt);
                    var type = cnt switch
                    {
                        3 => DiceCombination.ThreeOfAKind,
                        4 => DiceCombination.FourOfAKind,
                        5 => DiceCombination.FiveOfAKind,
                        _ => DiceCombination.SixOfAKind
                    };
                    list.Add(new ComboInfo(type, g.ToArray(), value));
                }
            }

            int ones = vals.Count(v => v == 1);
            int fives = vals.Count(v => v == 5);

            for (int i = 0; i < ones && i < 3; i++)
                list.Add(new ComboInfo(DiceCombination.One, new[] { 1 }, 100));

            for (int i = 0; i < fives && i < 3; i++)
                list.Add(new ComboInfo(DiceCombination.Five, new[] { 5 }, 50));

            return list;
        }

        public static bool IsStraight(IReadOnlyList<int> vals)
            => vals.Count == 6 && vals.Distinct().Count() == 6 && vals[0] == 1 && vals[5] == 6;

        public static int SetScoreLikeOriginal(int face, int count)
        {
            if (count < 3) return 0;
            int baseScore = (face == 1) ? 1000 : face * 100;

            int score = baseScore; // triple
            int extras = count - 3; // add for each extra beyond triple
            if (extras > 0) score += baseScore * extras;
            return score;
        }
    }
}
