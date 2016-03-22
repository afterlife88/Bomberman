using System;
using GameEngine.Common;

namespace GameEngine.MapGenerator
{
	public sealed class MapLoader
	{
		private static volatile Map.Map _instance;
		private static object syncRoot = new Object();

		public string GenerateMap(string mapWithoutBricks)
		{
			var array = mapWithoutBricks.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
					
			}
			string s = new string(array);
			return s;
		}
		public static Map.Map GetMap
		{
			get
			{
				if (_instance == null)
				{
					lock (syncRoot)
					{
						if (_instance == null)
							_instance = new Map.Map(ConstantValues.MapData, ConstantValues.Width, ConstantValues.Height, ConstantValues.TileSize);
					}
				}

				return _instance;
			}
		}
	}
}