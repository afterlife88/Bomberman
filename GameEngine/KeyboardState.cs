﻿using System;
using System.Collections.Generic;
using System.Text;
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
		//public KeyboardState(Dictionary<Keys, bool> keyState)
		//{
		//	_keyState = keyState;
		//}
		public bool this[Keys key] => _keyState[key];

		public bool Empty
		{
			get
			{
				return !this[Keys.SPACE] &&
				       !this[Keys.DOWN] &&
				       !this[Keys.LEFT] &&
				       !this[Keys.RIGHT] &&
				       !this[Keys.UP];
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var value in Enum.GetValues(typeof(Keys)))
			{
				if (sb.Length > 0)
				{
					sb.Append(", ");
				}

				sb.Append(Enum.GetName(typeof(Keys), value))
				  .Append(" = ")
				  .Append(this[(Keys)value]);
			}

			sb.AppendLine();

			return sb.ToString();
		}
	}
}
