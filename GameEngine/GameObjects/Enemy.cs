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
				// Get avalible move
				var avalibleList = GetPath();
				// Hit random move
				var randomMove = _random.Next(avalibleList.Count);
				// Set bomb to true if hit plant move
				BombSet = false || avalibleList[randomMove] == Keys.SPACE;
				state = GetDownKeyboardState(avalibleList[randomMove]);
				for (var i = 0; i < ConstantValues.CountToMoveOnActualPosition; i++)
				{
					yield return state;
				}
			}
		}
		/// <summary>
		/// Check if can make next step to avoid walls and bricks
		/// </summary>
		/// <returns></returns>
		private List<Keys> GetPath()
		{
			var avalibleMoves = new List<Keys>();
			var num = _random.Next(100);
			if (this.Movable(this.X - 1, this.Y))
			{
				avalibleMoves.Add(Keys.LEFT);
			}
			if (this.Movable(this.X, this.Y - 1))
			{
				avalibleMoves.Add(Keys.UP);
			}
			if (this.Movable(this.X + 1, this.Y))
			{
				avalibleMoves.Add(Keys.RIGHT);
			}
			if (this.Movable(this.X, this.Y + 1))
			{
				avalibleMoves.Add(Keys.DOWN);
			}
			if (num > 85 && num <= 100)
				avalibleMoves.Add(Keys.SPACE);
			return avalibleMoves;
			
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
