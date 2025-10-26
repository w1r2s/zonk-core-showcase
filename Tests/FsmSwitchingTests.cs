using Xunit;
using Zonk.Showcase.AI;
using Zonk.Showcase.TurnEngine;

namespace Zonk.Showcase.Tests
{
    public class FsmSwitchingByScoreTests
    {
        [Fact]
        public void Fsm_Switches_To_Aggressive_When_Leading_Opponent()
        {
            // Arrange: leading -> prefer Aggressive
            var ai = new FsmAdditiveStrategy();
            var state = new GameState(
                playerScore: 2200,
                opponentScore: 1500,
                bankedScore: 2100, // leading
                currentTurnScore: 0,
                diceLeft: 6,
                zonkedCount: 0);

            var roll = new[] { 1, 5, 2, 3, 4, 6 };

            // Act
            var decision = ai.Decide(state, roll);

            // Assert
            Assert.Equal("Aggressive", ai.ActiveStrategyName);
        }

        [Fact]
        public void Fsm_Switches_To_Conservative_When_Trailing_Opponent()
        {
            // Arrange: trailing -> prefer Conservative
            var ai = new FsmAdditiveStrategy();
            var state = new GameState(
                playerScore: 1000,
                opponentScore: 1800,
                bankedScore: 900,   // trailing
                currentTurnScore: 0,
                diceLeft: 6,
                zonkedCount: 0);

            var roll = new[] { 1, 2, 3, 4, 6, 2 };

            // Act
            var decision = ai.Decide(state, roll);

            // Assert
            Assert.Equal("Conservative", ai.ActiveStrategyName);
        }

        [Fact]
        public void Fsm_Zonk_Override_Forces_Conservative_Even_If_Leading()
        {
            // Arrange: leading but two zonks -> safety override to Conservative
            var ai = new FsmAdditiveStrategy();
            var state = new GameState(
                playerScore: 2200,
                opponentScore: 1500,
                bankedScore: 2100,  // leading
                currentTurnScore: 0,
                diceLeft: 6,
                zonkedCount: 2);     // override

            var roll = new[] { 1, 1, 1, 2, 3, 4 };

            // Act
            var decision = ai.Decide(state, roll);

            // Assert
            Assert.Equal("Conservative", ai.ActiveStrategyName);
        }
        [Fact]
        public void Fsm_Keeps_Current_On_Tie()
        {
            var ai = new FsmAdditiveStrategy();
            var state = new GameState(
                playerScore: 0, opponentScore: 1500,
                bankedScore: 1500, currentTurnScore: 0,
                diceLeft: 6, zonkedCount: 0);

            var roll = new[] { 1, 2, 3, 4, 5, 6 };
            ai.Decide(state, roll);

            // On tie, FSM keeps the current strategy (defaults to Aggressive)
            Assert.Equal("Aggressive", ai.ActiveStrategyName);
        }
        [Fact]
        public void Fsm_Switches_Trailing_Then_Back_To_Aggressive_When_Leading()
        {
            var ai = new FsmAdditiveStrategy();

            // Trailing -> Conservative
            var s1 = new GameState(
                playerScore: 0, opponentScore: 1800,
                bankedScore: 900, currentTurnScore: 0,
                diceLeft: 6, zonkedCount: 0);
            ai.Decide(s1, new[] { 1, 5, 2, 3, 4, 6 });
            Assert.Equal("Conservative", ai.ActiveStrategyName);

            // After banking enough, we are leading -> Aggressive
            var s2 = new GameState(
                playerScore: 0, opponentScore: 1500,
                bankedScore: 2000, currentTurnScore: 0,
                diceLeft: 6, zonkedCount: 0);
            ai.Decide(s2, new[] { 2, 2, 2, 3, 4, 6 });
            Assert.Equal("Aggressive", ai.ActiveStrategyName);
        }
    }
}
