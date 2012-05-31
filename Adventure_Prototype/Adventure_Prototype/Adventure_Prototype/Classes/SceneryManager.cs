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
		private static Player player1;
		private static Player player2;


		public static Player Player1
		{
			get { return player1; }
			set { player1 = value; }
		}



		public static Player Player2
		{
			get { return player2; }
			set { player2 = value; }
		}


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
