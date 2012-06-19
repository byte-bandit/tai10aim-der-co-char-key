using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Classes.Net;

namespace Classes
{
	public class Player : Character
	{

		private int owner;

		public Player(Game game, Room room, String id, String name, Animation animation = null, Texture2D sprite = null, float scale = 1.0f)
			: base(game, room, id, name, animation, sprite, scale)
		{
			//this.gfxInfo = null;
		}



		public int Owner
		{
			get { return this.owner; }
			set { this.owner = value; }
		}
		
	}
}
