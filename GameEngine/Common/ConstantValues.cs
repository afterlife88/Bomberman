namespace GameEngine.Common
{
	public static class ConstantValues
	{
		public const int Power = 100;
		public const int Delta = 10;
		public const int Fps = 60;

		public const int Width = 15;
		public const int Height = 13;
		public const int TileSize = 40;

		public static readonly int[][] MapArray = {
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
		public static readonly int[][] ExsplosionDirections = {
			new[] {1, 0},
			new[] {0, 1},
			new[] {-1, 0},
			new[] {0, -1}
		};
	}
}
