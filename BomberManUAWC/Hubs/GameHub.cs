using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace BomberManUAWC.Hubs
{
	public class GameHub : Hub
	{
		public override Task OnConnected()
		{
			// Initialize player who connected
			Clients.Caller.initializeMap().Wait();
			return base.OnConnected();
		}
	}
}
