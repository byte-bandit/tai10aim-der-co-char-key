/*
 * POINT AND CLICK ADVENTURE FOR DHBW MANNHEIM SE PROJEKT 8
 * CODENAME: ?
 * AUTHORS : LUCAS HILDEBRANDT; CHRISTIAN LOHR; FELIX OTTO
 * ENGINE  : BRUNNEN-G
 * 
 * Main Class. Leave as is!
 * 
 */



/*######################################################################################
 * Imports and uses
 * #####################################################################################
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;


using Classes;
using Classes.Dialogues;
using Classes.Graphics;
using Classes.IO;
using Classes.Dev;
using Classes.Pipeline;
using Classes.Net;
using Classes.Inventory;





//######################################################################################
//Namespace Definition: Make sure to change respective to build status
//######################################################################################
namespace Adventure_Prototype
{





	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{

		//######################################################################################
		//Global Variable Definition
		//######################################################################################
		GraphicsDeviceManager graphics;		//Global graphics card Interface
		SpriteBatch spriteBatch;			//Global spriteBatch used for Drawing
		//public Player player1;						//Link to Player1
		//public Player player2;						//Link to Player2
		public Menu menu = new Menu();				//Main Menu Variable
		Texture2D p1Sprite;
		Texture2D p2Sprite;


		public GameMode gameMode = GameMode.MAIN_MENU_CONNECT_OR_HOST; // Set to not logged in for initial start screen
		public Boolean _EDITOR = false;		//Boot up in Editor mode? [SUPPOSED TO BE FALSE FOR RELEASE]
		public float musicVolume = 1.0f;	//Use this to mute music for testing purposes
		


		public enum GameMode
		{
			MAIN_MENU_CONNECT_OR_HOST,
			MAIN_MENU_HOST,
			MAIN_MENU_CONNECT,
			GAME,
			GAME_MENU,
		}


		/// <summary>
		/// Game Constructor
		/// </summary>
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);		//Initialize our Graphics Card Interface
			Content.RootDirectory = "Content";				//Setting up our Content-Root Directory
			
			//Killing all game Server instances
			this.Disposed += new EventHandler<EventArgs>(Game1_Disposed);
			
			//*le setting screen resolution
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
		}



		/// <summary>
		/// Handles Game Disposal
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void Game1_Disposed(object sender, EventArgs args)
		{
			try
			{
				System.Diagnostics.Process[] pc = System.Diagnostics.Process.GetProcessesByName("GameServer");

				foreach (System.Diagnostics.Process p in pc)
				{
					p.Kill();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
			}
		}






		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{

			//Set Mouse to invisible in Game Mode
			//Setup our Cursor Class (Load images)
			if (!_EDITOR)
			{
				IsMouseVisible = false;
				Cursor.Initialize(this);
			}
			else
			{
				IsMouseVisible = true;
			}
			


			//Setup our Game Reference for use with Instances
			GameRef.Game = this;
			GameRef.Inventory = new Inventory();
			GameRef._EDITOR = this._EDITOR;
			GameRef.Resolution = new Vector2(1280, 720);
			GameRef.AnimationFrames = new Vector2(6, 3);
			
			

			//Generic Initializing Procedure
			RoomProcessor.Initialize(this);		//Builds our room from source files [.bmap]
			KeyboardEx.Initialize();			//A more powerful keyboard class
			DialogueManager.Initialize();		//Get all Dialogues
			NetworkManager.Initialize();
			Components.Add(GameRef.Inventory);
			
			//Load up all our fonts
			GraphicsManager.initializeFonts(Content.Load<SpriteFont>("ui_font"), Content.Load<SpriteFont>("big"), Content.Load<SpriteFont>("menu_font"), GraphicsDevice);
				
			//Initialize Parent
			base.Initialize();
			//LoadContent();
		}





		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{

			// Create a new SpriteBatch, which can be used to draw textures.
			// Assign it to the Graphics Manager
			spriteBatch = new SpriteBatch(GraphicsDevice);
			GraphicsManager.spriteBatch = spriteBatch;
			SoundManager.LoadContent();
			
			

			//TEMPORARY CREATION OF OUR PLAYER
			//THIS WILL BE CHANGED LATER ON - SO DONT RELY ON IT !!
			if (!_EDITOR)
			{
				//Room testRoom = RoomProcessor.createRoomFromFile("Data/Rooms/test1.bmap");
				//SceneryManager.CurrentRoom = testRoom;
				p1Sprite = Content.Load<Texture2D>("Graphics/Charsets/spriteA");
				p2Sprite = Content.Load<Texture2D>("Graphics/Charsets/spriteA");
				//player1 = new Player(this, testRoom, "player01", "Darksvakthaniel", new Animation(p1Sprite.Width, p1Sprite.Height, 6, 3, 0, 0, false), p1Sprite, 1.5f);
				//player1.Position = new Vector2(200, 500);
				//GameRef.Player1 = player1;
				//GraphicsManager.addChild(player1);
				//UpdateManager.addItem(player1);

				menu.Initialize();

			}
			else
			{
				Editor.Initialize(this); //- WTF is this doing here?!
			}

		}

		
	
		

		
		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			
			// TODO: Unload any non ContentManager content here
		}





		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// Allows the game to exit
			if (KeyboardEx.isKeyHit(Keys.Escape))
				this.Exit();
			

			//Update our Managers
			UpdateManager.Update(gameTime);
			if (this.gameMode == GameMode.GAME) 
			{ 
				InputManager.Update(1); 
			
			} 
			else 
			{ 
				InputManager.Update(0); 
			}
			
			DialogueManager.Update();

			if (!_EDITOR && menu != null)
			{
				menu.Update(gameTime);
			}
			

			//Update the Editor
			if (_EDITOR)
				Editor.Update();
			else
				NetworkManager.Update();

			//Focus?
			if (!this.IsActive || !MouseEx.inBoundaries(new Rectangle(0, 0, 1280, 720)))
			{
				return;
			}

			MouseEx.Update();
			KeyboardEx.Update();

			//SEND OUR PLAYER WALKING IF IN GAME MODE
			// WILL BE CHANGED TO INPUTMANAGER EVENTUALLY
			if (MouseEx.click() && Cursor.CurrentAction == Cursor.CursorAction.walk && !DialogueManager.busy && !_EDITOR && gameMode == GameMode.GAME)
			{
				NetworkManager.Profile.Puppet.setWalkingTarget(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
				NetworkManager.setPlayerWaypoint(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
			}


			
		}





		public void StartNewGame()
		{
			this.gameMode = GameMode.GAME;
			this.menu = null;

			SceneryManager.CurrentRoom = RoomProcessor.createRoomFromFile("Data/Rooms/DE_INFORMATION.bmap");

			SceneryManager.Player1 = new Player(this, SceneryManager.CurrentRoom , "p1", "Spieler 1", new Animation(p1Sprite.Width, p1Sprite.Height, 6, 3, 0, 0, false), p1Sprite, 1.5f);
			SceneryManager.Player2 = new Player(this, SceneryManager.CurrentRoom , "p2", "Spieler 2", new Animation(p1Sprite.Width, p1Sprite.Height, 6, 3, 0, 0, false), p1Sprite, 1.5f);

			SceneryManager.Player1.Position = new Vector2(200, 600);
			SceneryManager.Player2.Position = new Vector2(400, 600);

			SceneryManager.Player1.SetFloatingLineColor(Color.LightGreen);
			SceneryManager.Player2.SetFloatingLineColor(Color.Orange);

			if (NetworkManager.IsHost)
			{
				NetworkManager.Profile.Puppet = SceneryManager.Player1;
			}else{
				NetworkManager.Profile.Puppet = SceneryManager.Player2;
			}

			UpdateManager.addItem(SceneryManager.Player1);
			UpdateManager.addItem(SceneryManager.Player2);

			GraphicsManager.addChild(SceneryManager.Player1);
			GraphicsManager.addChild(SceneryManager.Player2);
		}




		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{

			//Let the GM handle the GFXOutput
			GraphicsManager.Draw(gameTime);

			//Menu
			if (!_EDITOR && menu != null)
			{
				menu.Draw(gameTime);
			}

			//Editor takes over if necessary
			if (_EDITOR)
				Editor.Draw(this, gameTime);

			//Draw Dialogue
			if (!_EDITOR)
			{
				DialogueManager.draw(GraphicsManager.font02, gameTime );
			}

			//Draw some debug info
			if (gameMode == GameMode.GAME)
			{
				GraphicsManager.drawText("Puppet: " + NetworkManager.Profile.Puppet.Name, new Vector2(20, 20), GraphicsManager.font02, Color.White, true);
				GraphicsManager.drawText("X: " + NetworkManager.Profile.Puppet.Position.X.ToString() + ", Y: " + NetworkManager.Profile.Puppet.Position.Y.ToString(), new Vector2(20, 40), GraphicsManager.font02, Color.White, true);
				GraphicsManager.drawText("Name: " + NetworkManager.Profile.Name, new Vector2(20, 60), GraphicsManager.font02, Color.White, true);
				//GraphicsManager.drawText("Token: " + NetworkManager.Profile.Token, new Vector2(20, 74), GraphicsManager.font02, Color.White, true);
			}

			//Draw Cursor at last
			if(!_EDITOR && this.IsActive && MouseEx.inBoundaries(new Rectangle(0,0,1280,720)))
				Cursor.Draw();
			
			base.Draw(gameTime);
		}
	}
}

//E O F
