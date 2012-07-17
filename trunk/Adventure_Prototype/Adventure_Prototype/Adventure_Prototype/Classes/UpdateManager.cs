using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Classes
{
	class UpdateManager
	{
		private static List<GameComponent > updateList = new List<GameComponent >();


		/// <summary>
		/// Adds an Item to the Update Manager
		/// </summary>
		/// <param name="Item">The Item you want to add.</param>
		public static void addItem(GameComponent  Item)
		{
		
			updateList.Add(Item);
		}



		/// <summary>
		/// Tries to remove an Item from the update list and returns true/false upon it's success
		/// </summary>
		/// <param name="Item">The item you want to remove from the list</param>
		/// <returns></returns>
		public static bool removeItem(GameComponent  Item)
		{
			try
			{
				updateList.Remove(Item);
				return true;
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
			return false;
		}


		/// <summary>
		/// Update Logic for the manager. Call once / frame.
		/// </summary>
		public static void Update(GameTime gameTime)
		{
			foreach (GameComponent g in updateList)
			{
				if (g != null)
				{
					g.Update(gameTime);
				}
			}
		}
	}
}
