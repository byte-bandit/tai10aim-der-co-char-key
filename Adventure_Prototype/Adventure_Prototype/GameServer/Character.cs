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
	public NetConnection Connection { get; set; }
	public Character(string name, int x, int y, NetConnection conn)
	{
		Name = name;
		X = x;
		Y = y;
		Connection = conn;
	}
}