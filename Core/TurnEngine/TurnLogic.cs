using Zonk.Showcase.Scoring;

namespace Zonk.Showcase.TurnEngine
{
    public static class TurnLogic
    {
        private static readonly ScoringService S = new ScoringService();

        public static bool IsZonkRoll(int[] roll) => S.IsZonkStrict(roll);

        public static GameState ApplyKeep(GameState s, int[] kept)
        {
            int gained = S.Score(kept);
            int diceLeftAfter = S.AllUsedScoring(kept, s.DiceLeft) ? 6 : s.DiceLeft - kept.Length;
            return s.With(currentTurnScore: s.CurrentTurnScore + gained, diceLeft: diceLeftAfter);
        }

        public static GameState Bank(GameState s)
        {
            int newTotal = s.PlayerScore + s.CurrentTurnScore;
            return s.With(
                playerScore: newTotal,
                bankedScore: newTotal,
                currentTurnScore: 0,
                diceLeft: 6
            );
        }

        public static GameState Zonk(GameState s) =>
            s.With(currentTurnScore: 0, diceLeft: 6, zonkedCount: s.ZonkedCount + 1);
    }
}
