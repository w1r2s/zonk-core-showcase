namespace Zonk.Showcase.TurnEngine
{
    /// <summary>
    /// Immutable snapshot of current game state used by AI decisions.
    /// </summary>
    public sealed class GameState
    {
        public int PlayerScore { get; }
        public int OpponentScore { get; }
        public int BankedScore { get; }
        public int CurrentTurnScore { get; }
        public int DiceLeft { get; }
        public int ZonkedCount { get; }

        public GameState(int playerScore, int opponentScore, int bankedScore, int currentTurnScore, int diceLeft, int zonkedCount = 0)
        {
            PlayerScore = playerScore;
            OpponentScore = opponentScore;
            BankedScore = bankedScore;
            CurrentTurnScore = currentTurnScore;
            DiceLeft = diceLeft;
            ZonkedCount = zonkedCount;
        }

        public GameState With(
            int? playerScore = null,
            int? opponentScore = null,
            int? bankedScore = null,
            int? currentTurnScore = null,
            int? diceLeft = null,
            int? zonkedCount = null)
            => new GameState(
                playerScore ?? PlayerScore,
                opponentScore ?? OpponentScore,
                bankedScore ?? BankedScore,
                currentTurnScore ?? CurrentTurnScore,
                diceLeft ?? DiceLeft,
                zonkedCount ?? ZonkedCount);
    }

    public readonly struct Decision
    {
        public int[] Keep { get; }
        public bool Bank { get; }

        public Decision(int[] keep, bool bank)
        {
            Keep = keep ?? System.Array.Empty<int>();
            Bank = bank;
        }
    }
}
