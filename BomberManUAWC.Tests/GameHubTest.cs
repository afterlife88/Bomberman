using BomberManUAWC.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using Xunit;


namespace BomberManUAWC.Tests
{
	public class GameHubTest
	{
		[Fact]
		public void GetMapForClient_Succesfull()
		{
			bool sendCalled = false;
			var context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
			var mockClients = new Mock<IHubCallerConnectionContext<string>>();
		
			string all = null;
			//context.OnConnected();
			mockClients.Setup(m => m.Caller).Returns(all);
			

		}

	}
}
