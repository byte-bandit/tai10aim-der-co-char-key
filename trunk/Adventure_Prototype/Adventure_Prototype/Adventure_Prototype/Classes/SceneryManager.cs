using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Classes;
using Classes.Graphics;

namespace Classes
{
	class SceneryManager
	{
		private static Room currentRoom;

		public static Room CurrentRoom
		{
			get { return currentRoom; }
			set 
			{
				GraphicsManager.removeChild(currentRoom);
				UpdateManager.removeItem(currentRoom);
				currentRoom = value;
				GraphicsManager.addChild(currentRoom);
				UpdateManager.addItem(currentRoom);
			}
		}
	}
}
