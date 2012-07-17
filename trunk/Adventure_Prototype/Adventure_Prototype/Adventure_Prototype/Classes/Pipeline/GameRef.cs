//This static Class is ONLY used for holding a Reference to our Game1 Instance, so whenever you need
//a Game Instance, like for:
//			DrawableGameComponent foo = new DrawableGameComponent(Game game);
//you can simple use GameRef.Game. Example:
//
//			class foo : DrawableGameComponent
//			{
//				public foo(String text, Int32 zahl, Game game) : base(game)
//				{
//					//Crazy Mudbutt
//				}
//			}
//
//
//Now goes like:
//
//			class foo : DrawableGameComponent
//			{
//				public foo(String text, Int32 zahl) : base(GameRef.Game)
//				{
//					//Crazy Mudbutt
//				}
//			}
//
//Of course, you will have to use the Classes.Pipeline Namespace for that
//technique to work.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Adventure_Prototype;
using Microsoft.Xna.Framework;

namespace Classes.Pipeline
{
	static class GameRef
	{
		private static Game1 game;
		private static Boolean editor;
		private static Boolean inventory;
		private static Vector2 resolution;
		private static Vector2 animationFrames;
		private static Player player1;
		private static Player player2;


		/// <summary>
		/// Gets or sets the Main game.
		/// You should only WRITE to this property ONCE during Initialization of the game
		/// </summary>
		public static  Game1 Game
		{
			get { return game; }
			set { game = value; }
		}





		/// <summary>
		/// Gets or sets the MaxFramesX and MaxFramesY for animations
		/// </summary>
		public static Vector2 AnimationFrames
		{
			get { return animationFrames; }
			set { animationFrames = value; }
		}



		/// <summary>
		/// Gets or Sets player 1
		/// </summary>
		public static Player Player1
		{
			get { return player1; }
			set { player1 = value; }
		}





		/// <summary>
		/// Gets or Sets player 2
		/// </summary>
		public static Player Player2
		{
			get { return player2; }
			set { player2 = value; }
		}





		/// <summary>
		/// Gets or sets the current Resolution of the game
		/// </summary>
		public static Vector2 Resolution
		{
			get { return resolution; }
			set { resolution = value; }
		}


		
		/// <summary>
		/// Gets or sets whether the game booted up in editor mode
		/// </summary>
		public static Boolean _EDITOR
		{
			get { return editor; }
			set { editor = value; }
		}

		public static Boolean Inventory
		{
			get { return inventory; }
			set { inventory = value; }
		}
	}
}
