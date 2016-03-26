using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
		private bool _deadBot;
		private int _idToRemove;
		private int _gameLoopRunning;
		//private List<State> allActiveObjects = new List<State>();

		public override Task OnConnected()
		{
			_currentPlayerState = SetNewPlayerState();
			_enemyStates = SetNewEnemyState();
			// Initialize player who connected with map
			Clients.Caller.initializeMap(MapLoader.MapData).Wait();
			// Run loop in new thread
			EnsureGameLoop();
			// Initialize player client call
			Clients.Caller.initialize(_enemyStates);

			// Initialize bots
			return Clients.Caller.initializePlayer(_currentPlayerState.Player);
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

		public override Task OnReconnected()
		{
			_currentPlayerState = SetNewPlayerState();
			_enemyStates = SetNewEnemyState();
			// Initialize player who connected with map
			Clients.Caller.initializeMap(MapLoader.MapData).Wait();
			// Run loop in new thread
			EnsureGameLoop();
			// Initialize player client call
			Clients.Caller.initialize(_enemyStates);

			// Initialize bots
			return Clients.Caller.initializePlayer(_currentPlayerState.Player);
		}

		/// <summary>
		/// Persistance connection with client to update state of player on server
		/// </summary>
		/// <param name="context"></param>
		private void Update(IHubContext context)
		{
			_deadBot = false;
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
				foreach (var enemyState in _enemyStates)
				{
					if (MapLoader.MapInstance.CheckExplosion(enemyState.Enemy.X, enemyState.Enemy.Y))
					{
						_deadBot = true;
						_idToRemove = enemyState.Enemy.Index;
						Debug.WriteLine("explosion {0}", MapLoader.MapInstance.PointsToExplode.Capacity);
						continue;
					}
					var input = enemyState.Enemy.GetNextMove();
					enemyState.Enemy.Update(input);
				}
				if (_deadBot)
					_enemyStates.RemoveAll(r => r.Enemy.Index == _idToRemove);
				// Update enemies on clientclient.updateEnemyStates
				Debug.WriteLine(_enemyStates.Count);
				MapLoader.MapInstance.PointsToExplode.Clear();
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
			//Clients.All.playerLeft(_currentPlayerState.Player);
			_currentPlayerState = null;
			MapLoader.MapData = null;
			MapLoader.MapInstance = null;
			_enemyStates.Clear();
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

