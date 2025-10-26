using Zonk.Showcase.TurnEngine;

namespace Zonk.Showcase.AI
{
	/// <summary>
	/// Core-only base class for AI strategies.
	///
	/// Original runtime roles:
	/// - GameControl: match controller (turn order, win conditions).
	/// - GameRound: human turn adapter (UI/input -> applies keep/bank).
	/// - AIController: agent turn adapter (calls strategy, applies result).
	///
	/// In this module:
	/// - BaseAI is a pure C# class defining the decision interface.
	/// - Strategies (Aggressive, Conservative, Cautious, FSM) inherit from it.
	/// - The logic matches the original game, excluding Unity-specific code.
	/// </summary>
	public abstract class BaseAI
	{
		public abstract string Name { get; }

		/// <summary>
		/// Calculates the next decision given the current game state and dice roll.
		/// </summary>
		public abstract Decision Decide(GameState state, int[] roll);
	}
}
