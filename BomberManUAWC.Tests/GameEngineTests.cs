using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using GameEngine;
using GameEngine.Common;
using GameEngine.Enums;
using GameEngine.GameObjects;
using GameEngine.GameStates;
using GameEngine.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BomberManUAWC.Tests
{
	[TestClass]
	public class GameEngineTests
	{
		[TestMethod]
		public void Get_MapString_Succesfull()
		{
			var map = MapLoader.MapData;
			Assert.IsNotNull(map);
		}
		[TestMethod]
		public void Check_Blocks_Are_Not_SpawnAtPlayersPositions_Succesfull()
		{
			var map = MapLoader.MapInstance;
			var initialPositions = new Point[4];
			initialPositions[0] = new Point(1, 1);
			initialPositions[1] = new Point(13, 1);
			initialPositions[2] = new Point(1, 11);
			initialPositions[3] = new Point(13, 11);
			bool tileIsClearForPlayer = false;
			for (int i = initialPositions.Length - 1; i >= 0; i--)
			{
				if (map[initialPositions[i].X, initialPositions[i].Y] == Tile.Grass)
					tileIsClearForPlayer = true;
			}
			Assert.IsTrue(tileIsClearForPlayer);
		}
		[TestMethod]
		public void Check_Moves_Of_Playerl()
		{
			int startX = 1,
				startY = 1,
				startExactX = 100,
				startExactY = 100;
			var player = new Player
			{
				X = startX,
				Y = startY,
				ExactX = startExactX,
				ExactY = startExactY
			};
			var rightState = GetDownKeyboardState(Keys.RIGHT);
			for (int i = 0; i < ConstantValues.CountToMoveOnActualPosition; i++)
			{
				player.Update(rightState);
			}
			Assert.AreNotEqual(startX, player.X);
		}
		[TestMethod]
		public void Check_BombPlanted_Succesfull()
		{
			int bombsPlantedBefore = 0;
			var player = new Player
			{
				X = 1,
				Y = 1,
				ExactX = 100,
				ExactY = 100,
				Bombs = bombsPlantedBefore
			};
			var plantBombState = GetDownKeyboardState(Keys.SPACE);
			player.Update(plantBombState);
			Assert.AreNotEqual(bombsPlantedBefore, player.Bombs);
			// Check that this bomb added to all bombs on map
			Assert.AreEqual(1, MapLoader.MapInstance.ListOfBombs.Count);
		}
		[TestMethod]
		public void Check_Explosion_Vector_Kill_Unit_After_Bomb()
		{
			int bombsPlantedBefore = 0;
			var player = new Player
			{
				X = 1,
				Y = 1,
				ExactX = 100,
				ExactY = 100,
				Bombs = bombsPlantedBefore
			};
			var plantBombState = GetDownKeyboardState(Keys.SPACE);
			player.Update(plantBombState);
			var rightState = GetDownKeyboardState(Keys.RIGHT);
			player.Update(plantBombState);
			for (int i = 0; i < ConstantValues.CountToMoveOnActualPosition; i++)
			{
				// Move right by X
				player.Update(rightState);
			}
			// Wait explosion
			Thread.Sleep(3100);
			// Check count of bomb after explosion
			Assert.AreEqual(bombsPlantedBefore, player.Bombs);
			// Check explosion points added to list
			Assert.AreNotEqual(0, MapLoader.MapInstance.PointsToExplode.Count);
			// Check explosion are in player position
			Assert.IsTrue(MapLoader.MapInstance.CheckExplosion(player.X, player.Y));
		}
		private static KeyboardState GetDownKeyboardState(Keys key)
		{
			var dict = new Dictionary<Keys, bool>();
			foreach (var v in Enum.GetValues(typeof(Keys)))
			{
				dict[(Keys)v] = false;
			}

			dict[key] = true;

			return new KeyboardState(dict, 0);
		}
	}
}
