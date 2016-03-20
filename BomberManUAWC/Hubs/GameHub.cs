using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace BomberManUAWC.Hubs
{
	public class GameHub : Hub
	{
		
		public override Task OnConnected()
		{
			Clients.Caller.initializeMap().Wait();
			return base.OnConnected();
		}
	}
}
