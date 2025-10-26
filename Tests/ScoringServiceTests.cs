using Xunit;
using Zonk.Showcase.Scoring;

namespace Zonk.Showcase.Tests
{
    public class ScoringServiceTests
    {
        private readonly ScoringService _s = new ScoringService();

        [Theory]
        [InlineData(new[] { 1, 2, 3, 4, 5, 6 }, 1500)]
        [InlineData(new[] { 1, 1, 1 }, 1000)]
        [InlineData(new[] { 1, 1, 1, 1 }, 2000)]
        [InlineData(new[] { 5, 5, 5 }, 500)]
        [InlineData(new[] { 2, 2, 2, 2 }, 400)]
        [InlineData(new[] { 4, 4, 4, 4, 4 }, 1200)]
        [InlineData(new[] { 1, 5 }, 150)]
        public void Calculates_Score_Correctly(int[] dice, int expected)
        {
            Assert.Equal(expected, _s.Score(dice));
        }

        [Fact]
        public void Non_Zonk_When_Scoring_Present()
        {
            Assert.False(_s.IsZonkStrict(new[] { 1, 2, 3, 4, 6 }));
        }
    }
}
