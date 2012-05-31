using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Classes.Graphics
{
	class GraphicsManager
	{
		private static List<DrawableGameComponent> children= new List<DrawableGameComponent > ();
		public static SpriteBatch spriteBatch;
		public static SpriteFont font01;
		public static SpriteFont font02;
		public static SpriteFont font03;
		public static Texture2D dummyTexture;

		private static GraphicsDevice gd;


		/// <summary>
		/// Adds a new Child for the Graphics Manager to draw.
		/// </summary>
		/// <param name="item">The Component to add to the GraphicsManagers List</param>
		public static void addChild(DrawableGameComponent item)
		{
			children.Add(item);
		}




		/// <summary>
		/// Tries to remove a Child from the GraphicsManagers Drawing List and returns the outcome.
		/// </summary>
		/// <param name="item">The item to remove from the GraphicManager</param>
		/// <returns></returns>
		public static bool removeChild(DrawableGameComponent item)
		{
			return children.Remove(item);
		}







		/// <summary>
		/// Draws a Line between 2 points
		/// </summary>
		/// <param name="width">line width</param>
		/// <param name="color">line color</param>
		/// <param name="point1">starting point</param>
		/// <param name="point2">end point</param>
		/// <returns></returns>
		public static void DrawLine(float width, Color color, Vector2 point1, Vector2 point2)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);

			GraphicsManager.spriteBatch.Draw(dummyTexture, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
		}






		/// <summary>
		/// Tries to remove a Child from the GraphicsManagers Drawing List and returns the outcome.
		/// </summary>
		/// <param name="item">The item to remove from the GraphicManager</param>
		/// <returns></returns>
		public static void drawText(String text, Vector2 position, SpriteFont font, Color color, bool shadow=false)
		{
			Graphics.GraphicsManager.spriteBatch.Begin();
			if (shadow)
			{
				drawShadowText(text, position, font);
			}

			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(position.X, position.Y), color);
			Graphics.GraphicsManager.spriteBatch.End();
		}




		private static void drawShadowText(String text, Vector2 pos, SpriteFont font)
		{
			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X - 1, pos.Y - 1), Color.Black);
			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X + 1, pos.Y - 1), Color.Black);
			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X - 1, pos.Y + 1), Color.Black);
			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X + 1, pos.Y + 1), Color.Black);

			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X, pos.Y + 1), Color.Black);
			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X, pos.Y - 1), Color.Black);
			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X - 1, pos.Y), Color.Black);
			Graphics.GraphicsManager.spriteBatch.DrawString(font, text, new Vector2(pos.X + 1, pos.Y), Color.Black);
		}




		/// <summary>
		/// Setup our fonts so we can use them later on. Only called ONCE!
		/// </summary>
		/// <param name="Font01">Path to font01</param>
		/// <param name="gp">Link to Graphics Device</param>
		public static void initializeFonts(SpriteFont Font01, SpriteFont Font02,  SpriteFont Font03, GraphicsDevice gp)
		{
			dummyTexture = new Texture2D(gp, 1,1);
			dummyTexture.SetData(new Color[] { Color.White });
			font01 = Font01;
			font02 = Font02;
			font03 = Font03;
			gd = gp;
		}






		public static GraphicsDevice GraphicsDevice
		{
			get { return gd; }
		}




		/// <summary>
		/// Draws the Contents of the GraphicsManager's Draw List
		/// </summary>
		/// <param name="gameTime">Elapsed Time since start of the Application</param>
		public static void Draw(GameTime gameTime)
		{
			foreach (DrawableGameComponent d in children)
			{
				d.Draw(gameTime);
			}

		}
	}
}
