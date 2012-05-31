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
		private Texture2D gfxDis;
		private String text;
		private int btnState = 0; //0=normal, 1=mouseOver, 2=mouseClick
		private String id;
		private bool disabled = false;



		public Button(Vector2 pos, String ID, String text = "", bool disabled = false)
			: base(GameRef.Game)
		{
			this.text = text;
			this.position = pos;
			this.id = ID;
			this.LoadContent();
			this.disabled = disabled;
		}



		public bool isDisabled
		{
			get { return this.disabled; }
			set { this.disabled = value; }
		}




		public int ButtonState
		{
			get { return this.btnState; }
			set { this.btnState = value; }
		}




		public String ID
		{
			get { return this.id; }
		}



		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}




		public Rectangle HitBox
		{
			get { return new Rectangle((int)position.X, (int)position.Y, gfx.Width, gfx.Height); }
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
			gfxDis = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/button_disabled");
			base.LoadContent();
		}





		public override void Update(GameTime gameTime)
		{

			btnState = 0;

			if (this.disabled)
			{
				return;
			}

			//Check if hover
			if (MouseEx.inBoundaries(this.HitBox))
			{
				btnState = 1;
			}

			//Check if clicked
			if (MouseEx.clickOnButton(this))
			{
				SoundManager.Click();
				btnState = 2;
			}
			
			base.Update(gameTime);
		}





		public override void Draw(GameTime gameTime)
		{
			Vector2 origin = Vector2.Add(position, Vector2.Multiply(new Vector2(gfx.Width, gfx.Height) , 0.5f));
			origin = Vector2.Subtract(origin, Vector2.Multiply(GraphicsManager.font03.MeasureString(text), 0.5f));

			if (!this.disabled)
			{

				if (btnState == 2)
				{
					GraphicsManager.spriteBatch.Begin();
					GraphicsManager.spriteBatch.Draw(gfxDown, position, Color.White);
					GraphicsManager.spriteBatch.End();
				}
				else
				{
					GraphicsManager.spriteBatch.Begin();
					GraphicsManager.spriteBatch.Draw(gfx, position, Color.White);
					GraphicsManager.spriteBatch.End();
				}

				if (btnState == 0)
				{
					GraphicsManager.drawText(text, origin, GraphicsManager.font03, Color.White, true);
				}
				else
				{
					GraphicsManager.drawText(text, origin, GraphicsManager.font03, Color.Yellow, true);
				}
			}
			else
			{
				GraphicsManager.spriteBatch.Begin();
				GraphicsManager.spriteBatch.Draw(gfxDis, position, Color.White);
				GraphicsManager.spriteBatch.End();
				GraphicsManager.drawText(text, origin, GraphicsManager.font03, Color.FloralWhite, true);
			}
			base.Draw(gameTime);
		}




	}
}
