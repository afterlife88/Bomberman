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
		private static string _generatedStringMap;

		private static int[][] GenerateMap(int[][] ar)
		{
			var tempAr = ar.Select(array => array.Select(e => e).ToArray()).ToArray();
			var rand = new Random();
			for (int i = 1; i < tempAr.Length - 1; i++)
			{
				for (int j = 1; j < tempAr[i].Length - 1; j++)
				{
					if (i % 2 == 0 && j % 2 == 0)
						tempAr[i][j] = (int)Tile.Wall;
					
					else if (rand.Next(3) == 2)
					{
						
						if ((i == 1 && (j == 1 || j == 2 || j == tempAr[i].Length - 2 || j == tempAr[i].Length - 3)) ||
						    (i == 2 && (j == 1 || j == tempAr[i].Length - 2)) ||
						    (i == tempAr.Length - 2 && (j == 1 || j == 2 || j == tempAr[i].Length - 2 || j == tempAr[i].Length - 3)) ||
						    (i == tempAr.Length - 3 && (j == 1 || j == tempAr[i].Length - 2)))
						{
							tempAr[i][j] = (int) Tile.Grass;
						}
						else
						{
							tempAr[i][j] = (int)Tile.Brick;
						}
						
					}
				}
			}
			return tempAr;
		}

		public static string MapData
		{
			get
			{
				if (_generatedStringMap == null)
				{
					_generatedStringMap = ArrToString(GenerateMap(ConstantValues.MapArray));
				}
				return _generatedStringMap;
			}
			set { _generatedStringMap = value; }
		}
		public static Map.Map MapInstance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new Map.Map(MapData, ConstantValues.Width, ConstantValues.Height, ConstantValues.TileSize);
					}
				}
				return _instance;
			}
			set { _instance = value; }
			
		}
		private static string ArrToString(int[][] ar)
		{
			return string.Join("", ar.Select(arr => string.Join("", arr)));
		}
	}
}