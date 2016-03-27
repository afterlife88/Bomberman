using System.Collections.Generic;
using System.Drawing;
using GameEngine.GameObjects;

namespace GameEngine.Map
{
	public class Map
	{
		private readonly Tile[] _map;
		public int TileSize { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		/// <summary>
		/// Expected exploding points
		/// </summary>
		public List<Point> PointsToExplode { get; }
		/// <summary>
		/// List of current bomb on the map
		/// </summary>
		public List<Bomb> ListOfBombs { get; }
		public Map(string mapData, int width, int height, int tileSize)
		{
			_map = Create(mapData);
			Width = width;
			Height = height;
			TileSize = tileSize;
			PointsToExplode = new List<Point>();
			ListOfBombs = new List<Bomb>();
		}
		public Tile this[int x, int y]
		{
			get
			{
				return _map[GetIndex(x, y)];
			}
			set
			{
				_map[GetIndex(x, y)] = value;
			}
		}
		/// <summary>
		/// Check if player is on explosion in current tick
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool CheckExplosion(int x, int y)
		{
			foreach (var item in PointsToExplode.ToArray())
			{
				if (x == item.X && item.Y == y)
				{
					return true;
				}

			}
			return false;
		}
		private Tile[] Create(string map)
		{
			var tiles = new Tile[map.Length];
			for (int i = 0; i < map.Length; i++)
			{
				tiles[i] = (Tile)(map[i] - '0');
			}
			return tiles;
		}
		private int GetIndex(int x, int y)
		{
			return (y * Width) + x;
		}
	}
}
