using Xunit;
using Zonk.Showcase.AI;
using Zonk.Showcase.TurnEngine;

namespace Zonk.Showcase.Tests
{
    public class StrategyTests
    {
        [Fact]
        public void Aggressive_Tends_To_Continue()
        {
            var ai = new AggressiveStrategy();
            var state = new GameState(1000, 1200, 0, 200, 6);
            var roll = new[] { 1, 5, 5, 2, 3, 4 };
            var d = ai.Decide(state, roll);
            Assert.False(d.Bank);
        }
        [Fact]
        public void Aggressive_Should_Bank_On_HighGain()
        {
           
            var ai = new AggressiveStrategy();
            var state = new GameState(
                playerScore: 1000,
                opponentScore: 1200,
                bankedScore: 0,
                currentTurnScore: 200,
                diceLeft: 6);

            // +1000 from triple ones
            var roll = new[] { 1, 1, 1, 2, 3, 4 };

            
            var d = ai.Decide(state, roll);

            
            // Even aggressive strategy should bank after a +1000 gain at the start of the turn.
            Assert.True(d.Bank);
        }

        [Fact]
        public void Conservative_Tends_To_Bank_Sooner()
        {
            var ai = new ConservativeStrategy();
            var state = new GameState(1000, 1200, 0, 700, 6);
            var roll = new[] { 1, 5, 3, 4, 2, 6 };
            var d = ai.Decide(state, roll);
            Assert.True(d.Bank);
        }

        [Fact]
        public void Fsm_Switches_To_Conservative_After_Two_Zonks_And_Banks_On_HighGain()
        {
            // two zonks -> FSM should switch to Conservative
            var ai = new FsmAdaptiveStrategy();
            var state = new GameState(
                playerScore: 1000,
                opponentScore: 1200,
                bankedScore: 0,
                currentTurnScore: 0,
                diceLeft: 6,
                zonkedCount: 2);

           // +1000 from triple ones -> Conservative should bank (target=300)
            var roll = new[] { 1, 1, 1, 2, 3, 4 };

            // Act
            var d = ai.Decide(state, roll);

            
            // Verify FSM actually switched to Conservative and decided to bank.
            Assert.Equal("Conservative", ai.ActiveStrategyName);
            Assert.True(d.Bank);
        }
    }
}
