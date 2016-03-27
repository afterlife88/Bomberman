using System;
using System.Collections.Generic;
using GameEngine.Common;
using GameEngine.Enums;
using GameEngine.GameStates;

namespace GameEngine.GameObjects
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
			// random moves
			while (true)
			{
				var num = _random.Next(100);
				BombSet = false;

				if (num <= 24)
				{
					state = GetDownKeyboardState(Keys.LEFT);
				}
				if (num > 24 && num <= 48)
				{
					state = GetDownKeyboardState(Keys.UP);
				}
				if (num > 48 && num <= 72)
				{
					state = GetDownKeyboardState(Keys.RIGHT);
				}
				if (num > 72 && num <= 98)
				{
					state = GetDownKeyboardState(Keys.DOWN);
				
				}
				if (num > 98 && num <= 100)
				{
					state = GetDownKeyboardState(Keys.SPACE);
					BombSet = true;
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
