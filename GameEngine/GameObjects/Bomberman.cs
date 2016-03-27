using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using GameEngine.Common;
using GameEngine.Enums;
using GameEngine.GameStates;
using GameEngine.Map;

namespace GameEngine.GameObjects
{
	/// <summary>
	/// Base class for bomberman
	/// </summary>
	public class Bomberman
	{
		private static readonly Point[] EastTargets = { new Point(1, -1), new Point(1, 0), new Point(1, 1) };
		private static readonly Point[] WestTargets = { new Point(-1, -1), new Point(-1, 0), new Point(-1, 1) };
		private static readonly Point[] NorthTargets = { new Point(-1, -1), new Point(0, -1), new Point(1, -1) };
		private static readonly Point[] SouthTargets = { new Point(-1, 1), new Point(0, 1), new Point(1, 1) };
		public int X { get; set; }
		public int Y { get; set; }
		public int ExactX { get; set; }
		public int ExactY { get; set; }
		public int DirectionX { get; set; }
		public int DirectionY { get; set; }
		public Directions Direction { get; set; }
		public int Index { get; set; }
		public int LastProcessed { get; set; }
		private int MaxBombs { get; set; } = 1;
		public int Bombs { get; set; }
		public bool BombSet { get; set; }
		public void Update(KeyboardState input)
		{

			LastProcessed = input.Id;

			int x = ExactX,
				y = ExactY;

			if (!input[Keys.UP])
			{
				DirectionY = 0;
			}

			if (!input[Keys.DOWN])
			{
				DirectionY = 0;
			}

			if (!input[Keys.LEFT])
			{
				DirectionX = 0;
			}

			if (!input[Keys.RIGHT])
			{
				DirectionX = 0;
			}

			if (input[Keys.DOWN])
			{
				DirectionY = 1;
			}

			if (input[Keys.UP])
			{
				DirectionY = -1;
			}

			if (input[Keys.LEFT])
			{
				DirectionX = -1;
			}

			if (input[Keys.RIGHT])
			{
				DirectionX = 1;
			}
			if (input[Keys.SPACE])
			{
				CreateBomb();
			}
			SetDirection(DirectionX, DirectionY);

			x += DirectionX * ConstantValues.Delta;
			y += DirectionY * ConstantValues.Delta;

			MoveExact(x, y);
		}
		public void RemoveBomb()
		{
			Bombs--;
		}
		private void CreateBomb()
		{
			BombSet = true;
			if (Bombs >= MaxBombs)
			{
				return;
			}
			var bomb = new Bomb(X, Y, this);
			lock (MapLoader.MapInstance.ListOfBombs)
			{
				MapLoader.MapInstance.ListOfBombs.Add(bomb);
			}
			bomb.StartCountdown();
			Bombs++;
		}
		private void MoveExact(int x, int y)
		{
			float effectiveX = x / (ConstantValues.Power * 1f),
				  effectiveY = y / (ConstantValues.Power * 1f);

			var actualX = (int)Math.Floor((float)(x + (ConstantValues.Power / 2)) / ConstantValues.Power);
			var actualY = (int)Math.Floor((float)(y + (ConstantValues.Power / 2)) / ConstantValues.Power);
			var targets = GetHitTargets();
			var sourceLeft = effectiveX * MapLoader.MapInstance.TileSize;
			var sourceTop = effectiveY * MapLoader.MapInstance.TileSize;
			var sourceRect = new RectangleF(sourceLeft, sourceTop, MapLoader.MapInstance.TileSize, MapLoader.MapInstance.TileSize);
			var collisions = new List<Point>();
			var possible = new List<Point>();

			foreach (var t in targets)
			{
				int targetX = actualX + t.X,
					targetY = actualY + t.Y;

				var targetRect = new RectangleF(targetX * MapLoader.MapInstance.TileSize, targetY * MapLoader.MapInstance.TileSize, MapLoader.MapInstance.TileSize, MapLoader.MapInstance.TileSize);
				var movable = Movable(targetX, targetY);
				var intersects = sourceRect.IntersectsWith(targetRect);

				if (!movable && intersects)
				{
					collisions.Add(new Point(targetX, targetY));
				}
				else
				{
					possible.Add(new Point(targetX, targetY));
				}
			}

			if (collisions.Count == 0)
			{
				SetDirection(DirectionX, DirectionY);

				X = actualX;
				Y = actualY;

				ExactX = x;
				ExactY = y;
			}
			else
			{
				var candidates = new List<Tuple<int, int, Point>>();
				Tuple<int, int, Point> candidate = null;
				var p1 = new Point(actualX + DirectionX, actualY);
				var p2 = new Point(actualX, actualY + DirectionY);
				foreach (var nextMove in possible)
				{
					if (p1.Equals(nextMove))
					{
						candidates.Add(Tuple.Create(DirectionX, 0, p1));
					}

					if (p2.Equals(nextMove))
					{
						candidates.Add(Tuple.Create(0, DirectionY, p2));
					}
				}

				if (candidates.Count == 1)
				{
					candidate = candidates[0];
				}
				else if (candidates.Count == 2)
				{
					int minDistance = Int32.MaxValue;
					for (int i = 0; i < candidates.Count; ++i)
					{
						var targetCandidate = candidates[i];
						int xs = (ExactX - candidates[i].Item3.X * ConstantValues.Power);
						int ys = (ExactY - candidates[i].Item3.Y * ConstantValues.Power);
						int distance = xs * xs + ys * ys;

						if (distance < minDistance)
						{
							minDistance = distance;
							candidate = targetCandidate;
						}
					}
				}

				if (candidate != null)
				{
					var diffX = candidate.Item3.X * ConstantValues.Power - ExactX;
					var diffY = candidate.Item3.Y * ConstantValues.Power - ExactY;
					var absX = Math.Abs(diffX);
					var absY = Math.Abs(diffY);
					int effectiveDirectionX = 0;
					int effectiveDirectionY = 0;

					if (absX == 100)
					{
						effectiveDirectionX = 0;
					}
					else
					{
						effectiveDirectionX = Math.Sign(diffX);
					}

					if (absY == 100)
					{
						effectiveDirectionY = 0;
					}
					else
					{
						effectiveDirectionY = Math.Sign(diffY);
					}

					if (effectiveDirectionX == 0 && effectiveDirectionY == 0)
					{
						effectiveDirectionX = candidate.Item1;
						effectiveDirectionY = candidate.Item2;
					}

					SetDirection(effectiveDirectionX, effectiveDirectionY);

					ExactX += ConstantValues.Delta * effectiveDirectionX;
					X = actualX;

					ExactY += ConstantValues.Delta * effectiveDirectionY;
					Y = actualY;
				}
				else
				{
					var diffY = (collisions[0].Y * ConstantValues.Power - ExactY);
					var diffX = (collisions[0].X * ConstantValues.Power - ExactX);
					var absX = Math.Abs(diffX);
					var absY = Math.Abs(diffY);
					int effectiveDirectionX = 0;
					int effectiveDirectionY = 0;

					if (absX >= 35 && absX < 100)
					{
						effectiveDirectionX = -Math.Sign(diffX);
					}

					if (absY >= 35 && absY < 100)
					{
						effectiveDirectionY = -Math.Sign(diffY);
					}

					SetDirection(effectiveDirectionX, effectiveDirectionY);

					ExactX += ConstantValues.Delta * effectiveDirectionX;
					ExactY += ConstantValues.Delta * effectiveDirectionY;
				}
			}
		}
		protected bool Movable(int x, int y)
		{
			if (y >= 0 && y < MapLoader.MapInstance.Height && x >= 0 && x < MapLoader.MapInstance.Width)
			{
				if (MapLoader.MapInstance[x, y] == Tile.Grass)
				{
					return true;
				}
			}

			return false;
		}
		private void SetDirection(int x, int y)
		{
			if (x == -1)
			{
				Direction = Directions.WEST;
			}
			else if (x == 1)
			{
				Direction = Directions.EAST;
			}

			if (y == -1)
			{
				Direction = Directions.NORTH;
			}
			else if (y == 1)
			{
				Direction = Directions.SOUTH;
			}
		}

		private Point[] GetHitTargets()
		{
			return GetXHitTargets().Concat(GetYHitTargets()).ToArray();
		}

		private Point[] GetXHitTargets()
		{
			if (DirectionX == 1)
			{
				return EastTargets;
			}
			else if (DirectionX == -1)
			{
				return WestTargets;
			}

			return new Point[0];
		}

		private Point[] GetYHitTargets()
		{
			if (DirectionY == -1)
			{
				return NorthTargets;
			}
			else if (DirectionY == 1)
			{
				return SouthTargets;
			}

			return new Point[0];
		}
	}
}
