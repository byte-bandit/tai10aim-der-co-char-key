using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Classes.Pipeline;
using Classes.Graphics;
using Classes.IO;

namespace Classes.UI
{
	class Button : DrawableGameComponent
	{

		private Vector2 position;
		private Texture2D gfx;
		private Texture2D gfxDown;
		private String text;



		public Button(Vector2 pos, String text = "")
			: base(GameRef.Game)
		{
			this.text = text;
			this.position = pos;
			this.LoadContent();
		}




		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}






		public String Text
		{
			get { return this.text; }
			set { this.text = value; }
		}






		protected override void LoadContent()
		{
			gfx = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/button_up");
			gfxDown = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/button_down");
			base.LoadContent();
		}





		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Begin();
			GraphicsManager.spriteBatch.Draw(gfx, position, Color.White);
			GraphicsManager.spriteBatch.End();

			Vector2 origin = Vector2.Add(position, Vector2.Multiply(new Vector2(gfx.Width, gfx.Height) , 0.5f));
			origin = Vector2.Subtract(origin, Vector2.Multiply(GraphicsManager.font03.MeasureString(text), 0.5f));

			GraphicsManager.drawText(text, origin, GraphicsManager.font03, Color.White, true);
			base.Draw(gameTime);
		}




	}
}
