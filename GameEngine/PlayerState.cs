using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
	class PlayerState
	{
		public Queue<KeyboardState> Inputs { get; set; } 
		public Player Player { get; set; }
	}
}
