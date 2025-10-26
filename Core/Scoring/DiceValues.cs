using System;
using System.Collections.Generic;
using System.Linq;

namespace Zonk.Showcase.Scoring
{
    /// <summary>
    /// Represents a single dice roll (values 1..6) as an immutable collection.
    /// </summary>
    public sealed class DiceValues
    {
        public IReadOnlyList<int> Values { get; }

        public DiceValues(IEnumerable<int> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            var arr = values.ToArray();
            if (arr.Length == 0) throw new ArgumentException("Dice list is empty.");
            if (arr.Any(v => v < 1 || v > 6)) throw new ArgumentException("All dice must be between 1 and 6.");
            Array.Sort(arr);
            Values = arr;
        }

        public int Count => Values.Count;
        public int CountOf(int face) => Values.Count(v => v == face);

        public override string ToString() => $"[{string.Join(",", Values)}]";
    }
}
