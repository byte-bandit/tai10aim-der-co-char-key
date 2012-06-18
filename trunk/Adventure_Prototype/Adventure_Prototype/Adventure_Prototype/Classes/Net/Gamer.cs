using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace Classes.Net
{
	class Gamer
	{
		public int X { get; set; }
		public int Y { get; set; }
		public string Name { get; set; }
		public bool ready { get; set; }
		public int GamerNumber { get; set; }
		public Player puppet { get; set; }
		public NetConnection Connection { get; set; }
		public Gamer(string name)
		{
			Name = name;
		}
	}
}
