﻿using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

using Classes.Pipeline;
using Classes.Graphics;

namespace Classes.Inventory
{
	public class Item :DrawableGameComponent
	{
		public delegate void FiredEvent(object sender);

		// Instances of delegate event.
		public FiredEvent OnMouseOver;
		public FiredEvent OnMouseOut;
		public FiredEvent OnMouseClick;

		private Vector2 position;
		private Texture2D image;
		private String id;
		private Texture2D tooltip;

		#region Properties
		public Vector2 Position
		{
			get { return position; }
			set { this.position = value; }
		}
		public Texture2D Image
		{
			get { return image; }
			set { this.image = value; }
		}
		public Texture2D Tooltip
		{
			get { return tooltip; }
			set { this.tooltip = value; }
		}
		public String ID
		{
			get { return this.id; }
		}

		public int totalWidth
		{
			get{ return (int)this.position.X + this.image.Width;}
		}

		public int totalHeight
		{
			get { return (int)this.position.Y + this.image.Height; }
		}
		#endregion

		public Item(int X, int Y, Texture2D Image, String ID)
			: base(GameRef.Game)
		{
			this.position = new Vector2(X, Y);
			this.image = Image;
			this.id = ID;
			this.Visible = true;
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
			if (this.Visible)
			{
				GraphicsManager.spriteBatch.Draw(this.image, this.Position, Color.White);
				base.Draw(gameTime);
			}
		}

	}
}
