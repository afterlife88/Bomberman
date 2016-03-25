using System;
using System.Collections.Generic;
using System.Drawing;
using GameEngine.Enums;
using GameEngine.MapGenerator;

namespace GameEngine
{
	public class Enemy : Bomberman
	{
		private readonly Random _random = new Random();
		private readonly IEnumerator<KeyboardState> _behaviour;
		private PlayerState _stateOfPlayer;
		public bool BombSet { get; set; }
		public Enemy()
		{
			_behaviour = GetBehaviour();
		}

		public KeyboardState GetNextMove(PlayerState stateOfPlayer)
		{
			_behaviour.MoveNext();
			_stateOfPlayer = stateOfPlayer;
			return _behaviour.Current;
		}

		private IEnumerator<KeyboardState> GetBehaviour()
		{
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
				//Go away from bomb
				if (ListOfBombs.Count > 0)
				{
					var bombs = this.ListOfBombs.ToArray();
					foreach (var bomb in bombs)
					{
						var ourPosition = new Point(this.X, this.Y);
						var dangetPoints = bomb.GetDangerPoints();
						dangetPoints.Add(bomb.Location);
						if (dangetPoints.Contains(ourPosition))
						{
							if (ourPosition.X >= bomb.Location.X)
							{
								dictionaryUp[Keys.RIGHT] = true;
								for (int i = 0; i < 12; i++)
								{
									int randomNumber = _random.Next(0, 100);
									var ks = new KeyboardState(dictionaryUp, randomNumber);
									yield return ks;
								}
							}
							else
							{
								dictionaryUp[Keys.LEFT] = true;
								for (int i = 0; i < 12; i++)
								{
									int randomNumber = _random.Next(0, 100);
									var ks = new KeyboardState(dictionaryUp, randomNumber);
									yield return ks;
								}
							}
							if (ourPosition.Y >= bomb.Location.Y)
							{
								dictionaryUp[Keys.UP] = true;
								for (int i = 0; i < 12; i++)
								{
									int randomNumber = _random.Next(0, 100);
									var ks = new KeyboardState(dictionaryUp, randomNumber);
									yield return ks;
								}
							}
							else
							{
								dictionaryUp[Keys.DOWN] = true;
								for (int i = 0; i < 12; i++)
								{
									int randomNumber = _random.Next(0, 100);
									var ks = new KeyboardState(dictionaryUp, randomNumber);
									yield return ks;
								}
							}
						}
					}
				}

				//if (num <= 25) //25%
				//{
				//	dictionaryUp[Keys.LEFT] = true;
				//}
				//if (num > 25 && num <= 50)
				//{
				//	dictionaryUp[Keys.UP] = true;
				//}
				//if (num > 50 && num <= 75)
				//{
				//	dictionaryUp[Keys.RIGHT] = true;
				//}
				if (num > 75 && num <= 100)
				{
					dictionaryUp[Keys.SPACE] = true;
					BombSet = true;
				}
				for (int i = 0; i < 12; i++)
				{
					int randomNumber = _random.Next(0, 100);
					var ks = new KeyboardState(dictionaryUp, randomNumber);
					yield return ks;
				}

			}
		}
	}
}
