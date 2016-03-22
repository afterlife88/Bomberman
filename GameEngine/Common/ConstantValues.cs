﻿namespace GameEngine.Common
{
	public static class ConstantValues
	{
		public const int Power = 100;
		public const int Delta = 10;
		public const int Fps = 60;

		public static int Width = 15;
		public static int Height = 13;
		public static int TileSize = 40;

		public static string MapData { get; } = "222222222222222" +
												"203000030000002" +
												"202020202020202" +
												"200000000000002" +
												"202020202020202" +
												"200000000000002" +
												"202020202020202" +
												"200000000000002" +
												"202020202020202" +
												"200000000000002" +
												"202020202020202" +
												"200000000000002" +
												"222222222222222";
		public static int[][] MapArray = {
				new[] {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,0,0,0,0,0,0,0,0,0,0,0,0,0,2},
			new[] {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
		};
	}
}
