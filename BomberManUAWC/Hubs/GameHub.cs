﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Common;
using GameEngine.Enums;
using GameEngine.Map;
using GameEngine.MapGenerator;
using Microsoft.AspNet.SignalR;

namespace BomberManUAWC.Hubs
{
	/// <summary>
	/// Main hub to manage client movement
	/// </summary>
	public class GameHub : Hub
	{
		private static PlayerState _currentPlayerState;
		private static List<EnemyState> _enemyStates;
		private Map Map = MapLoader.GetMap;
		private static int _gameLoopRunning;
		private List<State> allActiveObjects = new List<State>();

		public override Task OnConnected()
		{
			_currentPlayerState = SetNewPlayerState();
			_enemyStates = SetNewEnemyState();
			allActiveObjects.Add(_currentPlayerState);
			// add all enemys
			allActiveObjects.AddRange(_enemyStates);
			// Initialize player who connected with map
			Clients.Caller.initializeMap(MapLoader.GetMapData).Wait();
			// Run loop in new thread
			EnsureGameLoop();
			// Initialize player client call
			Clients.Caller.initializePlayer(_currentPlayerState.Player).Wait();
			// Initialize bots
			return Clients.Caller.initialize(_enemyStates);
		}

		private void EnsureGameLoop()
		{
			if (Interlocked.Exchange(ref _gameLoopRunning, 1) == 0)
			{
				new Thread(_ => RunGameLoop()).Start();
			}
		}

		/// <summary>
		/// Recive keys(movement) from client
		/// Set it to state of player
		/// </summary>
		/// <param name="inputs"></param>
		public void SendKeys(KeyboardState[] inputs)
		{
			lock (_currentPlayerState)
			{
				foreach (var input in inputs)
				{
					_currentPlayerState.Inputs.Enqueue(input);
				}
			}
		}

		private void RunGameLoop()
		{
			var frameTicks = (int)Math.Round(1000.0 / ConstantValues.Fps);
			var context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
			var lastUpdate = Environment.TickCount;

			while (true)
			{
				var delta = (lastUpdate + frameTicks) - Environment.TickCount;
				if (delta < 0)
				{
					lastUpdate = Environment.TickCount;
					Update(context);
				}
				else
				{
					Thread.Sleep(TimeSpan.FromTicks(delta));
				}
			}
		}

		/// <summary>
		/// Persistance connection with client to update state of player on server
		/// </summary>
		/// <param name="context"></param>
		private void Update(IHubContext context)
		{
			if (_currentPlayerState != null)
			{
				KeyboardState input;
				if (_currentPlayerState.Inputs.TryDequeue(out input))
				{
					_currentPlayerState.Player.Update(input);
					context.Clients.All.updatePlayerState(_currentPlayerState.Player);
				}
			}
			if (_enemyStates.Count > 0)
			{
				for (int i = 0; i < _enemyStates.Count; i++)
				{
					if (Map.CheckExplosion(_enemyStates[i].Enemy.X, _enemyStates[i].Enemy.Y))
					{
						_enemyStates.Remove(_enemyStates[i]);
						Map.PointsToExplode.Clear();
						continue;
					}
					var input = _enemyStates[i].Enemy.GetNextMove(_currentPlayerState);
					_enemyStates[i].Enemy.Update(input);
				}
		
				// Update enemies on clientclient.updateEnemyStates
				context.Clients.All.updateEnemyStates(_enemyStates);
			}
		
		}


		/// <summary>
		/// Disconnect behavior 
		/// </summary>
		/// <param name="stopCalled"></param>
		/// <returns></returns>
		public override Task OnDisconnected(bool stopCalled)
		{
			Clients.All.playerLeft(_currentPlayerState.Player);
			_currentPlayerState = null;

			return base.OnDisconnected(stopCalled);
		}

		/// <summary>
		/// Singleton new player state
		/// </summary>
		/// <returns></returns>
		private static PlayerState SetNewPlayerState()
		{
			if (_currentPlayerState == null)
			{
				var player = new Player();
				var initialPosition = new Point(1, 1);

				player.Index = 0;
				player.X = initialPosition.X;
				player.Y = initialPosition.Y;
				player.ExactX = initialPosition.X * ConstantValues.Power;
				player.ExactY = initialPosition.Y * ConstantValues.Power;
				player.Direction = Direction.SOUTH;

				return new PlayerState { Player = player, Inputs = new ConcurrentQueue<KeyboardState>() };
			}
			return _currentPlayerState;
		}
		/// <summary>
		/// Init position of bots
		/// </summary>
		/// <returns></returns>
		private static List<EnemyState> SetNewEnemyState()
		{
			var listOfStates = new List<EnemyState>();
			var initialEnemyPositions = new Point[3];
			initialEnemyPositions[0] = new Point(13, 1);
			initialEnemyPositions[1] = new Point(1, 11);
			initialEnemyPositions[2] = new Point(13, 11);
			for (int i = initialEnemyPositions.Length - 1; i >= 0; i--)
			{
				listOfStates.Add(new EnemyState()
				{
					Enemy = new Enemy()
					{
						Index = i + 1,
						X = initialEnemyPositions[i].X,
						Y = initialEnemyPositions[i].Y,
						ExactX = initialEnemyPositions[i].X * ConstantValues.Power,
						ExactY = initialEnemyPositions[i].Y * ConstantValues.Power,
						Direction = Direction.SOUTH
					},
					Inputs = new ConcurrentQueue<KeyboardState>()
				});
			}
			return listOfStates;
		}
	}
}

