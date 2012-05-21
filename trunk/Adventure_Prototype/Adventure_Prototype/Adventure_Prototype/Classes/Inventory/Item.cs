using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

using Classes.Pipeline;
using Classes.Graphics;

namespace Classes.Inventory
{
	class Item :DrawableGameComponent
	{
		private Vector2 position;
		private Texture2D image;
		private String id;

		public Item(int X, int Y, Texture2D Image, String ID) : base(GameRef.Game)
		{
			this.position = new Vector2(X, Y);
			this.image = Image;
			this.id = ID;
		}
		public String ID
		{
			get { return this.id; }
		}

		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}

		public int totalWidth
		{
			get{ return (int)this.position.X + this.image.Width;}
		}

		public int totalHeight
		{
			get { return (int)this.position.Y + this.image.Height; }
		}

		public bool GetCollision(int X, int Y)
		{
			if ((X >= this.position.X) && (Y >= this.position.Y) && (X <= this.totalWidth) && (Y <= this.totalHeight))
			{
				return true;
			}
			return false;
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Draw(this.image, this.Position, Color.White);
			base.Draw(gameTime);
		}

	}
}
