using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Classes;

namespace Classes.Events
{
    private class Action
    {
        private enum type
        {
            Unknown,
            WalkTo,
            GiveItem,
            RemoveItem,
            AddObject,
            RemoveObject,
        }
        private type typ;
        private object target;
        private object target2;
        private Vector2 targetVector;
        private Character targetCharacter;

        public Action()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Content">is a string with the following structure TYPE,PARAM,PARAM
        /// </param>
        public Action(String Content)
        {
            Content = Content.ToLower();

            if (Content.Contains("walk"))
            {
                this.typ = type.WalkTo;
                Content = Content.Substring(Content.IndexOf("walkto,") + 7);
            }

            if (Content.Contains("giveitem"))
            {
                this.typ = type.GiveItem;
                Content = Content.Substring(Content.IndexOf("giveitem,") + 9);
            }

            if(Content.Contains("removeitem"))
            {
                this.typ = type.RemoveItem;
                Content = Content.Substring(Content.IndexOf("removeitem,") +11);
            }
            
            if(Content.Contains("addobject"))
            {
                this.typ = type.AddObject;
                Content = Content.Substring(Content.IndexOf("addobject,") + 10);
            }

            if(Content.Contains("removeobject"))
            {
                this.typ = type.RemoveObject;
                Content = Content.Substring(Content.IndexOf("removeobject,") + 13);
            }

            //if this particular action type is not defined, an exception is thrown
            if(this.typ == type.Unknown)
            {
                throw new System.ArgumentException("action type not defined");
            }


            switch (this.typ)
            {
                case type.WalkTo:
                    {
                        int X = new Int32();
                        int Y = new Int32();
                        string player_id = Content.Substring(0, Content.IndexOf(","));
                        Content = Content.Substring(Content.IndexOf(",")+1);
                        try
                        {
                            X = Convert.ToInt32(Content.Substring(0, Content.IndexOf("x")).Trim());
                            Y = Convert.ToInt32(Content.Substring(Content.IndexOf("x") + 1).Trim());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Input string is not a sequence of digits.");
                        }
                        this.targetVector = new Vector2(X, Y);
                        break;
                    }
                case type.AddObject:
                    {
                        int X = new Int32();
                        int Y = new Int32();
                        string id = Content.Substring(0, Content.IndexOf(","));
                        Content = Content.Substring(Content.IndexOf(",") + 1);
                        try
                        {
                            X = Convert.ToInt32(Content.Substring(0, Content.IndexOf("x")).Trim());
                            Y = Convert.ToInt32(Content.Substring(Content.IndexOf("x") + 1).Trim());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Input string is not a sequence of digits.");
                        }
                        this.targetVector = new Vector2(X, Y);
                        break;
                    }
                case type.GiveItem:
                    {
                        string player_id = Content.Substring(0, Content.IndexOf(","));
                        string id = Content.Substring(Content.IndexOf(",") + 1).Trim();
                        break;
                    }
                case type.RemoveItem:
                    {
                        string player_id = Content.Substring(0, Content.IndexOf(","));
                        string id = Content.Substring(Content.IndexOf(",") + 1).Trim();
                        break;
                    }
                case type.RemoveObject:
                    {
                        string id = Content.Trim();
                        break;
                    }
            }

                
            
        }
    }
}
