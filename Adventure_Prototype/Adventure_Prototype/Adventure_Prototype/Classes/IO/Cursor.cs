using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Classes.Graphics;

namespace Classes.IO
{
	static class Cursor
	{

		private static Texture2D c_walk;
		private static Texture2D c_talk;
		private static Texture2D c_look;
		private static Texture2D c_use;
		private static Texture2D c_dialogue;
		private static CursorAction currentAction;


		/// <summary>
		/// Displays all possible Cursor states
		/// </summary>
		public enum CursorAction
		{
			walk,
			talk,
			look,
			use
		}



		/// <summary>
		/// Gets or sets the current Action of the Cursor
		/// </summary>
		public static CursorAction CurrentAction
		{
			get { return currentAction; }
			set { currentAction = value; }
		}


		/// <summary>
		/// Initializes the Cursor Control for use
		/// </summary>
		/// <param name="game"></param>
		public static void Initialize(Game game)
		{
			c_walk = game.Content.Load<Texture2D>("Graphics/Cursor/cursor_walk");
			c_talk = game.Content.Load<Texture2D>("Graphics/Cursor/cursor_talk");
			c_look = game.Content.Load<Texture2D>("Graphics/Cursor/cursor_look");
			c_use = game.Content.Load<Texture2D>("Graphics/Cursor/cursor_use");
			c_dialogue = game.Content.Load<Texture2D>("Graphics/Cursor/cursor_dialogue");
			currentAction = CursorAction.walk;
		}




		/// <summary>
		/// Draws the current Cursor Texture to the Mouse Position
		/// </summary>
		public static void Draw()
		{
			GraphicsManager.spriteBatch.Begin();


			//In Dialogue Mode?
			if (Dialogues.DialogueManager.busy)
			{
				GraphicsManager.spriteBatch.Draw(c_dialogue, MouseEx.Position(), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
				GraphicsManager.spriteBatch.End();
				return;
			}


			//So, which Action do we currently have selected?
			switch (currentAction)
			{
				case CursorAction.walk:
					GraphicsManager.spriteBatch.Draw(c_walk, MouseEx.Position(), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
					break;

				case CursorAction.talk:
					GraphicsManager.spriteBatch.Draw(c_talk, MouseEx.Position(), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
					break;

				case CursorAction.look:
					GraphicsManager.spriteBatch.Draw(c_look, MouseEx.Position(), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
					break;

				case CursorAction.use:
					GraphicsManager.spriteBatch.Draw(c_use, MouseEx.Position(), null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 1.0f);
					break;
			}

			GraphicsManager.spriteBatch.End();
		}
	}
}
