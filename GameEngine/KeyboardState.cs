using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Enums;

namespace GameEngine
{
	/// <summary>
	/// State of player
	/// </summary>
	public class KeyboardState
	{
		private readonly Dictionary<Keys, bool> _keyState;

		public int Id { get; set; }

		public KeyboardState(Dictionary<Keys, bool> keyState, int id)
		{
			_keyState = keyState;
			Id = id;
		}
		public bool this[Keys key] => _keyState[key];
	}
}
