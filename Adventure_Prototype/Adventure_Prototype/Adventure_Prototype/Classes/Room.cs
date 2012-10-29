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
using Classes.Action;
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
		private List<List<Actions>> events;

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
			this.events = new List<List<Actions>>();
		}

		#region Properties

		public List<List<Actions>> Events
		{
			get { return events; }
			set { this.events = value; }
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
		private void CheckForEvents(Object w, Cursor.CursorAction typ)
		{
			try
			{
				List<Actions> eventlist = this.Events.First<List<Actions>>();
				foreach (Actions a in eventlist)
				{
					if ((a.Trigger == w) && (a.Typ == typ))
					{
						TriggerEvent(a.Effects);
						eventlist.Remove(a);
						// MAybe potential problem
						break;
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
			}
		}

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
		/// Triggers an Event and therefor all effects
		/// </summary>
		/// <param name="effects"></param>
		private void TriggerEvent(List<string> effects)
		{
			string help = String.Empty;
			List<string> copy = new List<string>();
			foreach (string t in effects)
			{
				t.Trim();
				switch (t.Substring(0, t.IndexOf(" ")))
				{
					case "AddNPC":
						{
							copy.Add(t);
							break;
						}

					case "ChangeTalk":
						{
							break;
						}

					case "ChangeUse":
						{
							break;
						}
					case "ChangeWalk":
						{
							break;
						}
					case "ChangeTake":
						{
							break;
						}

					case "NPCWalk":
						{
							help = t.Substring(t.IndexOf(" ") + 1);
							string position = help.Substring(help.IndexOf(" ") + 1);
							Vector2 target = new Vector2(Int32.Parse(position.Substring(0, position.IndexOf(";")+1)), Int32.Parse(position.Substring(position.IndexOf(";")+1)));
							if (FindNPCbyName(help.Substring(0, help.IndexOf(" "))) != null)
							{
								FindNPCbyName(help.Substring(0, help.IndexOf(" "))).setWalkingTarget(target);
							}
							copy.Add(t);

							break;
						}

					case "RemoveNPC":
						{
							help = t.Substring(t.IndexOf(" ")+1);
							if (FindNPCbyName(help) != null)
							{
								this.removeNPC(FindNPCbyName(help));
							}
							copy.Add(t);
							break;
						}

					case "Wait":
						{
							help = t.Substring(0, t.IndexOf(" ")+1);
							help.Replace(" ", "");
							Wait(int.Parse(help));
							copy.Add(t);
							break;
						}
						// to be continued...
					default:
						{
							// to clean up the mess
							copy.Add(t);
							break;
						}
				}
			}
			foreach (string l in copy)
			{
				foreach (string t in effects)
				{
					if (l.Equals(t))
					{
						effects.Remove(t);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Is called, when Inventory focus is not null and Cursor is set at Use. 
		/// Checks if the two Objects are combinable.
		/// </summary>
		/// <param name="w"></param>
		private void CheckForCombination(object w)
		{
			// to be continued...
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
							CheckForEvents(w, Cursor.CursorAction.look);
							Dialogues.DialogueManager.PlayerSay(w.OnLook);
							Net.NetworkManager.PlayerSay(w.OnLook, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
							break;

						case Cursor.CursorAction.talk:
							CheckForEvents(w, Cursor.CursorAction.talk);
							Dialogues.DialogueManager.PlayerSay(w.OnTalk);
							Net.NetworkManager.PlayerSay(w.OnTalk, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
							break;

						case Cursor.CursorAction.use:
							if (GameRef.Inventory.Focus != null)
							{
								CheckForCombination(w);
							}
							else
							{
								CheckForEvents(w, Cursor.CursorAction.use);
							}
							Net.NetworkManager.PlayerSay(w.OnUse, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
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
							Net.NetworkManager.PlayerSay(w.onLook, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
							break;

						case Cursor.CursorAction.talk:
							if (w.onTalk.Trim().StartsWith("{D:"))
							{
								Dialogues.DialogueManager.startDialogue(w.onTalk.Trim().Substring(3, w.onTalk.Trim().Length - 4), w);
							}
							else
							{
								Dialogues.DialogueManager.PlayerSay(w.onTalk);
								Net.NetworkManager.PlayerSay(w.onTalk, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
							}
							break;

						case Cursor.CursorAction.use:
							Dialogues.DialogueManager.PlayerSay(w.onUse);
							Net.NetworkManager.PlayerSay(w.onUse, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
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
							Net.NetworkManager.PlayerSay(n.OnLook, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
							break;

						case Cursor.CursorAction.talk:
							if (n.OnTalk.Trim().StartsWith("{D:"))
							{
								Dialogues.DialogueManager.startDialogue(n.OnTalk.Trim().Substring(3, n.OnTalk.Trim().Length - 4), n);
							}
							else
							{
								Dialogues.DialogueManager.PlayerSay(n.OnTalk);
								Net.NetworkManager.PlayerSay(n.OnTalk, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
							}

							break;

						case Cursor.CursorAction.use:
							Dialogues.DialogueManager.PlayerSay(n.OnUse);
							Net.NetworkManager.PlayerSay(n.OnUse, Net.NetworkManager.Profile.Puppet.Position, Net.NetworkManager.Profile.Puppet.GetFloatingLineColor());
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
