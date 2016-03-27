using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Timers;
using GameEngine.Map;

namespace GameEngine.GameObjects
{
	public class Explosion
	{
		private int _lifeTime = 500;
		private Timer _timer;
		private readonly List<Point> _explosionPoints;
		public Explosion(List<Point> explosionPoints)
		{
			_explosionPoints = explosionPoints;
		}
		/// <summary>
		/// Timer to init explosion
		/// </summary>
		public void InitTimer()
		{
			_timer = new Timer(_lifeTime);
			_timer.Elapsed += (parSender, parArgs) =>
			{
				Explode();
			};
			_timer.AutoReset = false;
			_timer.Start();
		}
		private void Explode()
		{
			lock (MapLoader.MapInstance.PointsToExplode)
			{
				Debug.WriteLine("BombExplode");
				foreach (var dangerPoint in _explosionPoints)
				{
					MapLoader.MapInstance[dangerPoint.X, dangerPoint.Y] = Tile.Grass;
					MapLoader.MapInstance.PointsToExplode.Remove(dangerPoint);
				}
			}
		}

	}
}
