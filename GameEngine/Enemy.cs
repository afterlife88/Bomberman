using System;
using System.Collections.Generic;
using GameEngine.Enums;

namespace GameEngine
{
	public class Enemy : Bomberman
	{
		private static Random _random = new Random();
		private readonly IEnumerator<KeyboardState> _behaviour;
		public bool BombSet { get; set; }
		public Enemy()
		{
			_behaviour = GetBehaviour();
		}

		public KeyboardState GetNextMove()
		{
			_behaviour.MoveNext();
			return _behaviour.Current;
		}

		private IEnumerator<KeyboardState> GetBehaviour()
		{
			//var map = MapLoader.GetMap;
			//var getCurrentPlace = map[this.X, this.Y];
			Dictionary<Keys, bool> dictionaryUp;

			while (true)
			{
				var num = _random.Next(100);

				dictionaryUp = new Dictionary<Keys, bool>
				{
					[Keys.LEFT] = false,
					[Keys.DOWN] = false,
					[Keys.UP] = false,
					[Keys.RIGHT] = false,
					[Keys.SPACE] = false
				};
				BombSet = false;

				if (num <= 25) //25%
				{
					dictionaryUp[Keys.LEFT] = true;

					for (int i = 0; i < 12; i++)
					{
						int randomNumber = _random.Next(0, 100);
						var ks = new KeyboardState(dictionaryUp, randomNumber);
						yield return ks;
					}
				}
				if (num > 25 && num <= 50)
				{
					dictionaryUp[Keys.UP] = true;

					for (int i = 0; i < 12; i++)
					{
						int randomNumber = _random.Next(0, 100);
						var ks = new KeyboardState(dictionaryUp, randomNumber);
						yield return ks;
					}
				}
				if (num > 50 && num <= 75)
				{
					dictionaryUp[Keys.RIGHT] = true;

					for (int i = 0; i < 12; i++)
					{
						int randomNumber = _random.Next(0, 100);
						var ks = new KeyboardState(dictionaryUp, randomNumber);
						yield return ks;
					}
				}
				if (num > 75 && num <= 100)
				{
					dictionaryUp[Keys.SPACE] = true;

					for (int i = 0; i < 12; i++)
					{
						int randomNumber = _random.Next(0, 100);
						var ks = new KeyboardState(dictionaryUp, randomNumber);
						BombSet = true;
						yield return ks;
					}
				}
			}
		}
	}
}
