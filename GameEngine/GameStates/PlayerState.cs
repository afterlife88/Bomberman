using GameEngine.GameObjects;

namespace GameEngine.GameStates
{
	public class PlayerState : State
	{
		public Player Player
		{
			get
			{
				return Bomberman as Player;
			}
			set { Bomberman = value; }
		}

	}
}
