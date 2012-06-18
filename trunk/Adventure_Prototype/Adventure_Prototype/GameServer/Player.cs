using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameServer
{
	class Player
	{
		public int owner { get; set; }
		public int x { get; set; }
		public int y { get; set; }

		public Player(int Owner)
		{
			this.owner = Owner;
		}
	}
}
