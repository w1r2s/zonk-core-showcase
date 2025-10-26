using Zonk.Showcase.TurnEngine;

namespace Zonk.Showcase.AI
{
    /// <summary>
    /// Finite-state agent that switches strategies by match context:
    /// - If leading  (BankedScore > OpponentScore) -> Aggressive.
    /// - If trailing (BankedScore < OpponentScore) -> Conservative.
    /// - If tied                                   -> keep current strategy.
    /// - Safety override: if ZonkedCount >= 2      -> Conservative (always wins).
    ///
    /// BankedScore is the last committed total (post-bank), used as a stable reference.
    /// </summary>
    public sealed class FsmAdaptiveStrategy : BaseAI
    {
        public override string Name => "FSM-Adaptive";

        // Default to Aggressive at start.
        private BaseAI _current = new AggressiveStrategy();

        /// <summary>
        /// Exposes the active strategy name for tests and debugging.
        /// </summary>
        public string ActiveStrategyName => _current.Name;

        public override Decision Decide(GameState state, int[] roll)
        {
            // 1) Score-based switching (tie means "keep current")
            if (state.BankedScore > state.OpponentScore)
            {
                if (_current is not AggressiveStrategy)
                    _current = new AggressiveStrategy();
            }
            else if (state.BankedScore < state.OpponentScore)
            {
                if (_current is not ConservativeStrategy)
                    _current = new ConservativeStrategy();
            }

            // 2) Safety override (applied last to avoid being overwritten)
            if (state.ZonkedCount >= 2 && _current is not ConservativeStrategy)
                _current = new ConservativeStrategy();

            return _current.Decide(state, roll);
        }
    }
}
