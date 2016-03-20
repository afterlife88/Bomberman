using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BomberManUAWC.Startup))]

namespace BomberManUAWC
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();			
		}
	}
}
