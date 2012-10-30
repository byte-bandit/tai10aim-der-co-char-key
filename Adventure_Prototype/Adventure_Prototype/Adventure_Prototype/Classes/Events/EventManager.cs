using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;



namespace Classes.Events
{
	class EventManager
	{

		private static List<Event> events = new List<Event>();



		/// <summary>
		/// Adds an Event to the EventManager for Handling
		/// </summary>
		/// <param name="e"></param>
		public static void addEvent(Event e)
		{
			events.Add(e);
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
				events.Remove(e);
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
			foreach (Event e in events)
			{
				//e.check(); <=== Was not found?
			}
		}


	}
}
