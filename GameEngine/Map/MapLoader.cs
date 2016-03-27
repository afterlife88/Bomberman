using System;
using System.Linq;
using GameEngine.Common;

namespace GameEngine.Map
{
	/// <summary>
	/// Class to manage map
	/// </summary>
	public static class MapLoader
	{
		private static volatile Map _instance;
		private static readonly object SyncRoot = new object();
		private static string _generatedStringMap;
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
		public static Map MapInstance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new GameEngine.Map.Map(MapData, ConstantValues.Width, ConstantValues.Height, ConstantValues.TileSize);
					}
				}
				return _instance;
			}
			set { _instance = value; }
			
		}
		/// <summary>
		/// Generating field
		/// </summary>
		/// <param name="ar"></param>
		/// <returns></returns>
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
					// make bricks on 25 % of map
					else if (rand.Next(4) == 3)
					{
						// ingore first points by x and y with bomberman init position
						if ((i == 1 && (j == 1 || j == 2 || j == tempAr[i].Length - 2 || j == tempAr[i].Length - 3)) ||
							(i == 2 && (j == 1 || j == tempAr[i].Length - 2)) ||
							(i == tempAr.Length - 2 && (j == 1 || j == 2 || j == tempAr[i].Length - 2 || j == tempAr[i].Length - 3)) ||
							(i == tempAr.Length - 3 && (j == 1 || j == tempAr[i].Length - 2)))
						{
							tempAr[i][j] = (int)Tile.Grass;
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
		/// <summary>
		/// Convert string map
		/// </summary>
		/// <param name="ar"></param>
		/// <returns></returns>
		private static string ArrToString(int[][] ar)
		{
			return string.Join("", ar.Select(arr => string.Join("", arr)));
		}
	}
}