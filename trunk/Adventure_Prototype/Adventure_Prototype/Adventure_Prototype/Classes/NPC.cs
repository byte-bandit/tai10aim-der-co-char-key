using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Classes.Pipeline;
namespace Classes
{
	 public class NPC : Character
	{
		private String _ID;
		private String _name;


		/// <summary>
		/// Creates a new Instance of NPC.
		/// </summary>
		/// <param name="game">The Game to which the Component is loaded.</param>
		public NPC(Game game, Room room, String id, String name, Animation animation=null, Texture2D sprite=null) : base(game, room, id, name, animation, sprite)
		{
			//Empty
		}


		//public NPC(Game game, Room room, String id, String name, Animation animation = null, String sprite = null)
		//    : base(game, room, id, name, animation, sprite)
		//{
		//    //Empty
		//}




		public void setGFX(String gfx)
		{
			this.gfx = GameRef.Game.Content.Load<Texture2D>(gfx);
		}



		/// <summary>
		/// Gets or sets the name of the NPC
		/// </summary>
		public String name
		{
			get { return this._name; }
			set { this._name = value; }
		}


		/// <summary>
		/// Gets or sets the ID of the NPC
		/// </summary>
		public String ID
		{
			get { return this._ID; }
			set { this._ID = value; }
		}


		/// <summary>
		/// Update Logic of the NPC
		/// </summary>
		/// <param name="gameTime">The elapsed gameTime</param>
		public override void Update(GameTime gameTime)
		{
			//Update Logic of NPC here
			base.Update(gameTime);
		}
	}
}
