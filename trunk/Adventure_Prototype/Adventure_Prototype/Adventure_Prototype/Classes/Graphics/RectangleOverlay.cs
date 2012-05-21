using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Classes.Graphics
{
	public class RectangleOverlay : DrawableGameComponent
	{
		Rectangle dummyRectangle;
		Color Colori;




		/// <summary>
		/// Creates a new visible Rectangle.
		/// </summary>
		/// <param name="rect">The measurements of the Rectangle</param>
		/// <param name="colori">The Color you want the Rectangle to be</param>
		/// <param name="game">A link to our Game</param>
		public RectangleOverlay(Rectangle rect, Color colori, Game game)
			: base(game)
		{
			DrawOrder = 1000;
			dummyRectangle = rect;
			Colori = colori;
		}




		/// <summary>
		/// Draws the Rectangle
		/// </summary>
		/// <param name="gameTime">Elapsed Time since start of the Application</param>
		public override void Draw(GameTime gameTime)
		{
			Graphics.GraphicsManager.spriteBatch.Begin();
			Graphics.GraphicsManager.spriteBatch.Draw(Graphics.GraphicsManager.dummyTexture, dummyRectangle, Colori);
			Graphics.GraphicsManager.spriteBatch.End();
		}
	}
}
