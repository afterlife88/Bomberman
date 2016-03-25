using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using GameEngine.Map;
using GameEngine.MapGenerator;

namespace GameEngine
{
	public class Bomb
	{
		private readonly int _x;
		private readonly int _y;
		private readonly Timer _aTimer = new Timer();
		private readonly Bomberman _currentBomberman;
		private readonly Map.Map _map = MapLoader.GetMap;
		private static List<Bomb> listOfBombs = new List<Bomb>(); 
		private int _radius = 2;
		private readonly int[][] _direction =
		{
			new[] {1, 0},
			new[] {0, 1},
			new[] {-1, 0},
			new[] {0, -1}
		};
		public Bomb(int x, int y, Bomberman bomberman)
		{
			_x = x;
			_y = y;
			_currentBomberman = bomberman;
			_aTimer.Elapsed += Explode;
			_aTimer.Interval = 3000;
			_aTimer.Enabled = true;
			listOfBombs.Add(this);
		}

		public Point Location => new Point(_x,_y);

		public static List<Bomb> Bombs => listOfBombs;

		public List<Point> GetDangerPoints()
		{
			var dangerPoints = new List<Point>();
			foreach (var dir in _direction)
			{
				for (var j = 1; j <= _radius; j++)
				{
					var dx = dir[0] * j;
					var dy = dir[1] * j;
					var x = _x + dx;
					var y = _y + dy;
					if (CanDestroy(x, y))
					{
						dangerPoints.Add(new Point(x, y));
					}
					else
					{
						break;
					}
				}
			}
			return dangerPoints;
		}

		private void Explode(object source, ElapsedEventArgs e)
		{
			var dangerPoints = GetDangerPoints();
			foreach (var dangerPoint in dangerPoints)
			{
				_map[dangerPoint.X, dangerPoint.Y] = Tile.Grass;
				_map.PointsToExplode.Add(dangerPoint);
			}
			listOfBombs.Remove(this);
			_currentBomberman.RemoveBomb();
		}
		private bool CanDestroy(int x, int y)
		{
			var tile = _map[x, y];
			return tile == Tile.Brick || tile == Tile.Grass;
		}
	}
}
