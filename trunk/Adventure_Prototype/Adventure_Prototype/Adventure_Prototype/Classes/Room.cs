using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Classes.Graphics;
using Classes.IO;

using Classes.Pathfinding;

namespace Classes
{
	public class Room : DrawableGameComponent
	{
		private String id;
		private Texture2D background;
		private Game game;
		private String backgroundTString;
		private List<WorldObject> objects;
		private List<NPC> npcs;
		private List<POI> pois;
		private bool scaleCharacters;
		private int scaleCharactersMinPos;
		private int scaleCharactersMaxPos;
		private float scaleCharactersMin;
		private float scaleCharactersMax;
		private Polygon walkAreas;
		private String infoText;

		public Room(Game game, String id, String backgroundTexture)
			: base(game)
		{
			this.backgroundTString = backgroundTexture;
			this.game = game;
			this.id = id;
			this.scaleCharacters = false;
			this.objects = new List<WorldObject>();
			this.npcs = new List<NPC>();
			this.pois = new List<POI>();
			this.LoadContent();
		}

		public Polygon WalkAreas
		{
			get { return this.walkAreas; }
			set { this.walkAreas = value; }
		}

		public String TextureString
		{
			get { return this.backgroundTString; }
		}

		public List<POI> POIS
		{
			get { return this.pois; }
			set { this.pois = value; }
		}


		public String ID
		{
			get { return this.id; }
		}

		/// <summary>
		/// Adds a World Object to the room
		/// </summary>
		/// <param name="wObject">The World Object to add to the Room</param>
		public void addObject(WorldObject wObject)
		{
			this.objects.Add(wObject);
		}


		public Vector4 getScalingParams()
		{
			if (this.ScaleCharacters == false)
			{
				return Vector4.Zero;
			}

			return new Vector4(this.scaleCharactersMinPos, this.scaleCharactersMin, this.scaleCharactersMaxPos, this.scaleCharactersMax);
		}


		public bool ScaleCharacters
		{
			get { return this.scaleCharacters; }
		}


		/// <summary>
		/// Sets the Room up for scaling the players
		/// </summary>
		/// <param name="minLevel"></param>
		/// <param name="maxLevel"></param>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		public void ConfigureScaling(int minLevel, int maxLevel, float minValue, float maxValue)
		{
			this.scaleCharacters = true;
			this.scaleCharactersMinPos = minLevel;
			this.scaleCharactersMaxPos = maxLevel;
			this.scaleCharactersMin = minValue;
			this.scaleCharactersMax = maxValue;
		}


		/// <summary>
		/// Adds an NPC to the Room
		/// </summary>
		/// <param name="npc">The NPC to add to the Room</param>
		public void addNPC(NPC npc)
		{
			this.npcs.Add(npc);
		}


		/// <summary>
		/// Removes a World Object from the Room
		/// </summary>
		/// <param name="wObject"></param>
		/// <returns></returns>
		public bool removeObject(WorldObject wObject)
		{
			return this.objects.Remove(wObject);
		}



		/// <summary>
		/// Removes an NPC from the Room
		/// </summary>
		/// <param name="npc">The NPC to remove from the Room</param>
		/// <returns></returns>
		public bool removeNPC(NPC npc)
		{
			return this.npcs.Remove(npc);
		}


		/// <summary>
		/// Gets a List of all World Objects in the Room
		/// </summary>
		/// <returns></returns>
		public List<WorldObject> getWorldObjects()
		{
			return this.objects;
		}


		/// <summary>
		/// Gets a List of all NPCs in the Room
		/// </summary>
		/// <returns></returns>
		public List<NPC> getNPCs()
		{
			return this.npcs;
		}


		protected override void LoadContent()
		{
			this.background = game.Content.Load<Texture2D>(backgroundTString);
			base.LoadContent();
		}

		public override void Update(GameTime gameTime)
		{

			//Check Mouse Over
			MouseOverStuff();

			//Check Click
			MouseClickStuff();


			base.Update(gameTime);
		}






		private void MouseClickStuff()
		{
			//Mouse Click World Object?
			foreach (WorldObject w in this.objects)
			{
				if (MouseEx.clickInRectangle(w.getDrawingRect()))
				{
					switch (Cursor.CurrentAction)
					{
						case Cursor.CursorAction.look:
							Dialogues.DialogueManager.PlayerSay(w.OnLook);
							break;

						case Cursor.CursorAction.talk:
							Dialogues.DialogueManager.PlayerSay(w.OnTalk);
							break;

						case Cursor.CursorAction.use:
							Dialogues.DialogueManager.PlayerSay(w.OnUse);
							break;
					}
					return;
				}
			}





			//Mouse Click POI?
			foreach (POI w in this.pois)
			{
				if (MouseEx.clickInPolygon(w))
				{
					switch (Cursor.CurrentAction)
					{
						case Cursor.CursorAction.look:
							Dialogues.DialogueManager.PlayerSay(w.onLook);
							break;

						case Cursor.CursorAction.talk:
							Dialogues.DialogueManager.PlayerSay(w.onTalk);
							break;

						case Cursor.CursorAction.use:
							Dialogues.DialogueManager.PlayerSay(w.onUse);
							break;
					}
					return;
				}
			}

			//Mouse Click NPC?
			foreach (NPC n in this.npcs)
			{
				if (MouseEx.clickInRectangle(n.getDrawingRect()))
				{
					switch (Cursor.CurrentAction)
					{
						case Cursor.CursorAction.look:
							Dialogues.DialogueManager.PlayerSay(n.OnLook);
							break;

						case Cursor.CursorAction.talk:
							if (n.OnTalk.Trim().StartsWith("{D:"))
							{
								Dialogues.DialogueManager.startDialogue(n.OnTalk.Trim().Substring(3, n.OnTalk.Trim().Length - 4));
							}
							else
							{
								Dialogues.DialogueManager.PlayerSay(n.OnTalk);
							}

							break;

						case Cursor.CursorAction.use:
							Dialogues.DialogueManager.PlayerSay(n.OnUse);
							break;
					}
				}
			}
		}





		private void MouseOverStuff()
		{

			//Reset Info Text
			infoText = "";

			//Mouse Over World Object?
			foreach (WorldObject w in this.objects)
			{
				if (MouseEx.inBoundaries(w.getDrawingRect()))
				{
					infoText = w.Name;
					return;
				}
			}


			//Mouse Over Point of Interest?
			foreach (POI w in this.pois)
			{
				if (MouseEx.inPolygon(w))
				{
					infoText = w.Name;
					return;
				}
			}

			//Mouse Over NPC?
			foreach (NPC n in this.npcs)
			{
				if (MouseEx.inBoundaries(n.getDrawingRect()))
				{
					Character tmp = (Character)n;
					infoText = tmp.Name;
					return;
				}
			}
		}





		private void displayInfoText(String text)
		{
			Graphics.GraphicsManager.drawText(text, Vector2.Add(MouseEx.Position(), new Vector2(-10, -30)), Graphics.GraphicsManager.font02, Color.Yellow, true);
		}






		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Begin();
			GraphicsManager.spriteBatch.Draw(this.background, Vector2.Zero, Color.White);
			GraphicsManager.spriteBatch.End();

			foreach (WorldObject o in this.objects)
			{
				o.Draw(gameTime);
			}

			foreach (NPC n in this.npcs)
			{
				n.Draw(gameTime);
			}

			//Draw Text
			displayInfoText(infoText);

			base.Draw(gameTime);
		}

	}
}
