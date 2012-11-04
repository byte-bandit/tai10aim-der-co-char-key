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
        private static SortedList<String, Item> itemLibrary = new SortedList<string,Item>();

        public void AddToInventory(String id)
        {
            GameRef.Inventory.AddItem(itemLibrary[id]);
            
        }

        public static void Initialize()
        {
            String[] data;

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

                    int X = new Int32();
                    int Y = new Int32();

                    try
                        {
                            X = Convert.ToInt32(data[n].Substring(data[n].IndexOf("X:")+2));
                            n++;
                            Y = Convert.ToInt32(data[n].Substring(data[n].IndexOf("Y:")+2));
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Input string is not a sequence of digits.");
                        }
                    Item tmp = new Item(X, Y, GameRef.Game.Content.Load<Texture2D>(asset), id);
                    GraphicsManager.addChild(tmp);
                    itemLibrary.Add(tmp.ID, tmp);
                }

            }

       }
   }

}

