using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Common;
using GameEngine.Enums;
using Microsoft.AspNet.SignalR;

namespace BomberManUAWC.Hubs
{
	public class GameHub : Hub
	{
		private static PlayerState _currentPlayerState = GetPlayer();
		private static Point _initialPosition;
		private string _connectionId;
		private ConcurrentStack<Bomberman> allActiveObjects = new ConcurrentStack<Bomberman>();
		public override Task OnConnected()
		{
			_connectionId = Context.ConnectionId;
			// Initialize player who connected
			Clients.Caller.initializeMap(ConstantValues.MapData).Wait();
			EnsureGameLoop();
			Clients.Caller.initializePlayer(_currentPlayerState.Player).Wait();

			return Clients.Caller.initialize(_currentPlayerState.Player);
		}
		private void EnsureGameLoop()
		{
			new Thread(_ => RunGameLoop()).Start();
		}
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
			var frameTicks = (int)Math.Round(1000.0 / ConstantValues.FPS);
			var context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
			var lastUpdate = 0;

			while (true)
			{
				int delta = (lastUpdate + frameTicks) - Environment.TickCount;
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
		private void Update(IHubContext context)
		{
			KeyboardState input;
			if (_currentPlayerState.Inputs.TryDequeue(out input))
			{
				_currentPlayerState.Player.Update(input);
				context.Clients.Client(_connectionId).updatePlayerState(_currentPlayerState.Player);
			}
		}
		private static PlayerState GetPlayer()
		{
			if (_currentPlayerState == null)
			{
				Player player = new Player();
				_initialPosition = new Point(1, 1);

				player.Index = 0;
				player.X = _initialPosition.X;
				player.Y = _initialPosition.Y;
				player.ExactX = _initialPosition.X*ConstantValues.POWER;
				player.ExactY = _initialPosition.Y*ConstantValues.POWER;
				player.Direction = Direction.SOUTH;

				return new PlayerState() {Player = player, Inputs = new ConcurrentQueue<KeyboardState>()};
			}
			return _currentPlayerState;
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			_currentPlayerState = null;

			return base.OnDisconnected(stopCalled);
		}
	}
}
