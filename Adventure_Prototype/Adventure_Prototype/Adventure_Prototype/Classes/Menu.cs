using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;

using Classes.Pipeline;
using Classes.Graphics;
using Classes.IO;
using Classes.UI;
using Classes.Net;

namespace Classes
{
	public class Menu : DrawableGameComponent
	{

		Texture2D background;
		Texture2D front;
		Vector2 backPos;
		Vector2 frontPos;
		Texture2D playerspace;
		Texture2D noGamerPic;
		Texture2D sad;
		List<Button> buttons;
		int selectedSessionIndex;
		public MenuStates state;



		public enum MenuStates
		{
			MAIN,
			HOST,
			JOIN
		}

		public Menu()
			: base(GameRef.Game)
		{
			buttons = new List<Button>();
		}


		protected override void LoadContent()
		{
			background = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/menu_back");
			front = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/menu_title_font");
			playerspace = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/menu_player_space");
			noGamerPic = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/noGamePic");
			sad = GameRef.Game.Content.Load<Texture2D>("Graphics/UI/sad");
			backPos = new Vector2(0, 0);
			frontPos = new Vector2(0, 0);
			selectedSessionIndex = -1;

			base.LoadContent();
		}

		public override void Initialize()
		{
			SoundManager.playBackgroundMusic("Audio/music/title", true);
			this.LoadContent();
			this.state = MenuStates.MAIN;


			
			//base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{

			switch (this.state)
			{
				case MenuStates.MAIN:
					// Handle the title screen input here..
					buttons.Clear();
					buttons.Add(new Button(new Vector2(500, 300), "btnHost", "Spiel hosten"));
					buttons.Add(new Button(new Vector2(500, 396), "btnJoin", "Spiel joinen"));
					buttons.Add(new Button(new Vector2(500, 492), "btnQuit", "Beenden"));
					break;



				case MenuStates.HOST:
					// Handle the lobby input here...
					buttons.Clear();
					buttons.Add(new Button(new Vector2(512, 640), "btnReady", "Bereit"));
					if (NetworkManager.IsHost)
					{
						if (NetworkManager.IsEveryoneReady && NetworkManager.ConnectedGamers.Count == 2)
						{
							buttons.Add(new Button(new Vector2(960, 640), "btnGo", "Start", false));
						}
						else
						{
							buttons.Add(new Button(new Vector2(960, 640), "btnGo", "Start", true));
						}
						buttons.Add(new Button(new Vector2(64, 640), "btnBack", "Zurück"));
					}
					else
					{
						buttons.Add(new Button(new Vector2(64, 640), "btnBack3", "Zurück"));
					}
					break;



				case MenuStates.JOIN:
					// Handle the available sessions input here..
					buttons.Clear();
					if (selectedSessionIndex < 0)
					{
						buttons.Add(new Button(new Vector2(960, 640), "btnJoinSession", "Join", true));
					}
					else
					{
						buttons.Add(new Button(new Vector2(960, 640), "btnJoinSession", "Join", false));
					}
					buttons.Add(new Button(new Vector2(64, 640), "btnBack2", "Zurück"));
					handleAvailibleSessionsInput();
					break;
			}


	

			foreach (Button b in buttons)
			{
				b.Update(gameTime);

				clickHandlers(b);
			}

			base.Update(gameTime);
		}







		private void handleAvailibleSessionsInput()
		{
			if (!MouseEx.click())
			{
				return;
			}

			for (int sessionIndex = 0; sessionIndex < NetworkManager.availableServers.Count; sessionIndex++)
			{

				if (MouseEx.clickAt(new Rectangle(256, 192 + ((sessionIndex) * 192) + 32, 768, 192)))
				{
					selectedSessionIndex = sessionIndex;
					break;
				}
			}
		}





		private String getSelectedServerIP()
		{
			if (selectedSessionIndex < 0)
			{
				return null;
			}

			return NetworkManager.availableServers[selectedSessionIndex];
		}




		private void clickHandlers(Button b)
		{
			if (b.ButtonState == 2)
			{
				switch (b.ID)
				{
					case "btnHost":
						while (!NetworkManager.setPlayerName())
						{
							Microsoft.VisualBasic.Interaction.MsgBox("Der eingegebe Name ist ungültig. Bitte versuche es erneut.");
							return;
						}
						NetworkManager.createSession();
						this.state = MenuStates.HOST;
						break;
					case "btnJoin":
						NetworkManager.lookForServers();
						this.state = MenuStates.JOIN ;
						break;
					case "btnJoinSession":
						while (!NetworkManager.setPlayerName())
						{
							Microsoft.VisualBasic.Interaction.MsgBox("Der eingegebe Name ist ungültig. Bitte versuche es erneut.");
							return;
						}
						NetworkManager.Connect(getSelectedServerIP());
						this.state = MenuStates.HOST;
						break;
					case "btnReady":
						NetworkManager.Profile.Ready = !NetworkManager.Profile.Ready;
						break;
					case "btnBack":
						NetworkManager.quitSession();
						this.state = MenuStates.MAIN;
						break;
					case "btnBack2":
						this.state = MenuStates.MAIN;
						break;
					case "btnBack3":
						NetworkManager.leaveSession();
						this.state = MenuStates.MAIN;
						break;
					case "btnQuit":
						GameRef.Game.Exit();
						break;
					case "btnGo":
						NetworkManager.startGame();
						break;
				}
			}
		}





		private void DrawLobby()
		{

			GraphicsManager.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
			GraphicsManager.spriteBatch.Draw(playerspace, new Vector2(256, 192), Color.White);
			GraphicsManager.spriteBatch.Draw(playerspace, new Vector2(256, 416), Color.White);
			GraphicsManager.spriteBatch.End();

			int x = 1;

			for (int a = 0; a < NetworkManager.ConnectedGamers.Count ; a++ )
			{
				string text = NetworkManager.ConnectedGamers[a].Name  ;
				Texture2D pic = noGamerPic;

				if (NetworkManager.ConnectedGamers[a].Ready)
				{
					text += " - Bereit!";
				}

				GraphicsManager.spriteBatch.Begin();
				GraphicsManager.spriteBatch.Draw(pic, new Vector2(320, x * 256 - ((x-1)*32)), Color.White);
				GraphicsManager.spriteBatch.End();
				GraphicsManager.drawText(text, new Vector2(392, x * 264 - ((x-1)*40)), GraphicsManager.font03, Color.White, true);


				x++;
			}

			GraphicsManager.drawText("Connects: " + NetworkManager.ConnectedGamers.Count.ToString(), new Vector2(20, 20), GraphicsManager.font02, Color.White, true);
		}






		private void DrawAvailableSessions()
		{

			if (NetworkManager.availableServers.Count < 1)
			{
				GraphicsManager.drawText("Keine Spiele gefunden.", new Vector2(640 - GraphicsManager.font03.MeasureString("Keine Spiele gefunden").X/2, 192+32), GraphicsManager.font03, Color.White, true);
				GraphicsManager.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
				GraphicsManager.spriteBatch.Draw(sad, new Vector2(640 - sad.Width/2, 192+32+64), Color.White);
				GraphicsManager.spriteBatch.End();
			}

			for (int sessionIndex = 0; sessionIndex < NetworkManager.availableServers.Count; sessionIndex++)
			{
				Color c = Color.White;

				if (sessionIndex == selectedSessionIndex)
				{
					c = Color.Yellow;
				}

				GraphicsManager.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
				GraphicsManager.spriteBatch.Draw(playerspace, new Vector2(256, 192 + ((sessionIndex) * 192) + 32), Color.White);
				GraphicsManager.spriteBatch.End();

				GraphicsManager.drawText(NetworkManager.availableServers[sessionIndex] + "'s Spiel", new Vector2(288, 192 + ((sessionIndex) * 192) + 32 + 72), GraphicsManager.font03, c, true);


			}
		}






		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Begin();
			GraphicsManager.spriteBatch.Draw(background, backPos, Color.White);
			GraphicsManager.spriteBatch.Draw(front, frontPos, Color.White);
			GraphicsManager.spriteBatch.End();

			foreach (Button b in buttons)
			{
				b.Draw(gameTime);
			}

			if (this.state == MenuStates.HOST )
			{
				DrawLobby();
			}
			else if(this.state == MenuStates.JOIN)
			{
				DrawAvailableSessions();
			}

			base.Draw(gameTime);
		}
	}
}
