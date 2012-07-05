using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Adventure_Prototype.Classes;
using Classes.Inventory;

namespace Classes.Actions
{
	class Action
	{

		#region Character Actions

		/// <summary>
		/// Adds a new NPC to the Scenery
		/// </summary>
		public static void SpawnNPC()
		{
			// to be completed
		}

		/// <summary>
		/// Sets the Target of a NPC to a specific point
		/// </summary>
		/// <param name="end">target vector</param>
		/// <param name="_Char">NPC which should act</param>
		public static void NPCGoTo(Vector2 end, Character _Char)
		{
		_Char.setWalkingTarget(end);
		}

		/// <summary>
		/// Player 1 gets a new Walking Target
		/// </summary>
		/// <param name="end">target vector</param>
		public static void P1GoTo(Vector2 end)
		{
			SceneryManager.Player1.setWalkingTarget(end);
		}


		/// <summary>
		/// Player 2 gets a new Walking Target
		/// </summary>
		/// <param name="end">target vector</param>
		public static void P2GoTo(Vector2 end)
		{
			SceneryManager.Player2.setWalkingTarget(end);
		}
#endregion

		#region Wait Methods
		/// <summary>
		/// Helper Method
		/// Generates a Pause between actions
		/// </summary>
		/// <param name="_time">the time to wait</param>
		public static void Wait(int _duration)
		{
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();
			while (stopwatch.ElapsedMilliseconds < (long)1000*_duration)
			{
			}
			stopwatch.Stop();
		}

		/// <summary>
		/// Generates a Pause of a second between actions
		/// </summary>
		public static void Wait1()
		{
			Wait(1);
		}

		/// <summary>
		/// Generates a Pause of 2 seconds between actions
		/// </summary>
		public static void Wait2()
		{
			Wait(2);
		}
		
		/// <summary>
		/// Generates a Pause of 5 seconds between actions
		/// </summary>
		public static void Wait5()
		{
			Wait(5);
		}

		/// <summary>
		/// Generates a Pause of 10 seconds between actions
		/// </summary>
		public static void Wait10()
		{
			Wait(10);
		}
		#endregion

		#region Item Methods

		/// <summary>
		/// Changes the Position of an Item
		/// </summary>
		/// <param name="end">target vector</param>
		/// <param name="_item">target item</param>
		public static void ChangePosition(Vector2 end, Item _item)
		{
			_item.Position = end;
		}
		/// <summary>
		/// Toggles the Visibility of an Item
		/// </summary>
		/// <param name="_item">target item</param>
		public static void ToggleVisibility(Item _item)
		{
			if (_item.Visible)
			{
				_item.Visible = false;
			}
			else
			{
				_item.Visible = true;
			}
		}
		#endregion

	}
}
