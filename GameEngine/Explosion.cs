using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using GameEngine.Common;
using GameEngine.Map;
using GameEngine.MapGenerator;

namespace GameEngine
{
	public class Explosion 
	{
		private int _lifeTime = 1000;
		private Timer _timer;
		private int _radius = 2;
		private readonly List<Point> _explosionPoints;
		public Explosion(List<Point> explosionPoints)
		{
			_explosionPoints = explosionPoints;
		}
		/// <summary>
		/// Запустить таймер, отсчитывающий время жизни взрыва
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
			foreach (var dangerPoint in _explosionPoints)
			{
				MapLoader.MapInstance[dangerPoint.X, dangerPoint.Y] = Tile.Grass;
				
			}
			MapLoader.MapInstance.PointsToExplode = _explosionPoints;
		}
		
	}
}
