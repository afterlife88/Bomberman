using System.Collections.Generic;
using System.Drawing;
using GameEngine.Common;
using GameEngine.Map;
using GameEngine.MapGenerator;
using Timer = System.Timers.Timer;

namespace GameEngine
{
	/// <summary>
	/// Bomb class
	/// </summary>
	public class Bomb
	{
		#region Fields
		private readonly int _x;
		private readonly int _y;
		private readonly List<Point> _dangerPoints;
		private Bomberman _caller;
		/// <summary>
		/// Detonation time
		/// </summary>
		private int _lifeTime = 3000;
		/// <summary>
		/// Timer
		/// </summary>
		private readonly Timer _timer;
		/// <summary>
		/// Radius of explosion
		/// </summary>
		private int _radius = 2;
		#endregion
		/// <summary>
		/// Default constructor	
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="bomberman">Bomberman who set bomb</param>
		public Bomb(int x, int y, Bomberman bomberman)
		{
			_x = x;
			_y = y;
			_caller = bomberman;
			_dangerPoints = GetDangerPoints();
			_timer = new Timer(_lifeTime);
			_timer.Elapsed += (parSender, parArgs) =>
			{
				Explode(_dangerPoints);
			};
			_timer.AutoReset = false;
		}
		/// <summary>
		/// Run countdown of bomb 
		/// </summary>
		public void StartCountdown()
		{
			_timer.Start();
		}
		/// <summary>
		/// Get point that destroyed at explosion
		/// </summary>
		/// <returns></returns>
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
					// If points destroyable, then add destroy points list
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
	
		/// <summary>
		/// Make explosion
		/// </summary>
		/// <param name="dangerPoints"></param>
		private void Explode(List<Point> dangerPoints)
		{
			lock (MapLoader.MapInstance.PointsToExplode)
			{
				MapLoader.MapInstance.ListOfBombs.Clear();
				MapLoader.MapInstance.PointsToExplode.AddRange(dangerPoints);
				_caller.RemoveBomb();
				var explosion = new Explosion(dangerPoints);
				explosion.InitTimer();
			}
		}
	}
}
