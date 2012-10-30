using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Classes.Pipeline;

namespace Classes.Events
{
	class EventManager
	{

		private static SortedList<String, Event> EventLibrary;

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
                    Event tmp = new Event(id.ToLower());
                    n++;
                    while (!data[n].StartsWith("ENDEVENT"))
                    {
                        if (data[n].StartsWith("BEGINACTION:"))
                        {
                            List<string> content = new List<string>();
                            n++;
                            while (!data[n].StartsWith("ENDACTION:"))
                            {
                                content.Add(data[n]);
                                n++;
                            }
                            Classes.Events.Action action = new Classes.Events.Action(content);
                            tmp.Actions.Add(action);
                            n++;
                        }
                        if (data[n].Contains("DEPENDENCE:"))
                        {
                            while (data[n].Contains("DEPENDENCE:"))
                            {
                                string event_id = data[n].Substring(data[n].IndexOf("DEPENDENCE:") + 11).Trim();
                                try
                                {
                                    tmp.Dependencies.Add(EventLibrary[event_id]);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                                    return;
                                }
                                n++;
                            }
                        } 
                    }

                }

            }

        }

        public static void ExecuteEvent(String ID)
        {

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
                                break; 
                            }
                        case Action.type.GiveItem:
                            { 
                                break; 
                            }
                        case Action.type.RemoveItem:
                            {
                                GameRef.Inventory.RemoveItem((Inventory.Item) a.Target1);
                                break; 
                            }
                        case Action.type.RemoveObject:
                            {
                                SceneryManager.CurrentRoom.removeObject((Graphics.WorldObject) a.Target1);
                                break; 
                            }
                        case Action.type.StartDialogue:
                            {
								Dialogues.DialogueManager.startDialogue(a.TargetID, a.TargetCharacter);
                                break; 
                            }
                        case Action.type.WalkTo:
                            {
                                a.TargetCharacter.setWalkingTarget(a.TargetVector);
                                break; 
                            }
                    }

                }

                EventLibrary[ID].Executed = true;
            }
        }
	}
}
