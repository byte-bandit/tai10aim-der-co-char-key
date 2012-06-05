using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;


class Character
{
	public int X { get; set; }
	public int Y { get; set; }
	public string Name { get; set; }
	public bool ready { get; set; }
	public NetConnection Connection { get; set; }
	public Character(string name, NetConnection conn)
	{
		Name = name;
		Connection = conn;
	}
}