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
using Classes.Pipeline;
using Classes.Events;
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

		/// <summary>
		/// Constructor for Room
		/// </summary>
		/// <param name="game"></param>
		/// <param name="id"></param>
		/// <param name="backgroundTexture"></param>
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

		#region Properties



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
		#endregion

		/// <summary>
		/// Helper Method
		/// Generates a Pause between actions
		/// </summary>
		/// <param name="_time">the time to wait</param>
		public static void Wait(int _duration)
		{
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			while (stopwatch.ElapsedMilliseconds < (long)1000 * _duration)
			{
			}
			stopwatch.Stop();
		}

		/// <summary>
		/// Adds a World Object to the room
		/// </summary>
		/// <param name="wObject">The World Object to add to the Room</param>
		public void addObject(WorldObject wObject)
		{
			this.objects.Add(wObject);
		}

		/// <summary>
		///		
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Loads the necessary content for the room 
		/// </summary>
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

		/// <summary>
		/// Checks whether an object is declared as an triggerobject of the current List of Actions
		/// </summary>
		/// <param name="w"></param>
		/// <param name="typ"></param>

		/// <summary>
		/// Helper Method to find an NPC by his name
		/// </summary>
		/// <param name="name">name of the NPC</param>
		/// <returns></returns>
		private NPC FindNPCbyName(string name)
		{
			foreach (NPC npc in this.npcs)
			{
				if (npc.Name.ToLower().Equals(name.ToLower()))
				{
					return npc;
				}
			}

			return null;
		}



		/// <summary>
		/// Checks all World Objects, POI´s and NPC if the mouse has been clicked on them
		/// </summary>
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
							infoRoute(w.OnLook.Trim(), w);
							break;

						case Cursor.CursorAction.talk:
							infoRoute(w.OnTalk.Trim(), w);
							break;

						case Cursor.CursorAction.use:
							infoRoute(w.OnUse.Trim(), w);
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
							infoRoute(w.onLook.Trim(), w);
							break;

						case Cursor.CursorAction.talk:
							infoRoute(w.onTalk.Trim(), w);
							break;

						case Cursor.CursorAction.use:
							infoRoute(w.onUse.Trim(), w);
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
							infoRoute(n.OnLook.Trim(), n);
							break;

						case Cursor.CursorAction.talk:
							infoRoute(n.OnTalk.Trim(), n);
							break;

						case Cursor.CursorAction.use:
							infoRoute(n.OnUse.Trim(), n);
							break;
					}
				}
			}
		}

		/// <summary>
		/// Checks if the mouse hovers over any WOrld Object, POI or NPC and displays accordingly an infotext
		/// </summary>
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



		/// <summary>
		/// Analyzes given input for Dialogue or Script functions and delegates.
		/// </summary>
		private void infoRoute(String info, Entity actor)
		{
			if (info.StartsWith("{D:"))
			{
				Dialogues.DialogueManager.startDialogue(info.Substring(3, info.Length - 4), actor);
			}
			else if (info.StartsWith("{S:"))
			{
                Events.EventManager.ExecuteEvent(info.Substring(3, info.Length - 4));
			}
			else
			{
				Dialogues.DialogueManager.PlayerSay(info);
				Net.NetworkManager.PlayerSay(info, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
			}
		}






		/// <summary>
		/// helper method for drawing an infotext at the mouseposition
		/// </summary>
		/// <param name="text">text to be displayed</param>
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
