using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Classes.Graphics
{
	public class LineOverlay : DrawableGameComponent
	{
		Vector2 dummyVector1;
		Vector2 dummyVector2;
		Color Colori;

		/// <summary>
		/// Creates a new visible Line.
		/// </summary>
		/// <param name="vect">The measurements of the Rectangle</param>
		/// <param name="colori">The Color you want the Rectangle to be</param>
		/// <param name="game">A link to our Game</param>
		public LineOverlay(Vector2 vect1, Vector2 vect2, Color colori, Game game)
			: base(game)
		{
			DrawOrder = 1000;
			dummyVector1 = vect1;
			dummyVector2 = vect2;
			Colori = colori;
		}




		/// <summary>
		/// Draws the Line
		/// </summary>
		/// <param name="gameTime">Elapsed Time since start of the Application</param>
		public override void Draw(GameTime gameTime)
		{
			Graphics.GraphicsManager.spriteBatch.Begin();
			Graphics.GraphicsManager.DrawLine(3, Colori, dummyVector1, dummyVector2);
			Graphics.GraphicsManager.spriteBatch.End();
		}
	}
}
