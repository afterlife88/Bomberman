using System;
using System.Collections.Concurrent;
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
		private PlayerState _currentPlayerState = GetPlayer();
		private static Point _initialPosition;
		private ConcurrentStack<Bomberman> allActiveObjects = new ConcurrentStack<Bomberman>();
		public override Task OnConnected()
		{
			// Initialize player who connected
			Clients.Caller.initializeMap(ConstantValues.MapData).Wait();

			EnsureGameLoop();
			return Clients.Caller.initializePlayer(_currentPlayerState.Player).Wait();
		}
		private void EnsureGameLoop()
		{
			new Thread(_ => RunGameLoop()).Start();
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
				context.Clients.All.updatePlayerState(_currentPlayerState.Player);
			}
		}
		private static PlayerState GetPlayer()
		{
			Player player = new Player();
			_initialPosition = new Point(1, 1);

			player.Index = 0;
			player.X = _initialPosition.X;
			player.Y = _initialPosition.Y;
			player.ExactX = _initialPosition.X * ConstantValues.POWER;
			player.ExactY = _initialPosition.Y * ConstantValues.POWER;
			player.Direction = Direction.South;
			return new PlayerState() {Player = player};
		}
		//public override Task OnDisconnected(bool stopCalled)
		//{
		//	_currentPlayer = null;

		//	return base.OnDisconnected(stopCalled);
		//}
	}
}
