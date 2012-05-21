using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Classes
{
	class MouseEx 
	{
		private static MouseState prevMouseState;
		private static MouseState currMouseState;


		/// <summary>
		/// Checks wether a click is performed.
		/// </summary>
		/// <returns></returns>
		public static bool click()
		{
			if (prevMouseState.LeftButton == ButtonState.Released && currMouseState.LeftButton == ButtonState.Pressed)
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		/// <summary>
		/// Checks wether a click is performed at a given Position
		/// </summary>
		/// <param name="position">The Position to check the click against.</param>
		/// <returns></returns>
		public static bool clickAt(Vector2 position)
		{
			if (click())
			{
				if (currMouseState.X == position.X && currMouseState.Y == position.Y)
				{
					return true;
				}
			}
			return false;
		}



		/// <summary>
		/// Checks wether a click is performed at a given Position
		/// </summary>
		/// <param name="position">The Position to check the click against.</param>
		/// <returns></returns>
		public static bool clickAt(Rectangle position)
		{
			if (click())
			{
				if (currMouseState.X >= position.X && currMouseState.Y >= position.Y && currMouseState.X <= (position.X+position.Width) && currMouseState.Y <= (position.Y + position.Height))
				{
					return true;
				}
			}
			return false;
		}



		/// <summary>
		/// Checks wether a right click is performed.
		/// </summary>
		/// <returns></returns>
		public static bool rightClick()
		{
			if (prevMouseState.RightButton == ButtonState.Released && currMouseState.RightButton == ButtonState.Pressed)
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		/// <summary>
		/// Checks whether a right click is performed at a given Position
		/// </summary>
		/// <param name="position">The Position to check the click against.</param>
		/// <returns></returns>
		public static bool rightClickAt(Vector2 position)
		{
			if (rightClick())
			{
				if (currMouseState.X == position.X && currMouseState.Y == position.Y)
				{
					return true;
				}
			}
			return false;
		}



		/// <summary>
		/// Checks wether a right click is performed at a given Position
		/// </summary>
		/// <param name="position">The Position to check the click against.</param>
		/// <returns></returns>
		public static bool rightClickAt(Rectangle position)
		{
			if (rightClick())
			{
				if (currMouseState.X >= position.X && currMouseState.Y >= position.Y && currMouseState.X <= (position.X + position.Width) && currMouseState.Y <= (position.Y + position.Height))
				{
					return true;
				}
			}
			return false;
		}



		/// <summary>
		/// Gets the current Mouse State
		/// </summary>
		/// <returns></returns>
		public MouseState getState()
		{
			return Mouse.GetState();
		}



		/// <summary>
		/// Update Logic for this class
		/// </summary>
		public static void update()
		{
			prevMouseState = currMouseState;
			currMouseState = Mouse.GetState();
		}
	}
}
