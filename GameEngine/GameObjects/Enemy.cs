using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameEngine.Common;
using GameEngine.GameStates;
using GameEngine.Map;
using GameEngine.Moves;

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
			while (true)
			{
				KeyboardState state = null;

				if (MapLoader.MapInstance.ListOfBombs.Count > 0)
				{
					state = CheckBomb();
				}
				// random move
				if (state == null)
				{
					// Get avalible move, with probability to set plant
					var avalibleList = GetPath(true);
					// Hit random move
					var randomMove = _random.Next(avalibleList.Count);
					// Set bomb to true if hit plant move
					BombSet = false || avalibleList[randomMove] == DirectionsKeys.SPACE;
					state = GetDownKeyboardState(avalibleList[randomMove]);
				}
				for (var i = 0; i < ConstantValues.CountToMoveOnActualPosition; i++)
				{
					yield return state;
				}
			}
		}
		/// <summary>
		/// Check bomb on the map, and move out of them
		/// </summary>
		/// <returns></returns>
		private KeyboardState CheckBomb()
		{
			foreach (var bomb in MapLoader.MapInstance.ListOfBombs)
			{
				var ourPosition = new Point(this.X, this.Y);

				var dangerPoints = MapLoader.MapInstance.PointsToExplode.ToList();
				dangerPoints.Add(bomb.Location);
				var availableMoves = GetPath(false);
				if (dangerPoints.Contains(ourPosition))
				{
					if (ourPosition.X >= bomb.Location.X && availableMoves.Contains(DirectionsKeys.RIGHT))
					{
						return GetDownKeyboardState(DirectionsKeys.RIGHT);
					}
					if (ourPosition.X <= bomb.Location.X && availableMoves.Contains(DirectionsKeys.LEFT))
					{
						return GetDownKeyboardState(DirectionsKeys.LEFT);
					}
					if (ourPosition.Y >= bomb.Location.Y && availableMoves.Contains(DirectionsKeys.UP))
					{
						return GetDownKeyboardState(DirectionsKeys.UP);
					}
					if (ourPosition.Y <= bomb.Location.Y && availableMoves.Contains(DirectionsKeys.DOWN))
					{
						return GetDownKeyboardState(DirectionsKeys.DOWN);
					}
					return GetDownKeyboardState(availableMoves[_random.Next(availableMoves.Count)]);
				}
			}
			return null;


		}
		/// <summary>
		/// Check if can make next step to avoid walls and bricks
		/// </summary>
		/// <returns></returns>
		private List<DirectionsKeys> GetPath(bool canSetBomb)
		{
			var avalibleMoves = new List<DirectionsKeys>();
			var num = _random.Next(100);
			if (this.Movable(this.X - 1, this.Y))
			{
				avalibleMoves.Add(DirectionsKeys.LEFT);
			}
			if (this.Movable(this.X, this.Y - 1))
			{
				avalibleMoves.Add(DirectionsKeys.UP);
			}
			if (this.Movable(this.X + 1, this.Y))
			{
				avalibleMoves.Add(DirectionsKeys.RIGHT);
			}
			if (this.Movable(this.X, this.Y + 1))
			{
				avalibleMoves.Add(DirectionsKeys.DOWN);
			}
			// 10 % probability to plant bomb 
			if (num > 90 && num <= 100 && canSetBomb)
				avalibleMoves.Add(DirectionsKeys.SPACE);
			return avalibleMoves;

		}
		private KeyboardState GetDownKeyboardState(DirectionsKeys directionsKey)
		{
			var dict = new Dictionary<DirectionsKeys, bool>();
			foreach (var v in Enum.GetValues(typeof(DirectionsKeys)))
			{
				dict[(DirectionsKeys)v] = false;
			}

			dict[directionsKey] = true;

			return new KeyboardState(dict, 0);
		}
	}
}
