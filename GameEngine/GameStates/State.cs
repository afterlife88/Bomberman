using System.Collections.Concurrent;
using GameEngine.GameObjects;

namespace GameEngine.GameStates
{
	public abstract class State
	{
		public ConcurrentQueue<KeyboardState> Inputs { get; set; }
		protected Bomberman Bomberman { get; set; }
	}
}
