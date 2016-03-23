using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GameEngine.Enums;
using GameEngine.Map;
using GameEngine.MapGenerator;

namespace GameEngine
{
	public class Bomb
	{
		private int _x;
		private int _y;
		private System.Timers.Timer _aTimer = new System.Timers.Timer();
		private Bomberman _currentBomberman;
		private Map.Map Map = MapLoader.GetMap;
		private int radius = 2;
		private int[][] Direction =
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

		}
		private void Explode(object source, ElapsedEventArgs e)
		{
			_currentBomberman.RemoveBomb();
			for (var i = 0; i < Direction.Length; i++)
			{
				var dir = Direction[i];
				for (var j = 1; j <= radius; j++)
				{
					var dx = dir[0] * j;
					var dy = dir[1] * j;
					var x = _x + dx;
					var y = _y + dy;
					if (CanDestroy(x, y))
					{
						Map[x, y] = Tile.Grass;
					}
					else
					{
						break;
					}
				}
			}
		}

		private bool CanDestroy(int x, int y)
		{
			var map = MapLoader.GetMap;
			var tile = map[x, y];
			return tile == Tile.Brick || tile == Tile.Grass;
		}
	}
}
