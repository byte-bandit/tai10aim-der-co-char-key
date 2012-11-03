using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Classes.Events
{
	class RoomTransporter
	{
		private static bool active = false;
		private static Room target;
		private static Vector2 p1_source;
		private static Vector2 p2_source;
		private static Vector2 p1_target;
		private static Vector2 p2_target;

		#region Properties
		public static Room Target
		{
			get { return target; }
		}

		public static Vector2 P1SourceDestination
		{
			get { return p1_source; }
		}

		public static Vector2 P2SourceDestination
		{
			get { return p2_source; }
		}

		public static Vector2 P1TargetDestination
		{
			get { return p1_target; }
		}

		public static Vector2 P2TargetDestination
		{
			get { return p2_target; }
		} 
		#endregion



		/// <summary>
		/// Transports Player to another Room using the given choordinates
		/// </summary>
		/// <param name="room_id"></param>
		/// <param name="p1SourceDest"></param>
		/// <param name="p2SourceDest"></param>
		/// <param name="p1TargetDest"></param>
		/// <param name="p2TargetDesc"></param>
		public static void Transport(String room_id,Vector2 p1TargetDest, Vector2 p2TargetDesc)
		{
			target = Pipeline.RoomProcessor.createRoomFromFile("Data/Rooms/" + room_id + ".bmap");
			//p1_source = p1SourceDest;
			p1_target = p1TargetDest;
			//p2_source = p2SourceDest;
			p2_target = p2TargetDesc;
			active = true;
		}



		/// <summary>
		/// Please call me frequently
		/// </summary>
		public static void Update()
		{
			if (!active)
			{
				return;
			}

			System.Diagnostics.Debug.Print(Vector2.Distance(SceneryManager.Player1.Position, p1_source).ToString());

			//Players in place?
			if (SceneryManager.Player1.GFXInfo.AnimationState == Animation.AnimationCycle.Idle && SceneryManager.Player2.GFXInfo.AnimationState == Animation.AnimationCycle.Idle)
			{
				SceneryManager.ChangeRoom(target);
				SceneryManager.Player1.Position = p1_target;
				SceneryManager.Player2.Position = p2_target;

				Net.NetworkManager.Profile.ControlsActive = true;
				active = false;
			}
		}
	}
}
