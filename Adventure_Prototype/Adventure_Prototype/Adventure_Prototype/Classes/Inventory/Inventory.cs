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
	   
	class Inventory : DrawableGameComponent
	{
		static private Texture2D Image;
		static private List<Item> items = new List<Item>();
		static public List<Item> Items
		{
			get
			{
				return items;
			}
		}

		public static void AddItem(Item item)
		{
			items.Add(item);
		}

		public static bool RemoveItem(Item item)
		{
			return items.Remove(item);
		}


		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Begin();
			foreach (Item t in items)
			{
				t.Draw(gameTime); 
			}
			GraphicsManager.spriteBatch.End();
			base.Draw(gameTime);
		}




		public Inventory(): base(GameRef.Game)
		{}
	}

	

}
