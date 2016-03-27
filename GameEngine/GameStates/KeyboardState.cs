using System.Collections.Generic;
using GameEngine.Moves;

namespace GameEngine.GameStates
{
	/// <summary>
	/// State of player
	/// </summary>
	public class KeyboardState
	{
		private readonly Dictionary<DirectionsKeys, bool> _keyState;
		public int Id { get; }
		public KeyboardState(Dictionary<DirectionsKeys, bool> keyState, int id)
		{
			_keyState = keyState;
			Id = id;
		}
		public bool this[DirectionsKeys directionsKey] => _keyState[directionsKey];
	}
}
