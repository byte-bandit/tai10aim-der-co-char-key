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

using Classes;
using Classes.Pipeline;
using Classes.Graphics;

namespace Classes.Pathfinding
{
	public class Path : DrawableGameComponent
	{
		public LinkedList<Vector2> Way = new LinkedList<Vector2>(); //LinkedList 
		private Vector2 start;
		private Vector2 end;

		public Path(Vector2 Point1, Vector2 Point2)
			: base(GameRef.Game) // Constructor for Path
		{
			this.start = Point1;
			this.end = Point2;
		}

		public Vector2 Start //start point property
		{
			get { return start; }
			set { start = value; }
		}
		public Vector2 End //end point property
		{
			get { return end; }
			set { end = value; }
		}

		public LinkedList<Vector2> findPath()
		{
			LinkedList<Vector2> outcome = new LinkedList<Vector2>();
			LinkedList<LinkedList<Vector2>> lines = new LinkedList<LinkedList<Vector2>>();
			Vector2 tmp = SceneryManager.CurrentRoom.WalkAreas.Nodes[0];

			outcome.AddFirst(start);
			lines = Path.findAllSegmentCrosses(start, end);

			if (lines.Count == 0)
			{
				outcome = new LinkedList<Vector2>();
				outcome.AddFirst(end);
			}
			else
			{
				if ((lines.Count % 2) == 0)
				{
					//lines = findAllSegmentCrosses(end, outcome.Last.Previous.Value);
					lines = findAllSegmentCrosses(end, outcome.First.Value);
					while (lines.Count != 0)
					{
						foreach (LinkedList<Vector2> lv in lines)
						{
							if (outcome.Count == 1)
							{
								if (findAllSegmentCrosses(lv.First.Value, outcome.First.Value).Count == 0)
								{
									if (Path.astellarcosts(lv.First.Value, outcome.First.Value, end) < Path.astellarcosts(lv.First.Next.Value, outcome.First.Value, end))
									{
										outcome.Last.Value = lv.First.Value;
										outcome.AddLast(end);
									}
									else
									{
										outcome.Last.Value = lv.First.Next.Value;
										outcome.AddLast(end);
									}
									break;
								}
							}
							else
							{
								if (findAllSegmentCrosses(lv.First.Value, outcome.Last.Previous.Value).Count == 0)
								{
									if (Path.astellarcosts(lv.First.Value, outcome.Last.Previous.Value, end) < Path.astellarcosts(lv.First.Next.Value, outcome.Last.Previous.Value, end))
									{
										outcome.Last.Value = lv.First.Value;
										outcome.AddLast(end);
									}
									else
									{
										outcome.Last.Value = lv.First.Next.Value;
										outcome.AddLast(end);
									}
									break;
								}
							}
							
						}
						if (outcome.Count == 1)
						{
							lines = findAllSegmentCrosses(end, outcome.First.Value);
						}
						else
						{
							lines = findAllSegmentCrosses(end, outcome.Last.Previous.Value);
						}
					}
				}
			}
			if (outcome.First.Value == start)
			{
				outcome.RemoveFirst();
			}

			Way.Clear();
			Way = outcome;
			return outcome;

		}

		private static int astellarcosts(Vector2 point1, Vector2 actual, Vector2 End)
		{
			return ((int)Vector2.Distance(point1, actual) + (int)Vector2.Distance(point1, End));
		}





		public static bool LineSegmentsCross(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
		{
			float denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));

			if (denominator == 0)
			{
				return false;
			}

			float numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
			float numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

			if (numerator1 == 0 || numerator2 == 0)
			{
				return false;
			}

			float r = numerator1 / denominator;
			float s = numerator2 / denominator;

			return  ((r > 0 && r < 1) && (s > 0 && s < 1));
		}



		public static LinkedList<LinkedList<Vector2>> findAllSegmentCrosses(Vector2 Start, Vector2 End)
		{
			LinkedList<Vector2> line = new LinkedList<Vector2>();
			LinkedList<LinkedList<Vector2>> lines = new LinkedList<LinkedList<Vector2>>();
			for (int i = 1; i < SceneryManager.CurrentRoom.WalkAreas.Nodes.Count; i++)
			{
				if (LineSegmentsCross(Start, End, SceneryManager.CurrentRoom.WalkAreas.Nodes[i - 1], SceneryManager.CurrentRoom.WalkAreas.Nodes[i]))
				{
					line.Clear();
					line.AddFirst(SceneryManager.CurrentRoom.WalkAreas.Nodes[i - 1]);
					line.AddLast(SceneryManager.CurrentRoom.WalkAreas.Nodes[i]);
					lines.AddLast(line);
				}
			}

			return lines;
		}

		private void DrawLine(Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
		{
			float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			float length = Vector2.Distance(point1, point2);

			GraphicsManager.spriteBatch.Draw(blank, point1, null, color,
					   angle, Vector2.Zero, new Vector2(length, width),
					   SpriteEffects.None, 0);
		}
	}
}
