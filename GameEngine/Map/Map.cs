namespace GameEngine.Map
{
	public class Map
	{
		private readonly Tile[] _map;
		public int TileSize { get; private set; }
		public int Width { get; private set; }
		public int Height { get; private set; }
		public Map(string map, int width, int height, int tileSize)
		{
			_map = Create(map);
			Width = width;
			Height = height;
			TileSize = tileSize;
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
