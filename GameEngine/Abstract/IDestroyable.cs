using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Abstract
{
	public interface IDestroyable
	{
		/// <summary>
		/// Событие о необходимости уничтожения объекта
		/// </summary>
		event EventHandler DestroyMeEvent;
		/// <summary>
		/// Вызывает событие уничтожения
		/// </summary>
		void DestroyMe();
	}
}
