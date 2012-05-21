using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace Classes.IO
{
	static class KeyboardEx
	{

		private static Boolean[] prevKeyStates;		//Stores previous Key states
		private static Boolean[] currKeyStates;



		/// <summary>
		/// Gets the current Keyboard State
		/// </summary>
		/// <returns></returns>
		public static KeyboardState GetState()
		{
			return Keyboard.GetState();
		}




		/// <summary>
		/// Allows the Control to update it's logic. Call once per Cycle.
		/// </summary>
		public static void Update()
		{

			for (int n = 0; n < currKeyStates.Length; n++)
			{
				prevKeyStates[n] = currKeyStates[n];
			}

			int cnt = 0;
			foreach (Keys k in Enum.GetValues(typeof(Keys)))
			{
				if (Keyboard.GetState().IsKeyDown(k))
				{
					currKeyStates[cnt] = true;
				}
				else
				{
					currKeyStates[cnt] = false;
				}
				cnt++;
			}
		}





		/// <summary>
		/// Initializes the Component to be ready for use.
		/// </summary>
		public static void Initialize()
		{
			int cnt = 0;
			foreach (Keys k in Enum.GetValues(typeof(Keys)))
			{
				cnt++;
			}
			prevKeyStates = new Boolean[cnt];
			currKeyStates = new Boolean[cnt];
			cnt = 0;
			foreach (Keys k in Enum.GetValues(typeof(Keys)))
			{
				prevKeyStates[cnt] = false;
				currKeyStates[cnt] = false;
				cnt++;
			}
		}





		/// <summary>
		/// Returns wether the specified key is hit [!=down]
		/// </summary>
		/// <param name="key">The key to look for, i.e. Keys.A</param>
		/// <returns></returns>
		public static bool isKeyHit(Keys key)
		{
			if (Keyboard.GetState().IsKeyDown(key))
			{
				int c = 0;
				foreach (Keys k in Enum.GetValues(typeof(Keys)))
				{
					if (k == key && prevKeyStates[c] == false)
					{
						return true;
					}
					c++;
				}
			}
			return false;
		}





		/// <summary>
		/// Returns whether the specified Key is currently up
		/// </summary>
		/// <param name="key">The specific key to look for, i.e. Keys.A</param>
		/// <returns></returns>
		public static bool isKeyUp(Keys key)
		{
			return Keyboard.GetState().IsKeyUp(key);
		}




		/// <summary>
		/// Returns whether the specified Key is currently down (!= hit)
		/// </summary>
		/// <param name="key">The specific key to look for, i.e. Keys.A</param>
		/// <returns></returns>
		public static bool isKeyDown(Keys key)
		{
			return Keyboard.GetState().IsKeyDown(key);
		}
	}
}