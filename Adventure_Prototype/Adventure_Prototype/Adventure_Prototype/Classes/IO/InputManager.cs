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
        private static int update_interval = 50;
        private static int update_step = 0;

        public static void Update(int gameMode)
        {
            if (gameMode == 0)
            {
                return;
            }


            if (GameRef.Game.gameMode == Adventure_Prototype.Game1.GameMode.GAME)
            {

                if (KeyboardEx.isKeyHit(Keys.Tab) && (update_step == 0))
                {
                    if (GameRef.Inventory.Status)
                    {
                        GameRef.Inventory.Status = false;
                    }
                    else
                    {
                        GameRef.Inventory.Status = true;
                    }
                    update_step = update_interval;

                }
                else
                {
                    if (update_step > 0)
                    {
                        update_step--;
                    }
                }
            }



            //Handling Mouse Scrolls
            if (MouseEx.scrollDown())
            {
                if (!GameRef._EDITOR)
                {
                    switch (Cursor.CurrentAction)
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
