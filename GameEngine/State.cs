using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GameEngine
{
	/// <summary>
	/// State of 
	/// </summary>
	public abstract class State<T>
	{
		public ConcurrentQueue<KeyboardState> Inputs { get; set; }
		protected T Bomberman { get; set; }
	}

	public class PlayerState : State<Player>
	{
		public Player Player
		{
			get
			{
				return Bomberman;
			}
			set { Bomberman = value; }
		}
	}

	public class EnemyState : State<Enemy>
	{
		public Enemy Enemy
		{
			get
			{
				return Bomberman;
			}
			set { Bomberman = value; }
		}
	}
}
