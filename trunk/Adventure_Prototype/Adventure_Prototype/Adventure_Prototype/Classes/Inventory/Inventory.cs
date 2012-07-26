using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

using Classes.Pipeline;
using Classes.Graphics;
using Classes.IO;
using Classes.Action;


namespace Classes.Inventory
{
	   
	public static class Inventory : DrawableGameComponent
	{
		private static Texture2D Image;
		private static List<Item> items = new List<Item>();
		public static List<Item> Items
		{
			get
			{
				return items;
			}
		}
		public static bool Visible
		{
			get { return Visible; }
			set
			{
				foreach (Item p in items)
				{
					p.Visible = value;
				}
				Visible = value;
			}
		}


		public static void AddItem(Item item)
		{
			if (Inventory.Visible)
			{ item.Visible = true; }
			else
			{ item.Visible = false; }

			items.Add(item);
		}

		public static bool RemoveItem(Item item)
		{
			return items.Remove(item);
		}


		public override void Update(GameTime gameTime)
		{
			if (GameRef.Inventory)
			{
				Visible = true;
			}
			else
			{
				Visible = false;
			}
			base.Update(gameTime);
		}


		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Begin();
			GraphicsManager.spriteBatch.Draw(Image, Vector2.Zero, Color.White);
			foreach (Item t in items)
			{
				t.Draw(gameTime); 

			}
			foreach (Item t in items)
			{
				if (MouseEx.inBoundaries(t.Image.Bounds))
				{
					GraphicsManager.spriteBatch.Draw(t.Tooltip, MouseEx.Position(), Color.White);
				}
			}
			GraphicsManager.spriteBatch.End();
			base.Draw(gameTime);
		}

		public Inventory(): base(GameRef.Game)
		{}



	}

	

}
