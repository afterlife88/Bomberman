using System.Collections.Generic;

namespace GameEngine
{
	public class PlayerState
	{
		public Queue<KeyboardState> Inputs { get; set; } 
		public Player Player { get; set; }
	}
}
