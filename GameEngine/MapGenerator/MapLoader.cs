using System;
using System.Linq;
using GameEngine.Common;
using GameEngine.Map;

namespace GameEngine.MapGenerator
{
	public sealed class MapLoader
	{
		private static volatile Map.Map _instance;
		private static readonly object SyncRoot = new object();
		private static  string _generatedStringMap;

		private static int[][] GenerateMap(int[][] ar)
		{
			Random rand = new Random();
			for (int i = 1; i < ar.Length - 1; i++)
			{
				for (int j = 1; j < ar[i].Length - 1; j++)
				{
					if (i % 2 == 0 && j % 2 == 0)
						ar[i][j] = (int)Tile.Wall;
					//if ((ar[1][1] && ar[13][1] && ar[13][11] && ar[1][11]) != 0)
					//{
						
					//}
					else if (rand.Next(6) == 5)
						ar[i][j] = (int)Tile.Brick;
				}
			}
			return ar;
		}

		public static string GetMapData
		{
			get
			{
				if (_generatedStringMap == null)
				{
					_generatedStringMap = ArrToString(GenerateMap(ConstantValues.MapArray));;
				}
				return _generatedStringMap;;
			}
		}
		public static Map.Map GetMap
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new Map.Map(GetMapData, ConstantValues.Width, ConstantValues.Height, ConstantValues.TileSize);
					}
				}

				return _instance;
			}
		}
		private static string ArrToString(int[][] ar)
		{
			return string.Join("", ar.Select(arr => string.Join("", arr)));
		}
	}
}