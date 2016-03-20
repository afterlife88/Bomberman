using System;
using GameEngine.Common;

namespace GameEngine.MapGenerator
{
	public sealed class MapLoader
	{
		private static volatile Map.Map _instance;
		private static object syncRoot = new Object();
		//static string mapData = "222222222222222" +
		//                        "200000000000002" +
		//                        "202020202020202" +
		//                        "200000000000002" +
		//                        "202020202020202" +
		//                        "200000000000002" +
		//                        "202020202020202" +
		//                        "200000000000002" +
		//                        "202020202020202" +
		//                        "200000000000002" +
		//                        "200020202020202" +
		//                        "200000000000002" +
		//                        "222222222222222";

		private static int width = 15;
		private static int height = 13;
		private static int tileSize = 40;

		public static Map.Map GetMap
		{
			get
			{
				if (_instance == null)
				{
					lock (syncRoot)
					{
						if (_instance == null)
							_instance = new Map.Map(ConstantValues.MapData, width, height, tileSize);
					}
				}

				return _instance;
			}
		}
	}
}