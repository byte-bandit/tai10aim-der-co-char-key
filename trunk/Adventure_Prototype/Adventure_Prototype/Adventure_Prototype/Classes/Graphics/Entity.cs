using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Classes.Graphics
{

	public abstract  class Entity : DrawableGameComponent
	{

		public Entity(Game game) : base(game)
		{
		}

		public abstract Vector2 GetFloatingLinePosition();
		public abstract Color GetFloatingLineColor();
		public abstract void SetFloatingLineColor(Color color);

	}
}
