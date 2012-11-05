using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

using Classes.Pipeline;
using Classes.Graphics;
using Classes.IO;


namespace Classes.Inventory
{
	   
	public class Inventory : DrawableGameComponent
	{
		private Texture2D Image;
		private Item focus = new Item(null, "focus");
		private List<Item> items = new List<Item>();
		private bool status;

		#region Properties
		public List<Item> Items
		{
			get
			{
				return items;
			}
		}
		public bool Status
		{
			get { return this.status;	}
			set	{this.status = value;	}
		}

		public Item Focus
		{
			get { return focus; }
			set { focus = value; }
		}
		#endregion

		public  void AddItem(Item item)
		{
			if (this.Status)
			{ item.Visible = true; }
			else
			{ item.Visible = false; }

			items.Add(item);
		}

		public bool RemoveItem(Item item)
		{
			return items.Remove(item);
		}


		public override void Update(GameTime gameTime)
		{
			if (GameRef.Inventory.Status)
			{

				Status = true;
                foreach (Item t in items)
                {
                    t.Visible = Status;
                }
			}
			else
			{
				Status = false;
                foreach (Item t in items)
                {
                    t.Visible = Status;
                }
			}
			base.Update(gameTime);
		}




		public override void Draw(GameTime gameTime)
		{
            int counter = 0;
            Vector2 position;
			if (this.status)
			{
			GraphicsManager.spriteBatch.Begin();
			
				GraphicsManager.spriteBatch.Draw(Image, Vector2.Zero, Color.White);
				foreach (Item t in items)
				{
                    if (counter > 4)
                        position = new Vector2((294 * counter) + 100, 100);
                    else
                        position = new Vector2((294 * counter) + 100, 420);

                    GraphicsManager.spriteBatch.Draw(t.Image, position, Color.White);
					
					if (MouseEx.inBoundaries(t.Image.Bounds))
					{
						GraphicsManager.spriteBatch.Draw(t.Tooltip, MouseEx.Position(), Color.White);
					}
                    counter++;
				}
			
			GraphicsManager.spriteBatch.End();
			}
			base.Draw(gameTime);
		}

		public Inventory(): base(GameRef.Game)
		{
			this.status = false;
		}





		protected override void LoadContent()
		{
			Image = GameRef.Game.Content.Load<Texture2D>("Graphics/Backgrounds/funn");

			base.LoadContent();
		}
	}

	

}
