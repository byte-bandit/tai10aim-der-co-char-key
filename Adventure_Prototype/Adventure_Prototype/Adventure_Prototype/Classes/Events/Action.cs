using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Classes;
using Classes.Pipeline;

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
        private String targetCharacter;
        private String targetID;
        private String targetName;
        private String onUse;
        private String onLook;
        private String onTalk;

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

        public String TargetCharacter
        {
            get { return this.targetCharacter; }
            set { this.targetCharacter = value; }
        }

        public string TargetID
        {
            get { return this.targetID; }
            set { this.targetID = value; }
        }

        public string TargetName
        {
            get { return this.targetName; }
            set { this.targetName = value; }
        }

        public string OnUse
        {
            get { return this.onUse; }
            set { this.onUse = value; }
        }

        public string OnLook
        {
            get { return this.onLook; }
            set { this.onLook = value; }
        }

        public string OnTalk
        {
            get { return this.onTalk; }
            set { this.onTalk = value; }
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
        public Action(List<String> Content)
        {

            switch(Content[0].ToLower().Trim())
            {
                case "walkto":
                    {
                        this.typ = type.WalkTo;
						//foreach(Character ch in SceneryManager.CurrentRoom.getNPCs())
						//{
						//    if(Content[1] == ch.Name)
						//    {
						//        this.targetCharacter = ch;
						//        break;
						//    }
						//}
						this.targetCharacter = Content[1];
                        try
                        {
                            this.targetVector.X = Convert.ToInt16(Content[2].Trim());
                            this.targetVector.Y = Convert.ToInt16(Content[3].Trim());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Input string is not a sequence of digits.");
                        }
                        break;
                    }
                case "giveitem":
                    {
                        this.typ = type.GiveItem;
                        this.targetID = Content[1].Trim();
                        break;
                    }
                case "removeitem":
                    {
                        this.typ = type.RemoveItem;
                        foreach(Inventory.Item i in GameRef.Inventory.Items)
                        {
                            if( i.ID == Content[1].Trim())
                            {
                                this.target1 = i;
                                this.TargetID = i.ID;
                                break;
                            }
                        }

                        break;
                    }
                case "addobject":
                    {
                        this.typ = type.AddObject;
                        this.targetID = Content[1].Trim();
                        try
                        {
                            this.targetVector.X = Convert.ToInt32(Content[2].Trim());
                            this.targetVector.Y = Convert.ToInt32(Content[3].Trim());
                        }
                        catch (FormatException e)
                        {
                            Console.WriteLine("Input string is not a sequence of digits.");
                        }
                        this.targetName = Content[4].Trim();
                        this.onLook = Content[5].Trim();
                        this.onTalk = Content[6].Trim();
                        this.onUse = Content[7].Trim();
                        break;
                    }
                case "removeobject":
                    {
                        this.typ = type.RemoveObject;
                        this.TargetID = Content[1].Trim();
                        break;
                    }
				case "startdialogue":
					{
						this.typ = type.StartDialogue;
						this.targetID = Content[1].Trim();
						this.targetCharacter = Content[2];
						break;
					}

            }
            //if this particular action type is not defined, an exception is thrown
            if(this.typ == type.Unknown)
            {
                throw new System.ArgumentException("action type not defined");
            }

           
        }

        
    }
}
