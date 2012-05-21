using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Classes
{
	class Player : Character
	{

		public Player(Game game, Room room, String id, String name, Animation animation = null, Texture2D sprite = null)
			: base(game, room, id, name, animation, sprite)
		{
			//this.gfxInfo = null;
		}
		
	}
}
