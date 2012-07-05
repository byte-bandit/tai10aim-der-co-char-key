using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adventure_Prototype.Classes.Actions
{
	class ActionManager
	{
		private static bool isBusy = false;
		private List<ActionList> actionLibrary;
		public List<ActionList> ActionLibrary
		{
			get { return actionLibrary; }
		}


		public static void ToggleisBusy()
		{
			if (isBusy)
			{
				isBusy = false;
			}
			else
			{
				isBusy = true;
			}
		}




		public static void Initialize(String path)
		{
			String[] txt = null;

			try
			{
				txt = System.IO.File.ReadAllLines("Content/Roomevent.txt");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
				return;
			}


			String ID = String.Empty;

			foreach (String line in txt)
			{

				if (line.Trim().StartsWith("#"))
				{
					//Commentary Line - ignore and get next line
					continue;
				}



				if (line.Trim().StartsWith("@ROOM"))
				{

				}

				if (line.Trim().StartsWith("EVENT"))
				{

				}

				if (line.Trim().StartsWith("ACTION"))
				{

				}



				//Get next line
				continue;
			}

		
		}
	}
}
