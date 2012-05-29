using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Classes.Pipeline;
using Classes.Graphics;
using Classes.IO;
using Classes.UI;

namespace Classes
{
	class Menu : DrawableGameComponent
	{

		Texture2D background;
		Texture2D front;
		Vector2 backPos;
		Vector2 frontPos;
		Button test;

		public Menu()
			: base(GameRef.Game)
		{
			//No Boom Boom Pow
		}


		protected override void LoadContent()
		{
			background = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/menu_back");
			front = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/menu_title_font");
			backPos = new Vector2(0, background.Height * -1);
			frontPos = new Vector2(0, 0);
			test = new Button(new Vector2(200, 300), "Testbutton");
			base.LoadContent();
		}

		public override void Initialize()
		{
			SoundManager.playBackgroundMusic("Audio/music/title", true);
			this.LoadContent();
			//base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			
			//Init Menu
			if (frontPos.Y < 0)
			{
				if (backPos.Y < 0)
				{
					backPos.Y += 25;
				}
				else
				{
					backPos.Y = 0;
					frontPos.Y += 25;
				}
			}
			else
			{
				backPos.Y = 0;
				frontPos.Y = 0;
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Begin();
			GraphicsManager.spriteBatch.Draw(background, backPos, Color.White);
			GraphicsManager.spriteBatch.Draw(front, frontPos, Color.White);
			GraphicsManager.spriteBatch.End();

			test.Draw(gameTime);

			base.Draw(gameTime);
		}
	}
}
