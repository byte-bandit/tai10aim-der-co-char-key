using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Adventure_Prototype;
using Classes;
using Classes.Pipeline;

namespace Classes.IO
{
	class InputManager
	{

		public static void Update(int gameMode)
		{
			if (gameMode == 0)
			{
				return;
			}

			
			
			if ( GameRef.Game.gameMode == Adventure_Prototype.Game1.GameMode.GAME)
			{
				if (KeyboardEx.isKeyHit(Keys.Tab))
				{

					if (GameRef.Inventory)
					{
						GameRef.Inventory = false;
						Inventory.Inventory.Visible = false;
						System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
						stopwatch.Start();
						while (stopwatch.ElapsedMilliseconds < (long)200)
						{
						}
					}
					else
					{
						GameRef.Inventory = true;
						Inventory.Inventory.Visible = true;
						System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
						stopwatch.Start();
						while (stopwatch.ElapsedMilliseconds < (long)200)
						{
						}
					}
				}
			}

			
			//Handling Mouse Scrolls
			if (MouseEx.scrollDown())
			{
				if (!GameRef._EDITOR)
				{
					switch (Cursor.CurrentAction )
					{
						case Cursor.CursorAction.walk:
							Cursor.CurrentAction = Cursor.CursorAction.talk;
							break;

						case Cursor.CursorAction.talk:
							Cursor.CurrentAction = Cursor.CursorAction.look;
							break;

						case Cursor.CursorAction.look:
							Cursor.CurrentAction = Cursor.CursorAction.use;
							break;

						case Cursor.CursorAction.use:
							Cursor.CurrentAction = Cursor.CursorAction.walk;
							break;
					}
				}
			}





			//Handling Mouse Scrolls - Rev
			if (MouseEx.scrollUp())
			{
				if (!GameRef._EDITOR)
				{
					switch (Cursor.CurrentAction)
					{
						case Cursor.CursorAction.walk:
							Cursor.CurrentAction = Cursor.CursorAction.use;
							break;

						case Cursor.CursorAction.talk:
							Cursor.CurrentAction = Cursor.CursorAction.walk;
							break;

						case Cursor.CursorAction.look:
							Cursor.CurrentAction = Cursor.CursorAction.talk;
							break;

						case Cursor.CursorAction.use:
							Cursor.CurrentAction = Cursor.CursorAction.look;
							break;
					}
				}
			}


		}
	}
}
