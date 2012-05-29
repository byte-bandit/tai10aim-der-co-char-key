using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Classes.Pipeline;

namespace Classes.IO
{
	class MouseEx 
	{
		private static MouseState prevMouseState;
		private static MouseState currMouseState;


		/// <summary>
		/// Checks whether a click is performed.
		/// </summary>
		/// <returns></returns>
		public static bool click()
		{
			if (prevMouseState.LeftButton == ButtonState.Released && currMouseState.LeftButton == ButtonState.Pressed)
			{
				if (inBoundaries(new Rectangle(0,0,(int)GameRef.Resolution.X,(int) GameRef.Resolution.Y)))
					return true;
				else
					return false;
			}
			else
			{
				return false;
			}
		}





		public static bool clickInPolygon(Classes.Pathfinding.Polygon polygon)
		{
			if (!click())
			{
				return false;
			}

			Vector2 mid1 = Vector2.Add(polygon.Nodes[0],Vector2.Multiply(Vector2.Subtract(polygon.Nodes[1], polygon.Nodes[0]), 0.5f));
			Vector2 mid2 = Vector2.Add(mid1, Vector2.Multiply(Vector2.Subtract(polygon.Nodes[2], mid1), 0.5f));

			Pathfinding.Path route = new Pathfinding.Path(mid2, MouseEx.Position(), polygon);
			LinkedList<Vector2> walkingRoute = route.findPath();

			if (walkingRoute.Count % 2 ==  1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}






		public static bool inPolygon(Classes.Pathfinding.Polygon polygon)
		{

			Vector2 mid1 = Vector2.Add(polygon.Nodes[0], Vector2.Multiply(Vector2.Subtract(polygon.Nodes[1], polygon.Nodes[0]), 0.5f));
			Vector2 mid2 = Vector2.Add(mid1, Vector2.Multiply(Vector2.Subtract(polygon.Nodes[2], mid1), 0.5f));

			Pathfinding.Path route = new Pathfinding.Path(mid2, MouseEx.Position(), polygon);
			LinkedList<Vector2> walkingRoute = route.findPath();

			if (walkingRoute.Count % 2 == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}





		/// <summary>
		/// Checks whether the mouse is clicked inside a given rectangle
		/// </summary>
		/// <param name="target">The target Rectangle to check against</param>
		/// <returns></returns>
		public static bool clickInRectangle(Rectangle target)
		{
			if (click())
			{
				if (inBoundaries(target))
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}



		/// <summary>
		/// Returns whether the Left Mouse Button is currently pressed down.
		/// </summary>
		/// <returns></returns>
		public static bool pressed_LMB()
		{
			if (Mouse.GetState().LeftButton == ButtonState.Pressed && inBoundaries(new Rectangle(0,0, (int)GameRef.Resolution.X, (int)GameRef.Resolution.Y)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}




		/// <summary>
		/// Checks whether the Mouse is currently inside the given Rectangle
		/// </summary>
		/// <param name="Boundaries">The Boundarie to check the Mouse's position against</param>
		/// <returns></returns>
		public static bool inBoundaries(Rectangle Boundaries)
		{
			if (Mouse.GetState().X >= Boundaries.X && Mouse.GetState().X <= Boundaries.X + Boundaries.Width && Mouse.GetState().Y >= Boundaries.Y &&
				Mouse.GetState().Y <= Boundaries.Y + Boundaries.Height)
			{
				return true;
			}
			else
			{
				return false;
			}
		}




		/// <summary>
		/// Checks whether the Mouse is within 4.0 pixels of the given Vector.
		/// </summary>
		/// <param name="Boundaries">The Vector to check against.</param>
		/// <returns></returns>
		public static bool inRange(Vector2 Boundaries, float Distance = 4.0f)
		{
			if (Vector2.Distance(new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Boundaries) <= Distance)
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
		/// Gets the current Position of the Mouse
		/// </summary>
		/// <returns></returns>
		public static Vector2 Position()
		{
			return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
		}



		/// <summary>
		/// Returns wether the MouseScroll has been scrolled Up
		/// </summary>
		/// <returns></returns>
		public static bool scrollUp()
		{
			if (prevMouseState.ScrollWheelValue < currMouseState.ScrollWheelValue)
			{
				return true;
			}
			else
			{
				return false;
			}
		}




		/// <summary>
		/// Returns wether the MouseScroll has been scrolled down
		/// </summary>
		/// <returns></returns>
		public static bool scrollDown()
		{
			if (prevMouseState.ScrollWheelValue > currMouseState.ScrollWheelValue)
			{
				return true;
			}
			else
			{
				return false;
			}
		}



		/// <summary>
		/// Gets the current Mouse State
		/// </summary>
		/// <returns></returns>
		public static MouseState getState()
		{
			return Mouse.GetState();
		}



		/// <summary>
		/// Returns the previous Mouse State
		/// </summary>
		/// <returns></returns>
		public static MouseState getPreviousState()
		{
			return prevMouseState;
		}


		/// <summary>
		/// Gets the current X Position of the Mouse
		/// </summary>
		public static int X
		{
			get { return Mouse.GetState().X; }
		}


		/// <summary>
		/// Gets the current Y Position of the Mouse
		/// </summary>
		public static int Y
		{
			get { return Mouse.GetState().Y; }
		}



		/// <summary>
		/// Update Logic for this class
		/// </summary>
		public static void Update()
		{
			prevMouseState = currMouseState;
			currMouseState = Mouse.GetState();
		}
	}
}
