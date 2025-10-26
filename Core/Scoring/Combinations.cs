using System.Collections.Generic;

namespace Zonk.Showcase.Scoring
{
    public enum DiceCombination
    {
        One,
        Five,
        ThreeOfAKind,
        FourOfAKind,
        FiveOfAKind,
        SixOfAKind,
        Straight
    }

    public sealed class ComboInfo
    {
        public DiceCombination Combination { get; }
        public IReadOnlyList<int> Dice { get; }
        public int Value { get; }

        public ComboInfo(DiceCombination combination, IReadOnlyList<int> dice, int value)
        {
            Combination = combination;
            Dice = dice;
            Value = value;
        }

        public override string ToString() => $"{Combination} ({string.Join(",", Dice)}) = {Value}";
    }
}
