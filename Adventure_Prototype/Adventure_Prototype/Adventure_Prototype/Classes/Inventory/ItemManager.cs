using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using Classes.Graphics;
using Classes.Pipeline;

namespace Classes.Inventory
{
     public class ItemManager
    {
        public SortedList<String, Item> ItemLibrary;

        public  bool CheckForItem(String item_id)
        {
            return ItemLibrary.ContainsKey(item_id);
        }

        public ItemManager()
        {
        }

        public void Initialize()
        {
            String[] data;
            ItemLibrary = new SortedList<string, Item>();

            try
            {
                data = System.IO.File.ReadAllLines("Content/Items.txt");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return;
            }

            for (int n = 0; n < data.Length; n++)
            {
                String line = data[n];

                if (line.Trim().StartsWith("#"))
                {
                    //Commentary Line - ignore and get next line
                    continue;
                }
               
                if (line.Trim().StartsWith("BEGINITEM"))
                {
                    n++;
                    //Get Id of the event
                    String id = data[n].Substring(data[n].IndexOf("ID:") + 3);
                    n++;
                    String asset = data[n].Substring(data[n].IndexOf("ASSET:")+6);
                    n++;

                    Item tmp = new Item(GameRef.Game.Content.Load<Texture2D>("Graphics/Sprites/"+asset), id);
                    GraphicsManager.addChild(tmp);
                    ItemLibrary.Add(tmp.ID, tmp);
                }

            }

       }
   }

}

