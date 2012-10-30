using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Classes;

namespace Classes.Events
{
    public class Action
    {
        public enum type
        {
            Unknown,
            WalkTo,
            GiveItem,
            RemoveItem,
            AddObject,
            RemoveObject,
            StartDialogue,
        }
        private type typ;
        private object target1;
        private object target2;
        private Vector2 targetVector;
        private Character targetCharacter;
        private String targetID;

        #region Properties
        public type Typ
        {
            get { return this.typ; }
            set { this.typ = value; }
        }
        public object Target1
        {
            get { return this.target1; }
            set { this.target1 = value; }
        }
        public object Target2
        {
            get { return this.target2; }
            set { this.target2 = value; }
        }
        public Vector2 TargetVector
        {
            get { return this.targetVector; }
            set { this.targetVector = value; }
        }

        public Character TargetCharacter
        {
            get { return this.targetCharacter; }
            set { this.targetCharacter = value; }
        }

        public string TargetID
        {
            get { return this.targetID; }
            set { this.targetID = value; }
        }

        #endregion

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
                int X = new Int32();
                int Y = new Int32();
                string player_id = Content.Substring(0, Content.IndexOf(","));
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
            }

            if (Content.Contains("giveitem"))
            {
                this.typ = type.GiveItem;
                Content = Content.Substring(Content.IndexOf("giveitem,") + 9);
                string player_id = Content.Substring(0, Content.IndexOf(","));
                string id = Content.Substring(Content.IndexOf(",") + 1).Trim();
            }

            if(Content.Contains("removeitem"))
            {
                this.typ = type.RemoveItem;
                Content = Content.Substring(Content.IndexOf("removeitem,") +11);
                string player_id = Content.Substring(0, Content.IndexOf(","));
                string id = Content.Substring(Content.IndexOf(",") + 1).Trim();
            }
            
            if(Content.Contains("addobject"))
            {
                this.typ = type.AddObject;
                Content = Content.Substring(Content.IndexOf("addobject,") + 10);
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
            }

            if(Content.Contains("removeobject"))
            {
                this.typ = type.RemoveObject;
                string id = Content.Substring(Content.IndexOf("removeobject,") + 13).Trim();
            }

            if (Content.Contains("startdialogue"))
            {
                this.typ = type.StartDialogue;
                this.targetID = Content.Substring(Content.IndexOf("startdialogue,") + 14); ;
            }

            //if this particular action type is not defined, an exception is thrown
            if(this.typ == type.Unknown)
            {
                throw new System.ArgumentException("action type not defined");
            }


            switch (this.typ)
            {

                case type.GiveItem:
                    {
                        
                        break;
                    }
                case type.RemoveItem:
                    {
                        
                        break;
                    }
                case type.RemoveObject:
                    {
                        
                        break;
                    }
                case type.StartDialogue:
                    {
                        
                        break;
                    }
            }

                
            
        }

        
    }
}
