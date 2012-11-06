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
        private Item focus = new Item(null, "focus", "", "", "", "");
        private Item old_focus = new Item(null, "old_focus", "", "", "", "");
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
            get { return this.status; }
            set { this.status = value; }
        }

        public Item Focus
        {
            get { return focus; }
            set { focus = value; }
        }
        public Item Old_Focus
        {
            get { return old_focus; }
            set { old_focus = value; }
        }
        #endregion

        public void AddItem(Item item)
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
            int counter = 0;
            if (GameRef.Inventory.Status)
            {

                Status = true;
                foreach (Item t in items)
                {
                    if (counter < 4)
                    {
                        t.X = 294 * counter + 100;
                        t.Y = 100;
                    }
                    else
                    {
                        t.X = 294 * counter + 100;
                        t.Y = 420;
                    }
                    counter++;
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




        public void Draw(GameTime gameTime)
        {
            int counter = 0;
           
            GraphicsManager.spriteBatch.Begin();
            GraphicsManager.spriteBatch.Draw(Image, Vector2.Zero, Color.White);
            GraphicsManager.spriteBatch.End();
            foreach (Item t in items)
            {
                GraphicsManager.spriteBatch.Begin();

                GraphicsManager.spriteBatch.Draw(t.Image, new Vector2(t.X, t.Y), Color.White);
                GraphicsManager.spriteBatch.End();
                if (MouseEx.inBoundaries(t.getDrawingRectangle()))
                {
                    GraphicsManager.drawText(t.Tooltip, new Vector2(MouseEx.Position().X-10, MouseEx.Position().Y), GraphicsManager.font01, Color.White, true);
                }
                counter++;
            }
        }

        public Inventory()
            : base(GameRef.Game)
        {
            this.status = false;
            Image = GameRef.Game.Content.Load<Texture2D>("Graphics/Backgrounds/funn");

        }

    }
}

	


