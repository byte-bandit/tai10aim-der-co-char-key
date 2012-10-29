using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Classes.Pipeline;
using Classes.Graphics;

namespace Classes.Pathfinding
{
	public class Polygon : Entity
	{
		private bool changeable;
		private Color floatingLineColor = default(Color);
		public List<Vector2> Nodes = new List<Vector2>();

		public Polygon(Boolean change, Game game)
			: base(GameRef.Game)
		{
			this.changeable = change;
		}




		public override Color GetFloatingLineColor()
		{
			return this.floatingLineColor;
		}

		public override void SetFloatingLineColor(Color color)
		{
			this.floatingLineColor = color;
		}



		public bool Changeable
		{
			get { return changeable; }
			set { changeable = value; }
		}

		public bool firstNodeInRange(Vector2 position)
		{
			return (NodeInRange(Nodes[0], position, 4.0f));
		}

		public static bool NodeInRange(Vector2 Point1, Vector2 Point2, float Range)
		{
			if (Vector2.Distance(Point1, Point2) <= 4.0f)
			{
				return true;
			}
			return false;
		}

		public override Vector2 GetFloatingLinePosition()
		{
			return new Vector2(Nodes[0].X, Nodes[0].Y);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.End();
			GraphicsManager.spriteBatch.Begin();
			for (int i = 1; i <= Nodes.Count; i++)
			{
				GraphicsManager.DrawLine(1, Color.White, Nodes[i], Nodes[i - 1]);
			}
			GraphicsManager.spriteBatch.End();
			GraphicsManager.spriteBatch.Begin();
			base.Draw(gameTime);
		}

	}
}
