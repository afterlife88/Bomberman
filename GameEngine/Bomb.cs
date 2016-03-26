using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using GameEngine.Common;
using GameEngine.Map;
using GameEngine.MapGenerator;

namespace GameEngine
{
	public class Bomb
	{
		private readonly int _x;
		private readonly int _y;
		private readonly List<Point> _dangerPoints;
		private Bomberman _caller;
	
		/// <summary>
		/// Время до детонации 
		/// </summary>
		private int _lifeTime = 3000;

		/// <summary>
		/// Таймер
		/// </summary>
		private Timer _timer;

		private int _radius = 2;

		public Bomb(int x, int y, Bomberman bomberman)
		{
			_x = x;
			_y = y;
			_caller = bomberman;
			MapLoader.MapInstance.ListOfBombs.Add(this);
			_dangerPoints = GetDangerPoints();
			_timer = new Timer(_lifeTime);
			_timer.Elapsed += (parSender, parArgs) =>
			{
				Explode(_dangerPoints);
			};
			_timer.AutoReset = false;
		}

		private List<Point> GetDangerPoints()
		{
			var dangerPoints = new List<Point>();
			foreach (var dir in ConstantValues.ExsplosionDirections)
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
		/// <summary>
		/// Check if bomb can destroy objects of map
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private bool CanDestroy(int x, int y)
		{
			var tile = MapLoader.MapInstance[x, y];
			return tile == Tile.Brick || tile == Tile.Grass;
		}
		public Point Location => new Point(_x, _y);
		/// <summary>
		/// Run countdown of bomb 
		/// </summary>
		public void StartCountdown()
		{
			_timer.Start();
		}
		private void Explode(List<Point> dangerPoints)
		{
			MapLoader.MapInstance.ListOfBombs.Clear();
			_caller.RemoveBomb();
			var explosion = new Explosion(dangerPoints);
			explosion.InitTimer();
		}
	}
}
