using Xunit;
using Zonk.Showcase.Scoring;

namespace Zonk.Showcase.Tests
{
    public class ZonkTests
    {
        private readonly ScoringService _s = new ScoringService();

        // ����: ��� ������ 1�6, ��� ����� (3+), ��� ��������� 1 ��� 5
        [Theory]
        [InlineData(new[] { 2, 2, 3, 4, 6 })]
        [InlineData(new[] { 2, 3, 3, 4, 6 })]
        [InlineData(new[] { 2, 3, 4, 6, 6 })]
        [InlineData(new[] { 2, 2, 3, 4, 4 })]
        public void IsZonkStrict_Returns_True_On_Purely_NonScoring_Rolls(int[] dice)
        {
            Assert.True(_s.IsZonkStrict(dice));
        }

        // �� ����: ���� ���� �� ���� ����� (������, ��� 3+, ��������� 1 ��� 5)
        [Theory]
        [InlineData(new[] { 1, 2, 3, 4, 6 })]        // single 1
        [InlineData(new[] { 2, 3, 4, 5, 6 })]        // single 5
        [InlineData(new[] { 2, 2, 2, 3, 4, 6 })]     // set of twos
        [InlineData(new[] { 1, 2, 3, 4, 5, 6 })]     // Straight 1..6
        public void IsZonkStrict_Returns_False_When_Scoring_Present(int[] dice)
        {
            Assert.False(_s.IsZonkStrict(dice));
        }
    }
}
