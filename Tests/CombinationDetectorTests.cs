using Xunit;
using Zonk.Showcase.Scoring;
using System.Linq;

namespace Zonk.Showcase.Tests
{
    public class CombinationDetectorTests
    {
        [Fact]
        public void Detects_Straight()
        {
            var roll = new DiceValues(new[] { 1, 2, 3, 4, 5, 6 });
            var combos = CombinationDetector.DetectAll(roll);
            Assert.Contains(combos, c => c.Combination == DiceCombination.Straight);
        }

        [Fact]
        public void Detects_Triples()
        {
            var roll = new DiceValues(new[] { 2, 2, 2, 3, 4, 6 });
            var combos = CombinationDetector.DetectAll(roll);
            Assert.Single(combos.Where(c => c.Combination == DiceCombination.ThreeOfAKind));
        }

        [Fact]
        public void Detects_Singles()
        {
            var roll = new DiceValues(new[] { 1, 2, 3, 4, 5, 6 });
            var combos = CombinationDetector.DetectAll(roll);
            Assert.Contains(combos, c => c.Combination == DiceCombination.One);
            Assert.Contains(combos, c => c.Combination == DiceCombination.Five);
        }
    }
}
