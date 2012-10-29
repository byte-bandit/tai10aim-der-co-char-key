using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

using Classes.Pipeline;
using Classes.IO;
using Classes.Graphics;
using Classes.Pathfinding;

using Adventure_Prototype;

namespace Classes.Dev
{
	class Editor
	{

		//+++++++++++++++++++++++++++++++++++++++++++++++
		//Variable Declaration
		//+++++++++++++++++++++++++++++++++++++++++++++++

		//Output - Used by Editor to print Info
		private static StringBuilder edOut;
		private static String VersionNumber = "0.2.3 [Brunnis-2]";
		private static String helpmsg = "Press Strg-H to see the help";
		private static String helpString = "Strg-N : New Room\nStrg-O Open existing Room\nTAB : Change Mode\nE : Create Walk Map\nW : Place World Objects\nN : Create NPCs\nQ: Create POI";


		//Some Variables to decide wether Users is dragging or not
		private static Vector2 dragOrigin;
		private static bool isDragging = false;
		private static Vector2 draggingPoint;


		//Keeping track of our walkable Areas
		private static Polygon walkAreas;
		

		//Editor has two main modes and numerous submodes. 
		private static EditorModeTypes mode = EditorModeTypes.Creation; //The two main modes are creation and manipulation
		private static CreationStatusTypes creationmode = CreationStatusTypes.WalkArea; //The sub modes for creation concentrate on wether to edit a walkArea, and Object, etc, etc...


		//Some Variables to check against selection
		private static Object selection = new Object();	//Storing our current selection
		private static Vector2 SelectionVortex;			//Keeps distance vector between mouse and selection positino
		private static Vector2 resizePivot = Vector2.Zero;
		

		//Variables for working with WorldObjects
		private static List<Texture2D> WOLib = new List<Texture2D>();
		private static Texture2D WorldObjectToPlace;
		private static int WorldObjectToPlaceIndex;
		private static List<WorldObject> WorldObjects = new List<WorldObject>();
		private static WorldObject selectedWorldObject;


		//Variables for working with NPCs
		private static List<NPC> NPCs = new List<NPC>();
		private static List<Texture2D> NPCLib = new List<Texture2D>();
		private static Texture2D NPCToPlace;
		private static int NPCToPlaceIndex;
		private static SpriteEffects NPCToPlaceSE = SpriteEffects.None;


		//Variables for working with Points of Interest
		private static List<POI> POIs = new List<POI>();
		private static POI currentPOI = null;

		


		/*
		 * In creation mode, this specifies which asset type to work with
		 */
		private enum CreationStatusTypes
		{
			WalkArea,
			WorldObject,
			NPC,
			POI
		}


		/*
		 * The editor can run in two modes - creation and manipulation
		 */
		private enum EditorModeTypes
		{
			Creation,
			Manipulation
		}




		/// <summary>
		/// This will save the map in a file.
		/// </summary>
		private static void saveMap()
		{
			edOut.AppendLine("Saving Walk-Areas...");
			StringBuilder fileout = new StringBuilder();
			fileout.AppendLine("# Brunnen-G Adventure Engine Room File");
			fileout.AppendLine("# Created: " + DateTime.Now.ToString());
			fileout.AppendLine("# By: " + Environment.UserName.ToString() + " - alias: " + Environment.UserDomainName.ToString());
			fileout.AppendLine("# On: " + Environment.MachineName.ToString() + " - " + Environment.OSVersion.ToString());
			fileout.AppendLine("# 64Bit: " + Environment.Is64BitOperatingSystem.ToString());
			fileout.AppendLine("# Number of Processors: " + Environment.ProcessorCount.ToString());
			fileout.AppendLine("# Mapped Memory: " + Environment.WorkingSet.ToString());
			fileout.AppendLine("# This File is automatically created by the Brunnen-G Adventure Engine EDITOR! V" + VersionNumber);
			fileout.AppendLine("");
			fileout.AppendLine("BEGIN");
			fileout.AppendLine("BEGINROOM");
			fileout.AppendLine("ID:" + SceneryManager.CurrentRoom.ID + ",TEXTURE:" + SceneryManager.CurrentRoom.TextureString);
			fileout.AppendLine("SCALING:" + SceneryManager.CurrentRoom.ScaleCharacters.ToString());
			fileout.AppendLine("ENDROOM");
			foreach (Vector2 v in walkAreas.Nodes)
			{
				fileout.AppendLine("BEGINWALK");
				fileout.AppendLine("X:" + v.X + ",Y:" + v.Y);
				fileout.AppendLine("ENDWALK");

			}
			foreach (WorldObject r in WorldObjects)
			{
				fileout.AppendLine("BEGINWO");
				fileout.AppendLine("X:" + r.Position.X + ",Y:" + r.Position.Y);
				fileout.AppendLine("TEXTURE:" + r.Texture);
				fileout.AppendLine("NAME:" + r.Name);
				fileout.AppendLine("ONLOOK:" + r.OnLook);
				fileout.AppendLine("ONUSE:" + r.OnUse);
				fileout.AppendLine("ONTALK:" + r.OnTalk);
				fileout.AppendLine("ENDWO");

			}
			foreach (NPC r in NPCs)
			{
				fileout.AppendLine("BEGINNPC");
				fileout.AppendLine("X:" + r.Position.X + ",Y:" + r.Position.Y);
				fileout.AppendLine("TEXTURE:" + r.GFX);
				fileout.AppendLine("MIRROR:" + r.IsMirrored);
				fileout.AppendLine("NAME:" + r.Name);
				fileout.AppendLine("ONLOOK:" + r.OnLook);
				fileout.AppendLine("ONUSE:" + r.OnUse);
				fileout.AppendLine("ONTALK:" + r.OnTalk);
				fileout.AppendLine("ENDNPC");
			}
			foreach (POI r in POIs)
			{
				fileout.AppendLine("BEGINPOI");
				foreach (Vector2 v in r.Nodes)
				{
					fileout.AppendLine("X:" + v.X + ",Y:" + v.Y);
				}
				fileout.AppendLine("NAME:" + r.Name);
				fileout.AppendLine("ONLOOK:" + r.onLook);
				fileout.AppendLine("ONUSE:" + r.onUse);
				fileout.AppendLine("ONTALK:" + r.onTalk);
				fileout.AppendLine("ENDPOI");
			}
			fileout.AppendLine("END");

			try
			{
				String filename = Environment.CurrentDirectory + "..\\..\\..\\..\\Resource\\maps\\";
				System.IO.Directory.CreateDirectory(filename);
				filename = filename + SceneryManager.CurrentRoom.ID.ToString() + ".bmap";
				System.IO.File.WriteAllText(filename, fileout.ToString());

				filename = Environment.CurrentDirectory + "\\Content\\Data\\Rooms\\";
				System.IO.Directory.CreateDirectory(filename);
				filename = filename + SceneryManager.CurrentRoom.ID.ToString() + ".bmap";
				System.IO.File.WriteAllText(filename, fileout.ToString());

				System.Diagnostics.ProcessStartInfo sinfo = new System.Diagnostics.ProcessStartInfo(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\notepad.exe", filename);

				System.Diagnostics.Process.Start(sinfo);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
			}
		}



		private static void toggleMode()
		{
			if (mode == EditorModeTypes.Creation)
			{
				mode = EditorModeTypes.Manipulation;
			}
			else
			{
				mode = EditorModeTypes.Creation;
				selection = new Vector2();
				SelectionVortex = Vector2.Zero;
			}
		}






		private static void openMapFile()
		{
			String files = "";
			foreach (String s in System.IO.Directory.GetFiles("Content/Data/Rooms/"))
			{
				files += (s.Substring(s.LastIndexOf(@"/") + 1, s.LastIndexOf(".") - s.LastIndexOf(@"/") - 1)) + "\n";
			}

			String name = Microsoft.VisualBasic.Interaction.InputBox(files, "Room name");

			if (!System.IO.File.Exists("Content/Data/Rooms/" + name + ".bmap"))
			{
				Microsoft.VisualBasic.Interaction.MsgBox("Failed to find Background file. Quitting app.");
				name = "";
				GameRef.Game.Exit();
				return;

			}
			SceneryManager.CurrentRoom = RoomProcessor.createRoomFromFile("Data/Rooms/" + name + ".bmap");
			walkAreas = SceneryManager.CurrentRoom.WalkAreas;
			WorldObjects = SceneryManager.CurrentRoom.getWorldObjects();
			NPCs = SceneryManager.CurrentRoom.getNPCs();
			POIs = SceneryManager.CurrentRoom.POIS;


			if (walkAreas.Nodes.Count < 1)
			{
				walkAreas.Changeable = true;
			}
		}




		private static void generateEditorOutput()
		{
			edOut = new StringBuilder();
			edOut.AppendLine("Brunnen-G Adventure Engine 0.0.3 EDITOR V." + VersionNumber + "\t" + helpmsg);
			edOut.AppendLine("Mouse at: " + Mouse.GetState().X + ", " + Mouse.GetState().Y + "\nRoom: " + SceneryManager.CurrentRoom.ID + "\tMode: " + mode.ToString());
		}





		private static void UpdateCreation()
		{

			//Set creationmode to WorldObject
			if (KeyboardEx.isKeyHit(Keys.W))
			{
				creationmode = CreationStatusTypes.WorldObject;
			}



			//Set creationmode to POI
			if (KeyboardEx.isKeyHit(Keys.Q))
			{
				creationmode = CreationStatusTypes.POI;
			}


			//Set Creationmode to WalkArea
			if (KeyboardEx.isKeyHit(Keys.E))
			{
				creationmode = CreationStatusTypes.WalkArea;
			}


			//Set Creationmode to NPCs
			if (KeyboardEx.isKeyHit(Keys.N))
			{
				creationmode = CreationStatusTypes.NPC;
			}

			//Which Creationmode are we in?
			switch (creationmode)
			{
				case CreationStatusTypes.WorldObject:
					UpdateCreation_WO();
					break;

				case CreationStatusTypes.WalkArea:
					UpdateCreation_WA();
					break;

				case CreationStatusTypes.POI:
					UpdateCreation_POI();
					break;

				case CreationStatusTypes.NPC:
					UpdateCreation_NPC();
					break;
			}
		}






		private static void UpdateCreation_NPC()
		{

			//Place selected NPC
			if (MouseEx.click())
			{
				Vector2 mp = MouseEx.Position();

				String tName = Microsoft.VisualBasic.Interaction.InputBox("Name");
				String tOnLook = Microsoft.VisualBasic.Interaction.InputBox("OnLook");
				String tOnTalk = Microsoft.VisualBasic.Interaction.InputBox("OnTalk");
				String tOnUse = Microsoft.VisualBasic.Interaction.InputBox("OnUse");

				NPC tmp = new NPC(GameRef.Game, SceneryManager.CurrentRoom, "c_" + tName.ToLower(), tName, new Animation(NPCToPlace.Width, NPCToPlace.Height, 6, 3, 0, 0, false), NPCToPlace);
				tmp.Position = mp;

				if (NPCToPlaceSE == SpriteEffects.None)
				{
					tmp.IsMirrored = false;
				}
				else
				{
					tmp.IsMirrored = true;
				}

				tmp.Name = tName;
				tmp.OnLook = tOnLook;
				tmp.OnTalk = tOnTalk;
				tmp.OnUse = tOnUse;


				NPCs.Add(tmp);
			}


			//Change selected NPC
			if (MouseEx.scrollDown())
			{
				int n = NPCLib.Count - 1;

				if (NPCToPlaceIndex == n)
				{
					NPCToPlaceIndex = -1;
				}

				NPCToPlaceIndex++;

				NPCToPlace = NPCLib[NPCToPlaceIndex];
			}

			//Change selected NPC
			if (MouseEx.scrollUp())
			{
				if (NPCToPlaceIndex == 0)
				{
					NPCToPlaceIndex = NPCLib.Count;
				}

				NPCToPlaceIndex--;

				NPCToPlace = NPCLib[NPCToPlaceIndex];
			}

			//Mirror NPC
			if (MouseEx.rightClick())
			{
				if (NPCToPlaceSE == SpriteEffects.None)
				{
					NPCToPlaceSE = SpriteEffects.FlipHorizontally;
				}
				else
				{
					NPCToPlaceSE = SpriteEffects.None;
				}
			}
		}







		private static void UpdateCreation_WA()
		{

			//if (MouseEx.pressed_LMB())
			//{
			//    drawNewArea();
			//}
			//else
			//{
			//    if (isDragging)
			//    {
			//        stopDragging();
			//        if (walkAreas.Changeable)
			//        {

			//            if (walkAreas.Nodes.Count > 2 && walkAreas.firstNodeInRange(draggingPoint))
			//            {
			//                Vector2 tmp = walkAreas.Nodes[0];
			//                walkAreas.Nodes.Add(tmp);
			//                walkAreas.Changeable = false;
			//                Microsoft.VisualBasic.Interaction.MsgBox("Walkare Complete.");
			//            }
			//            else
			//            {
			//                walkAreas.Nodes.Add(draggingPoint);
			//            }
			//        }

			//    }
			//}

			if (MouseEx.click())
			{
				if (walkAreas.Changeable)
				{

					if (walkAreas.Nodes.Count > 2 && walkAreas.firstNodeInRange(MouseEx.Position()))
					{
						Vector2 tmp = walkAreas.Nodes[0];
						walkAreas.Nodes.Add(tmp);
						walkAreas.Changeable = false;
						Microsoft.VisualBasic.Interaction.MsgBox("Walkare Complete.");
					}
					else
					{
						walkAreas.Nodes.Add(MouseEx.Position());
					}
				}
			}

		}







		private static void UpdateCreation_POI()
		{

			edOut.AppendLine("Creating Point of Interest");

			if (MouseEx.click())
			{
				draggingPoint = MouseEx.Position();
				if (currentPOI == null)
				{
					currentPOI = new POI(true, GameRef.Game);
				}

				if (currentPOI.Changeable)
				{

					if (currentPOI.Nodes.Count > 2 && currentPOI.firstNodeInRange(draggingPoint))
					{
						Vector2 tmp = currentPOI.Nodes[0];
						currentPOI.Nodes.Add(tmp);
						currentPOI.Changeable = false;

						String tName = Microsoft.VisualBasic.Interaction.InputBox("Name");
						String tOnLook = Microsoft.VisualBasic.Interaction.InputBox("OnLook");
						String tOnTalk = Microsoft.VisualBasic.Interaction.InputBox("OnTalk");
						String tOnUse = Microsoft.VisualBasic.Interaction.InputBox("OnUse");

						currentPOI.Name = tName;
						currentPOI.onLook = tOnLook;
						currentPOI.onTalk = tOnTalk;
						currentPOI.onUse = tOnUse;

						POIs.Add(currentPOI);
						currentPOI = null;

					}
					else
					{
						currentPOI.Nodes.Add(draggingPoint);
					}
				}

			}
		}









		private static void UpdateCreation_WO()
		{
			edOut.AppendLine("Adding World Object: " + WorldObjectToPlace.Name);

			//Place selected WO
			if (MouseEx.click())
			{

				WorldObject tmp = new WorldObject(GameRef.Game, WorldObjectToPlace);
				tmp.Position = MouseEx.Position();

				String tName = Microsoft.VisualBasic.Interaction.InputBox("Name");
				String tOnLook = Microsoft.VisualBasic.Interaction.InputBox("OnLook");
				String tOnTalk = Microsoft.VisualBasic.Interaction.InputBox("OnTalk");
				String tOnUse = Microsoft.VisualBasic.Interaction.InputBox("OnUse");

				tmp.Name = tName;
				tmp.OnLook = tOnLook;
				tmp.OnTalk = tOnTalk;
				tmp.OnUse = tOnUse;

				WorldObjects.Add(tmp);
			}


			//Change selected WO
			if (MouseEx.scrollDown())
			{
				int n = WOLib.Count - 1;

				if (WorldObjectToPlaceIndex == n)
				{
					WorldObjectToPlaceIndex = -1;
				}

				WorldObjectToPlaceIndex++;

				WorldObjectToPlace = WOLib[WorldObjectToPlaceIndex];
			}

			//Change selected WO
			if (MouseEx.scrollUp())
			{
				if (WorldObjectToPlaceIndex == 0)
				{
					WorldObjectToPlaceIndex = WOLib.Count;
				}

				WorldObjectToPlaceIndex--;

				WorldObjectToPlace = WOLib[WorldObjectToPlaceIndex];
			}

		}




		private static void UpdateManipulation()
		{

			//Select an Object
			if (MouseEx.click())
			{
				MouseState m = MouseEx.getState();


				//Select a Walk Area
				walkAreas.Nodes.Reverse();
				foreach (Vector2 r in walkAreas.Nodes)
				{
					if (MouseEx.inRange(r))
					{
						selection = r;
					}
				}
				walkAreas.Nodes.Reverse();


				//Select a World Object
				WorldObjects.Reverse();
				foreach (WorldObject r in WorldObjects)
				{
					if (MouseEx.inBoundaries(r.getDrawingRect()))
					{
						selection = r;
					}
				}
				WorldObjects.Reverse();


				//Select an NPC
				NPCs.Reverse();
				foreach (NPC r in NPCs)
				{
					if (MouseEx.inBoundaries(r.getDrawingRect()))
					{
						selection = r;
					}
				}
				NPCs.Reverse();
			}




			//Move an Object
			if (MouseEx.pressed_LMB() && selection != null)
			{

				if (selection.GetType() == typeof(WorldObject))
				{
					moveWorldObject();
				}
				else if (selection.GetType() == typeof(Vector2))
				{
					moveWalkArea();
				}
				else if (selection.GetType() == typeof(NPC))
				{
					moveNPC();
				}
			}
			else
			{
				SelectionVortex = Vector2.Zero;
			}


			//Delete the selected World Object
			if (KeyboardEx.isKeyHit(Keys.Delete))
			{
				if (selection.GetType() == typeof(WorldObject))
				{
					WorldObjects.Remove((WorldObject)selection);
					SelectionVortex = Vector2.Zero;
				}




				//Delete the Walk Area

				if (selection.GetType() == typeof(Vector2))
				{
					walkAreas.Nodes.Remove((Vector2)selection);
					selection = null;
					SelectionVortex = Vector2.Zero;
				}




				//Delete the NPC

				if (selection.GetType() == typeof(NPC))
				{
					NPCs.Remove((NPC)selection);
					selection = null;
					SelectionVortex = Vector2.Zero;
				}
			}
		}







		private static void moveNPC()
		{
			//Selection == WO ?
			if (selection == null || selection.GetType() != typeof(NPC))
			{
				return;
			}


			//Find Selection
			for (int n = 0; n < NPCs.Count; n++)
			{
				if ((NPC)selection == NPCs[n] && MouseEx.inBoundaries(NPCs[n].getDrawingRect()))
				{
					//First run?
					if (SelectionVortex == Vector2.Zero)
					{
						SelectionVortex = new Vector2(Mouse.GetState().X - NPCs[n].Position.X, Mouse.GetState().Y - NPCs[n].Position.Y);
						return;
					}
					else
					{
						NPC tmp = NPCs[n];
						tmp.Position = new Vector2(Mouse.GetState().X - (int)SelectionVortex.X, Mouse.GetState().Y - (int)SelectionVortex.Y);
						NPCs[n] = tmp;
						return;
					}
				}
			}

		}








		private static void moveWorldObject()
		{
			//Selection == WO ?
			if (selection == null || selection.GetType() != typeof(WorldObject))
			{
				return;
			}


			//Find Selection
			for (int n = 0; n < WorldObjects.Count; n++)
			{
				if (selection == WorldObjects[n] && MouseEx.inBoundaries(WorldObjects[n].getDrawingRect()))
				{
					//First run?
					if (SelectionVortex == Vector2.Zero)
					{
						SelectionVortex = new Vector2(Mouse.GetState().X - WorldObjects[n].Position.X, Mouse.GetState().Y - WorldObjects[n].Position.Y);
						return;
					}
					else
					{
						WorldObject tmp = WorldObjects[n];
						tmp.Position = new Vector2(Mouse.GetState().X - (int)SelectionVortex.X, Mouse.GetState().Y - (int)SelectionVortex.Y);
						WorldObjects[n] = tmp;
						return;
					}
				}
			}

		}


		private static void moveWalkArea()
		{
			//Selection == WO ?
			if (selection == null || selection.GetType() != typeof(Vector2))
			{
				return;
			}
			

			//Find Selection
			for (int n = 0; n < walkAreas.Nodes.Count; n++)
			{
				if ((Vector2)selection == walkAreas.Nodes[n])
				{
					//First run?
					if (SelectionVortex == Vector2.Zero)
					{
						SelectionVortex = new Vector2(Mouse.GetState().X - walkAreas.Nodes[n].X, Mouse.GetState().Y - walkAreas.Nodes[n].Y);
						return;
					}
					else
					{
						Vector2 tmp = walkAreas.Nodes[n];
						tmp.X = Mouse.GetState().X - (int)SelectionVortex.X;
						tmp.Y = Mouse.GetState().Y - (int)SelectionVortex.Y;
						walkAreas.Nodes[n] = tmp;
						selection = walkAreas.Nodes[n];
						return;
					}
				}
			}

		}




		public static void Update()
		{

			//Generate General Editor Output i.e. Editor Version Number, etc...
			generateEditorOutput();



			//What Mode is the Editor currently running in?

			if (mode == EditorModeTypes.Creation)
			{
				UpdateCreation();
			}
			else
			{
				UpdateManipulation();
			}




			//Check whether the user wants to switch Editor Mode
			if (KeyboardEx.isKeyHit(Keys.Tab))
			{
				toggleMode();
				System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
				stopwatch.Start();
					while(stopwatch.ElapsedMilliseconds< (long)500)
					{
					}
			}



			//Check wether the user wants to Open an existing Map file
			if (KeyboardEx.isKeyHit(Keys.O) && KeyboardEx.isKeyDown(Keys.LeftControl))
			{
				openMapFile();
			}



			//Check whether the user wants to save the map
			if (KeyboardEx.isKeyHit(Keys.S) && KeyboardEx.isKeyDown(Keys.LeftControl))
			{
				saveMap();
			}


			//Toggle the Help Message
			if (KeyboardEx.isKeyHit(Keys.H) && KeyboardEx.isKeyDown(Keys.LeftControl))
			{
				toggleHelpDisplay();
			}


		}




		private static void toggleHelpDisplay()
		{
			if (helpmsg == "")
			{
				helpmsg = "Press H to see the help";
			}
			else
			{
				helpmsg = "";
			}
		}


		private static void setupScaling()
		{
			//IAsyncResult  Result = Guide.BeginShowKeyboardInput(PlayerIndex.One, "Input here", "You need to input text here", "[HERE YOU GO]", null, null);
		}






		private static void manipulateWO()
		{
			if (!isDragging)
			{
				isDragging = true;
				Vector2 m = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
				WorldObjects.Reverse();
				foreach (WorldObject r in WorldObjects)
				{
					if (m.X > r.Position.X && m.X < r.Position.X + r.size.X && m.Y > r.Position.Y && m.Y < r.Position.Y + r.size.Y)
					{
						selectedWorldObject = r;
						SelectionVortex = new Vector2(Mouse.GetState().X - selectedWorldObject.Position.X, Mouse.GetState().Y - selectedWorldObject.Position.Y);
						return;
					}
				}
				WorldObjects.Reverse();
			}
			else
			{
				for (int n = 0; n < WorldObjects.Count; n++)
				{
					if (WorldObjects[n] == selectedWorldObject)
					{
						selectedWorldObject.Position = new Vector2(Mouse.GetState().X - (int)SelectionVortex.X, Mouse.GetState().Y - (int)SelectionVortex.Y);
						WorldObjects[n] = selectedWorldObject;
						return;
					}
				}

				//selection.X = Mouse.GetState().X + (int)SelectionVortex.X;
				//selection.Y = Mouse.GetState().Y + (int)SelectionVortex.Y;
			}
		}







		private static void manipulate()
		{
			if (!isDragging)
			{
				isDragging = true;
				Vector2 m = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
				walkAreas.Nodes.Reverse();
				foreach (Vector2 r in walkAreas.Nodes)
				{
					if (Polygon.NodeInRange(m, r, 5))
					{
						//selection = r;
						//SelectionVortex = new Vector2(Mouse.GetState().X - selection.X, Mouse.GetState().Y - selection.Y);
						//return;
					}
				}
				walkAreas.Nodes.Reverse();
			}
			else
			{
				for (int n = 0; n < walkAreas.Nodes.Count; n++)
				{
					//if (walkAreas[n] == selection)
					//{
					//    selection.X = Mouse.GetState().X - (int)SelectionVortex.X;
					//    selection.Y = Mouse.GetState().Y - (int)SelectionVortex.Y;
					//    walkAreas[n] = selection;
					//    return;
					//}
				}

				//selection.X = Mouse.GetState().X + (int)SelectionVortex.X;
				//selection.Y = Mouse.GetState().Y + (int)SelectionVortex.Y;
			}
		}


		private static void drawNewArea()
		{
			if (!isDragging)
			{
				startDragging();
			}
			else
			{
				draggingPoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			}
			edOut.AppendLine("Create Walk-Node: X:" + draggingPoint.X + ", Y:" + draggingPoint.Y);
		}

		private static void stopDragging()
		{
			isDragging = false;
		}

		private static void startDragging()
		{
			dragOrigin = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			draggingPoint = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
			isDragging = true;
		}


		public static void Initialize(Game game)
		{
			walkAreas = new Polygon(true, game);
			foreach (String s in System.IO.Directory.GetFiles("Content/Graphics/Sprites/"))
			{
				String si = (s.Substring(s.LastIndexOf(@"/") + 1, s.LastIndexOf(".") - s.LastIndexOf(@"/") - 1));
				WOLib.Add(game.Content.Load<Texture2D>("Graphics/Sprites/" + si));
				WOLib[WOLib.Count - 1].Name = si;

			}

			WorldObjectToPlace = WOLib[0];





			foreach (String s in System.IO.Directory.GetFiles("Content/Graphics/Charsets/"))
			{
				String si = (s.Substring(s.LastIndexOf(@"/") + 1, s.LastIndexOf(".") - s.LastIndexOf(@"/") - 1));
				NPCLib.Add(game.Content.Load<Texture2D>("Graphics/Charsets/" + si));
				NPCLib[NPCLib.Count - 1].Name = si;

			}

			NPCToPlace = NPCLib[0];






			 switch (Microsoft.VisualBasic.Interaction.MsgBox("Yes = New Room, No = Open Room, Cancel = Quit", Microsoft.VisualBasic.MsgBoxStyle.YesNoCancel))
			{
				case Microsoft.VisualBasic.MsgBoxResult.Yes:
					createNewRoom(game);
					break;

				case Microsoft.VisualBasic.MsgBoxResult.No:
					openMapFile();
					break;

				case Microsoft.VisualBasic.MsgBoxResult.Cancel:
					game.Exit();
					break;
			}


		}





		private static void createNewRoom(Game game)
		{
			String files = "";
			foreach (String s in System.IO.Directory.GetFiles("Content/Graphics/Backgrounds/"))
			{
				files += (s.Substring(s.LastIndexOf(@"/") + 1, s.LastIndexOf(".") - s.LastIndexOf(@"/") - 1)) + "\n";
			}

			String name = Microsoft.VisualBasic.Interaction.InputBox("Name:");
			String texID = Microsoft.VisualBasic.Interaction.InputBox(files, "Background", "Graphics/Backgrounds/");

			if (!System.IO.File.Exists("Content/" + texID + ".xnb"))
			{
				Microsoft.VisualBasic.Interaction.MsgBox("Failed to find Background file. Quitting app.");
				texID = "Graphics/Backgrounds/room01";
				game.Exit();
				GameRef.Game.Exit();

			}

			SceneryManager.CurrentRoom = new Room(game, name, texID);
		}





		public static void Draw(Game game, GameTime gameTime)
		{
			Graphics.GraphicsManager.drawText(edOut.ToString(), new Vector2(5, 5), Graphics.GraphicsManager.font01, Color.White, true);


			if (helpmsg == "")
			{
				Graphics.GraphicsManager.drawText(helpString, new Vector2(650, 5), Graphics.GraphicsManager.font01, Color.White, true);
			}


			foreach (WorldObject w in WorldObjects)
			{
				if (selection != null && (selection.GetType() == typeof(WorldObject) && w == selection))
				{
					w.HighlightColor = Color.Red;
					if (w.Name != null)
					{
						GraphicsManager.drawText(w.Name, Vector2.Subtract(w.Position, new Vector2(-5, 10)), GraphicsManager.font01, Color.Green, true);
						GraphicsManager.drawText("X: " + w.Position.X.ToString() + ", Y: " + w.Position.Y.ToString(), Vector2.Add(w.Position, new Vector2(5, w.size.Y + 10)), GraphicsManager.font01, Color.Green, true);
					}
				}
				else
				{
					w.HighlightColor = Color.White;
				}
				w.Draw(gameTime);
			}


			//Graphics.GraphicsManager.spriteBatch.Begin();

			//if (isDragging && mode == EditorModeTypes.Creation)
			//{
			//    LineOverlay tmp;
			//    tmp = new LineOverlay(new Vector2(MouseEx.getState().X, MouseEx.getState().Y), draggingPoint, new Color(128, 255, 128, 128), game);
			//    tmp.Draw(gameTime);
			//}



			if (walkAreas.Nodes.Count > 1)
			{
				/*foreach (Vector2 r in walkAreas.Nodes)
				{*/
				LineOverlay tmp;

				for (int i = 1; i < walkAreas.Nodes.Count; i++)
				{
					tmp = new LineOverlay(walkAreas.Nodes[i], walkAreas.Nodes[i - 1], Color.White, GameRef.Game);
					tmp.Draw(gameTime);
				}

				/*if (selection != null && (selection.GetType() == typeof(Vector2) && r == (Vector2)selection))
					tmp = new LineOverlay(new Vector2(MouseEx.getState().X, MouseEx.getState().Y), r, new Color(256, 128, 256, 128), game);
				else
					tmp = new LineOverlay(new Vector2(MouseEx.getState().X, MouseEx.getState().Y), r, new Color(128, 128, 256, 128), game);*/


			}




			//Draw POI in Progress
			if (currentPOI != null && currentPOI.Nodes.Count > 1)
			{
				/*foreach (Vector2 r in walkAreas.Nodes)
				{*/
				LineOverlay tmp;

				for (int i = 1; i < currentPOI.Nodes.Count; i++)
				{
					tmp = new LineOverlay(currentPOI.Nodes[i], currentPOI.Nodes[i - 1], Color.Red, GameRef.Game);
					tmp.Draw(gameTime);
				}

				/*if (selection != null && (selection.GetType() == typeof(Vector2) && r == (Vector2)selection))
					tmp = new LineOverlay(new Vector2(MouseEx.getState().X, MouseEx.getState().Y), r, new Color(256, 128, 256, 128), game);
				else
					tmp = new LineOverlay(new Vector2(MouseEx.getState().X, MouseEx.getState().Y), r, new Color(128, 128, 256, 128), game);*/


			}



			//Draw done POIs
			foreach (POI p in POIs)
			{
				if (p.Nodes.Count > 1)
				{
					/*foreach (Vector2 r in walkAreas.Nodes)
					{*/
					LineOverlay tmp;

					for (int i = 1; i < p.Nodes.Count; i++)
					{
						tmp = new LineOverlay(p.Nodes[i], p.Nodes[i - 1], Color.Red, GameRef.Game);
						tmp.Draw(gameTime);
					}

					/*if (selection != null && (selection.GetType() == typeof(Vector2) && r == (Vector2)selection))
						tmp = new LineOverlay(new Vector2(MouseEx.getState().X, MouseEx.getState().Y), r, new Color(256, 128, 256, 128), game);
					else
						tmp = new LineOverlay(new Vector2(MouseEx.getState().X, MouseEx.getState().Y), r, new Color(128, 128, 256, 128), game);*/


				}
			}


			foreach (NPC w in NPCs)
			{
				if (selection != null && (selection.GetType() == typeof(NPC) && w == selection))
				{
					w.HighlightColor = Color.Red;
					if (w.Name != null)
					{
						GraphicsManager.drawText(w.Name, Vector2.Subtract(w.Position, new Vector2(-5, 10)), GraphicsManager.font01, Color.Green, true);
						GraphicsManager.drawText("X: " + w.Position.X.ToString() + ", Y: " + w.Position.Y.ToString(), Vector2.Add(w.Position, new Vector2(5, w.getDrawingRect().Height + 10)), GraphicsManager.font01, Color.Green, true);
					}
				}
				else
				{
					w.HighlightColor = Color.White;
				}
				w.Draw(gameTime);
			}




			if (mode == EditorModeTypes.Creation)
			{
				switch (creationmode)
				{
					case CreationStatusTypes.WorldObject:
						GraphicsManager.spriteBatch.Begin();
						GraphicsManager.spriteBatch.Draw(WorldObjectToPlace, new Vector2(MouseEx.X, MouseEx.Y), Color.White);
						GraphicsManager.spriteBatch.End();
						break;

					case CreationStatusTypes.NPC:
						GraphicsManager.spriteBatch.Begin();
						GraphicsManager.spriteBatch.Draw(NPCToPlace, new Rectangle(MouseEx.X, MouseEx.Y, NPCToPlace.Width / 6, NPCToPlace.Height / 3), new Rectangle(0, 0, NPCToPlace.Width / 6, NPCToPlace.Height / 3), Color.White, 0.0f, Vector2.Zero, NPCToPlaceSE, 1.0f);
						GraphicsManager.spriteBatch.End();
						break;
				}

			}
		}














		/*#########################################################################################
		*
		* O L D    L E G A C Y    J U N K
		* 
		*###########################################################################################*/

		//private static void resizeArea()
		//{
		//    if (selection != new Rectangle() && selection != null)
		//    {
		//        int lol=0;
		//        if (resizePivot == Vector2.Zero)
		//        {
		//            resizePivot = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
		//        }
		//        for (int n = 0; n < walkAreas.Count; n++)
		//        {
		//            if (walkAreas[n] == selection)
		//            {
		//                lol = (int)Vector2.Distance(resizePivot, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
		//                lol = lol / 10;
		//                if (lol == lastKnownSize)
		//                    break;
		//                lastKnownSize = lol;
		//                if (Mouse.GetState().X < resizePivot.X && Mouse.GetState().Y < resizePivot.Y)
		//                    lol *= -1;
		//                selection.Inflate(lol, lol);
		//                walkAreas[n] = selection;
		//                break;
		//            }
		//        }
		//        edOut.AppendLine("Resize Object: Pivot = " + resizePivot.X + ", " + resizePivot.Y + ", Scale: " + lol / 10);
		//        edOut.Append("    |    Object: X: " +selection.X+", Y: "+selection.Y+", Width: " + selection.Width + ", Height: " + selection.Height);
		//    }
		//}




		//static class EditorSelection
		//{
		//    private static WorldObject swo = null;
		//    private static NPC npc = null;
		//    private static Rectangle swa = new Rectangle();
		//    private static SelectionType selType;

		//    private enum SelectionType
		//    {
		//        WorldObject,
		//        WalkArea,
		//        NPC
		//    }

		//    public static WorldObject Selection_WorldObject
		//    {
		//        get { return swo; }
		//    }


		//}






		//String[] txt;
		//try
		//{
		//    txt = System.IO.File.ReadAllLines("Content/WorldObjects.txt");
		//}
		//catch (Exception ex)
		//{
		//    System.Diagnostics.Debug.WriteLine(ex.ToString());
		//    return;
		//}

		//foreach (String line in txt)
		//{
		//    if (line.Trim().StartsWith("#") || line.Trim().Length < 1)
		//    {
		//        continue;
		//    }
		//    WOLib.Add(game.Content.Load<Texture2D>("Graphics/Sprites/" + line));
		//    WOLib[WOLib.Count - 1].Name = line;
		//}
		//WorldObjectToPlace = WOLib[0];
	}

}
