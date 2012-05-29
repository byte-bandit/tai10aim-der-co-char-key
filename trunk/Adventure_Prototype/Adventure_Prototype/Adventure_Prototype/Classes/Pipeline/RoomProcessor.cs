using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Xna.Framework;

using Classes;
using Classes.Graphics;
using Classes.Pathfinding;

namespace Classes.Pipeline
{
	/// <summary>
	/// Builds up a Room from a source file
	/// </summary>
	class RoomProcessor
	{

		private static Game _game;




		/// <summary>
		/// Sets up the Class to be ready for use
		/// </summary>
		/// <param name="game">Link to our main game Instance</param>
		public static void Initialize(Game game)
		{
			_game = game;
		}





		/// <summary>
		/// Reads in the given source file and returns a room if possible
		/// </summary>
		/// <param name="path">The path to the source file (*.bmap)</param>
		/// <returns></returns>
		public static Room createRoomFromFile(String path)
		{
			String[] data = null;

			try
			{
				path.Replace("/", @"\");
				path = @"Content\" + path;
				data = File.ReadAllLines(path);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
				return null;
			}



			String ID = String.Empty;
			String Texture_Path = String.Empty;
			bool isScaling = false;
			Polygon walkAreas = new Polygon(false, GameRef.Game);
			Room ret = null;



			for (int n = 0; n < data.Length; n++)
			{
				String line = data[n];



				if (line.Trim().StartsWith("#"))
				{
					//Comentary Line - ignore and get next line
					continue;
				}



				if (line.Trim().StartsWith("BEGINROOM"))
				{
					//Get ID and Texture-String
					n++;
					ID = data[n].Substring(data[n].IndexOf("ID:") + 3, data[n].IndexOf(",TEXTURE:") - 3);
					Texture_Path = data[n].Substring(data[n].IndexOf("TEXTURE:") + 8);

					//Get Scaling Values
					n++;
					if (data[n].Substring(data[n].IndexOf("SCALING:") + 8).ToLower() == "true")
					{
						isScaling = true;
					}

					ret = new Room(_game, ID, Texture_Path);

					//Get next line
					continue;
				}




				if (line.Trim().StartsWith("BEGINWALK"))
				{
					//Get X, Y
					n++;

					int X = int.Parse(data[n].Substring(data[n].IndexOf("X:") + 2, data[n].IndexOf(",Y:") - 2));
					int Y = int.Parse(data[n].Substring(data[n].IndexOf("Y:") + 2)) ;

					walkAreas.Nodes.Add(new Vector2(X, Y));

					//Get next line
					continue;
				}






				if (line.Trim().StartsWith("BEGINPOI"))
				{
					//Get X, Y, Width and Height
					n++;
					POI p = new POI(true, GameRef.Game);

					while (data[n].Trim().StartsWith("X:"))
					{

						int X = int.Parse(data[n].Substring(data[n].IndexOf("X:") + 2, data[n].IndexOf(",Y:") - 2));
						int Y = int.Parse(data[n].Substring(data[n].IndexOf("Y:") + 2));
						p.Nodes.Add(new Vector2(X,Y));
						n++;

					}

					//Get Name
					p.Name = data[n].Substring(data[n].IndexOf("NAME:") + 5);

					//Get Look
					n++;
					p.onLook = data[n].Substring(data[n].IndexOf("ONLOOK:") + 7);

					//Get Use
					n++;
					p.onUse = data[n].Substring(data[n].IndexOf("ONUSE:") + 6);

					//Get Talk
					n++;
					p.onTalk = data[n].Substring(data[n].IndexOf("ONTALK:") + 7);

					ret.POIS.Add(p);

					//Get next line
					continue;
				}




				if (line.Trim().StartsWith("BEGINWO"))
				{
					//Get X, Y, Width and Height
					n++;

					int X = int.Parse(data[n].Substring(data[n].IndexOf("X:") + 2, data[n].IndexOf(",Y:") - 2));
					int Y = int.Parse(data[n].Substring(data[n].IndexOf("Y:") + 2));
					//int W = int.Parse(data[n].Substring(data[n].IndexOf("WIDTH:") + 6, data[n].IndexOf(",HEIGHT:") - data[n].IndexOf("WIDTH:") - 6));
					//int H = int.Parse(data[n].Substring(data[n].IndexOf("HEIGHT:") + 7));

					//Get Texture
					n++;
					String WO_Texture_Path = data[n].Substring(data[n].IndexOf("TEXTURE:") + 8);

					//Get Name
					n++;
					String name = data[n].Substring(data[n].IndexOf("NAME:") + 5);

					//Get Look
					n++;
					String look = data[n].Substring(data[n].IndexOf("ONLOOK:") + 7);

					//Get Use
					n++;
					String use = data[n].Substring(data[n].IndexOf("ONUSE:") + 6);

					//Get Talk
					n++;
					String talk = data[n].Substring(data[n].IndexOf("ONTALK:") + 7);

					WorldObject tmp = new WorldObject(_game, WO_Texture_Path);
					tmp.Position = new Vector2(X, Y);
					tmp.Name = name;
					tmp.OnLook = look;
					tmp.OnUse = use;
					tmp.OnTalk = talk;

					ret.addObject(tmp);

					//Get next line
					continue;
				}




				if (line.Trim().StartsWith("BEGINNPC"))
				{
					//Get X, Y
					n++;

					int X = int.Parse(data[n].Substring(data[n].IndexOf("X:") + 2, data[n].IndexOf(",Y:") - 2));
					int Y = int.Parse(data[n].Substring(data[n].IndexOf("Y:") + 2));

					//Get Texture
					n++;
					String WO_Texture_Path = data[n].Substring(data[n].IndexOf("TEXTURE:") + 8);

					//Get mirrored
					n++;
					bool mirror = Convert.ToBoolean(data[n].Substring(data[n].IndexOf("MIRROR:") + 7));

					//Get Name
					n++;
					String name = data[n].Substring(data[n].IndexOf("NAME:") + 5);

					//Get Look
					n++;
					String look = data[n].Substring(data[n].IndexOf("ONLOOK:") + 7);

					//Get Use
					n++;
					String use = data[n].Substring(data[n].IndexOf("ONUSE:") + 6);

					//Get Talk
					n++;
					String talk = data[n].Substring(data[n].IndexOf("ONTALK:") + 7);

					NPC tmp = new NPC(_game, ret, "c_" + name.ToLower(), name);

					tmp.setGFX("Graphics/Charsets/" + WO_Texture_Path);
					tmp.GFXInfo = new Animation(tmp.GFX.Width, tmp.GFX.Height, 6, 3, 0, 0, false);
					tmp.IsMirrored = mirror;

					tmp.Position = new Vector2(X, Y);
					tmp.Name = name;
					tmp.OnLook = look;
					tmp.OnUse = use;
					tmp.OnTalk = talk;


					ret.addNPC(tmp);

					//Get next line
					continue;
				}




			}




			ret.WalkAreas = walkAreas;

			return ret;
		}
	}
}
