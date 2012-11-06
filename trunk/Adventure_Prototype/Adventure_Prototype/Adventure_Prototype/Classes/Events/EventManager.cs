using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Classes.Inventory;
using Classes.Pipeline;

namespace Classes.Events
{
	class EventManager
	{

		public static SortedList<String, Event> EventLibrary;

		/// <summary>
		/// Adds an Event to the EventManager for Handling
		/// </summary>
		/// <param name="e"></param>
		public static void addEvent(Event e)
		{
            EventLibrary.Add(e.ID, e);
		}

		/// <summary>
		/// Removes an Event from the Event Manager. Returns true if succeeded and false if not possible to remove
		/// </summary>
		/// <param name="e">The Event to be removed from the Manager.</param>
		/// <returns></returns>
		public static bool removeEvent(Event e)
		{
			try
			{
                EventLibrary.Remove(e.ID);
				return true;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				return false;
			}
		}

		/// <summary>
		/// Update Logic for Event Manager. Call every  frame
		/// </summary>
		public static void Update()
		{

			//e.check(); <=== Was not found?
		}

        public static void Initialize()
        {
            String[] data;
            EventLibrary = new SortedList<String, Event>();

            try
            {
                data = System.IO.File.ReadAllLines("Content/Events.txt");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return;
            }

            for (int n = 0; n < data.Length; n++)
            {
                String line = data[n];

                if (line.Trim().StartsWith("#"))
                {
                    //Comentary Line - ignore and get next line
                    continue;
                }

                if (line.Trim().StartsWith("BEGINEVENT"))
                {
                    n++;
                    //Get Id of the event
                    String id = data[n].Substring(data[n].IndexOf("ID:") + 3);
                    Event tmp = new Event(id.ToLower().Trim());
                    n++;
                    while (!data[n].StartsWith("ENDEVENT"))
                    {
                        if (data[n].StartsWith("BEGINACTION:"))
                        {
                            List<string> content = new List<string>();
                            n++;
                            while (!data[n].StartsWith("ENDACTION"))
                            {
                                content.Add(data[n]);
                                n++;
                            }
                            Classes.Events.Action action = new Classes.Events.Action(content);
                            tmp.Actions.Add(action);
                            n++;
							continue;
                        }
						if (data[n].StartsWith("REPEATABLE"))
						{
							tmp.Repeatable = true;
							n++;
							continue;
						}
                        if (data[n].StartsWith("COMBTYPW"))
                        {
                            tmp.CombTyp = Event.combtyp.WorldObject;
                            n++;
                            continue;
                        }
                        if (data[n].StartsWith("COMBTYPI"))
                        {
                            tmp.CombTyp = Event.combtyp.InventoryItem;
                            n++;
                            continue;
                        }
                        if (data[n].StartsWith("COMBITEM:"))
                        {
                            tmp.Repeatable = true;
                            n++;
                            continue;
                        }
                        if (data[n].Contains("DEPENDENCE:"))
                        {
                            while (data[n].Contains("DEPENDENCE:"))
                            {
                                string event_id = data[n].Substring(data[n].IndexOf("DEPENDENCE:") + 11).Trim();
                                try
                                {
                                    tmp.Dependencies.Add(event_id);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                                    return;
                                }
                                n++;
								
                            }
							continue;
                        }

						//If we reach this point, we have an empty line in the file
						n++;
 						
                    }

					//Needs to add the event to the lib
					EventLibrary.Add(tmp.ID, tmp);

                }

            }

        }

        public static void ExecuteEvent(String ID)
        {
			ID = ID.ToLower().Trim();
            if (EventLibrary[ID] == null)
            {
                throw new System.ArgumentException("ExecuteEvent:no such event defined");
            }
            if (!EventLibrary[ID].Executed && EventLibrary[ID].checkDependencies())
            {
                if (EventLibrary[ID].Actions.Count == 0)
                {
                    //if no action is defined, use alternative
                    
                }
                foreach (Action a in EventLibrary[ID].Actions)
                {
                    switch (a.Typ)
                    {
                        case Action.type.AddObject:
                            { 
                                Graphics.WorldObject wo = new Graphics.WorldObject(GameRef.Game, "Graphics/Sprites/" + a.TargetID);
                                wo.Name = a.TargetName;
                                wo.OnLook = a.OnLook;
                                wo.OnTalk = a.OnTalk;
                                wo.OnUse = a.OnUse;
                                wo.Position = a.TargetVector;
                                SceneryManager.CurrentRoom.addObject(wo);
                                break; 
                            }
                        case Action.type.GiveItem:
                            {
                                if (GameRef.ItemManager.CheckForItem(a.TargetID))
                                {
                                    GameRef.Inventory.AddItem(GameRef.ItemManager.ItemLibrary[a.TargetID]);
                                }
                                else
                                {
                                    a.TargetID = "green_bottle";
                                }

                                
                                break; 
                            }
                        case Action.type.RemoveItem:
                            {
                                GameRef.Inventory.RemoveItem((Inventory.Item) a.Target1);
                                break; 
                            }
                        case Action.type.RemoveObject:
                            {
                                SceneryManager.CurrentRoom.removeObject(SceneryManager.CurrentRoom.getWorldObjectByID(a.TargetID));
                                break; 
                            }
                        case Action.type.StartDialogue:
                            {
								NPC target = null;
								foreach (NPC n in SceneryManager.CurrentRoom.getNPCs())
								{
									if (n.Name == a.TargetCharacter)
									{
										target = n;
										break;
									}
								}

								if (target != null)
								{
									Dialogues.DialogueManager.startDialogue(a.TargetID, target);
								}
                                break; 
                            }
                        case Action.type.WalkTo:
                            {
								NPC target = null;

								foreach (NPC n in SceneryManager.CurrentRoom.getNPCs())
								{
									if (n.Name == a.TargetCharacter)
									{
										target = n;
										break;
									}
								}

								if (target != null)
								{
									target.setWalkingTarget(a.TargetVector);
								}

                                
                                break; 
                            }
						case Action.type.DisableControls:
							{
								if (Net.NetworkManager.Profile.Puppet.id.Contains(a.TargetID))
								{
									Net.NetworkManager.Profile.ControlsActive = false;
								}
								break;
							}
						case Action.type.EnableControls:
							{
								if (Net.NetworkManager.Profile.Puppet.id.Contains(a.TargetID))
								{
									Net.NetworkManager.Profile.ControlsActive = true;
								}
								break;
							}
						case Action.type.Sleep:
							{
								//currently not supported lol
								break;
							}
						case Action.type.PlayerWalkTo:
							{
								if (Net.NetworkManager.Profile.Puppet.id.Contains(a.TargetID))
								{
									Net.NetworkManager.Profile.Puppet.setWalkingTarget(a.TargetVector);
									Net.NetworkManager.setPlayerWaypoint(a.TargetVector);
								}
								break;
							}
						case Action.type.PortToRoom:
							{
								RoomTransporter.Transport(a.TargetID, a.TargetVector, a.TargetVector2);
								break;
							}
                    }

                }

				if (!EventLibrary[ID].Repeatable)
				{
					EventLibrary[ID].Executed = true;
				}
            }
        }
	}
}
