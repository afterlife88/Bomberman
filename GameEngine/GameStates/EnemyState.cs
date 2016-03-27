using GameEngine.GameObjects;

namespace GameEngine.GameStates
{
	public class EnemyState : State
	{
		public Enemy Enemy
		{
			get
			{
				return Bomberman as Enemy;
			}
			set { Bomberman = value; }
		}
	}
}
