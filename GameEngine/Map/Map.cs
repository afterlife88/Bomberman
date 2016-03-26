using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameEngine.Map
{
	public class Map
	{
		private readonly Tile[] _map;
		public int TileSize { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public List<Point> PointsToExplode { get; }
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
		public bool CheckExplosion(int x, int y)
		{
			return PointsToExplode.ToArray().Any(item => x == item.X && item.Y == y);
		}

		private Tile[] Create(string map)
		{
			var tiles = new Tile[map.Length];
			for (var i = 0; i < map.Length; i++)
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
