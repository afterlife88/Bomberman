using System;
using System.Collections.Generic;
using GameEngine.Common;
using GameEngine.Enums;

namespace GameEngine
{
	public class Enemy : Bomberman
	{
		private static Random _random = new Random();
		private readonly IEnumerator<KeyboardState> _behaviour;

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
			KeyboardState state = null;

			while (true)
			{
				var num = _random.Next(100);
				BombSet = false;

				if (num <= 25) //25%
				{
					state = GetDownKeyboardState(Keys.LEFT);
				}
				if (num > 25 && num <= 50)
				{
					state = GetDownKeyboardState(Keys.UP);
				}
				if (num > 50 && num <= 75)
				{
					state = GetDownKeyboardState(Keys.RIGHT);
				}
				if (num > 75 && num <= 100)
				{
					state = GetDownKeyboardState(Keys.DOWN);
					//BombSet = true;
				}
				for (int i = 0; i < ConstantValues.CountToMoveOnActualPosition; i++)
				{
					yield return state;
				}
			}
		}
		private static KeyboardState GetDownKeyboardState(Keys key)
		{
			var dict = new Dictionary<Keys, bool>();
			foreach (var v in Enum.GetValues(typeof(Keys)))
			{
				dict[(Keys)v] = false;
			}

			dict[key] = true;

			return new KeyboardState(dict, 0);
		}
	}
}
