using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Classes.Graphics;
using Classes.Pipeline;
using Adventure_Prototype;

namespace Classes.Dialogues
{
	class DialogueManager
	{

		private static bool isBusy = false;
		private static Dialogue dialogue;
		private static MouseState prevMouseState;
		private static MouseState mouseState;
		private const int LINEBREAK = 30;
		private static State state = State.topics;
		private static Topic currentTopic;
		private static String currentInfoLine;
		private static Dialogue currentChoice;
		private static List<Dialogue> DialogueLibrary;

		private static String playerFeedback;
		private static int feedbackTimer;


		private enum State
		{
			topics,
			info,
			choice
		}


		/// <summary>
		/// Starts a new Dialogue with the given specs.
		/// </summary>
		/// <returns></returns>
		public static bool startDialogue(String identifier)
		{
			if (!isBusy)
			{
				isBusy = true;
				state = State.topics;
				dialogue = getDialogueByID(identifier);

				if (dialogue == null)
				{
					isBusy = false;
					return false;
				}

				return true;
			}
			else
			{
				return false;
			}
		}





		public static void PlayerSay(String text)
		{
			if (!isBusy)
			{
				isBusy = true;
				playerFeedback = text;
				feedbackTimer = 150;
			}
		}


		/// <summary>
		/// Checks wether the Dialogue Manager is currently working with a Dialogue.
		/// </summary>
		public static bool busy
		{
			get { return isBusy; }
		}



		public static void draw(SpriteFont font, GameTime gametime)
		{


			if (isBusy && feedbackTimer > 1)
			{
				feedbackTimer--;
				Graphics.GraphicsManager.drawText(playerFeedback, Vector2.Add(GameRef.Player1.Position, new Vector2(10, -30)), Graphics.GraphicsManager.font02, Color.SkyBlue, true);
			}
			else if(feedbackTimer == 1)
			{
				feedbackTimer = 0;
				isBusy = false;
			}


			if (!isBusy || dialogue == null)
			{
				return;
			}

			//*le fancy Background
			RectangleOverlay fancyBG = new RectangleOverlay(new Rectangle(0, 0, (int)GameRef.Resolution.X, (dialogue.Topics.Count + 2) * LINEBREAK), new Color(0,0,0,128), GameRef.Game);
			fancyBG.Draw(gametime);

			switch (state)
			{
				case State.topics:
					for (int n = 0; n < dialogue.Topics.Count; n++)
					{
						Graphics.GraphicsManager.drawText(dialogue.Topics[n].getText(), new Vector2(10, (n + 1) * LINEBREAK), font, dialogue.Topics[n].color);
					}
					break;

				case State.info:
					Graphics.GraphicsManager.drawText(currentInfoLine, new Vector2(10, 30),font, Color.LightBlue);
					break;

				case State.choice:
					for (int n = 0; n < currentChoice.Topics.Count; n++)
					{
						Graphics.GraphicsManager.drawText(currentChoice.Topics[n].getText(), new Vector2(10, (n + 1) * LINEBREAK), font, currentChoice.Topics[n].color);
					}
					break;

				default:

					break;

			}


		}



		public static void Update()
		{

			if (dialogue == null)
			{
				return;
			}

			mouseState = Mouse.GetState();


			switch (state)
			{
				case State.topics:
					for (int n = 0; n < dialogue.Topics.Count; n++)
					{
						if (mouseState.Y > (n + 1) * LINEBREAK && mouseState.Y < (n + 2) * LINEBREAK)
						{
							dialogue.Topics[n].color = Color.Red;
						}
						else
						{
							dialogue.Topics[n].color = Color.White;
						}
					}

					if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
					{
						//Click Detected
						for (int n = 0; n < dialogue.Topics.Count; n++)
						{
							if (mouseState.Y > (n + 1) * LINEBREAK && mouseState.Y < (n + 2) * LINEBREAK)
							{
								topicClick(dialogue.Topics[n]);
							}
						}
					}
					break;

				case State.info:
					if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
					{
						String n = currentTopic.TopicInfo.getNextLine();
						if (n == "" || n == null)
						{
							if (currentTopic.TopicInfo.isChoiceTrigger)
							{
								state = State.choice;
								currentChoice = currentTopic.TopicInfo.getChoice();
							}
							else
							{
								topicDone();
							}
						}
						else
						{
							currentInfoLine = n;
						}
					}
					break;

				case State.choice:
					for (int n = 0; n < currentChoice.Topics.Count; n++)
					{
						if (mouseState.Y > (n + 1) * LINEBREAK && mouseState.Y < (n + 2) * LINEBREAK)
						{
							currentChoice.Topics[n].color = Color.Red;
						}
						else
						{
							currentChoice.Topics[n].color = Color.White;
						}
					}

					if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
					{
						//Click Detected
						for (int n = 0; n < currentChoice.Topics.Count; n++)
						{
							if (mouseState.Y > (n + 1) * LINEBREAK && mouseState.Y < (n + 2) * LINEBREAK)
							{
								topicClick(currentChoice.Topics[n]);
							}
						}
					}
					break;

				default:

					break;

			}



			prevMouseState = mouseState;
		}


		private static void topicDone()
		{
			if (currentTopic.TopicInfo.isGoodbye && currentChoice == null)
			{
				dialogue = null;
				isBusy = false;
			}
			if (currentChoice != null)
			{
				currentChoice = null;
			}
			currentTopic = null;
			state = State.topics;
			currentInfoLine = null;
		}

		private static void topicClick(Topic t)
		{
			currentTopic = t;
			state = State.info;
			currentInfoLine = t.TopicInfo.getNextLine();
		}





		public static void Initialize()
		{
			String[] txt;
			Dialogue cDia = null;
			Topic cTop = null;
			Info cInf = null;
			DialogueLibrary = new List<Dialogue>();

			try
			{
				txt = System.IO.File.ReadAllLines("Content/Dialogues.txt");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.ToString());
				return;
			}

			foreach (String line in txt)
			{
				//Check for Comments
				if (line.Trim().StartsWith("#"))
				{
					continue;
				}


				if (line.Trim().StartsWith("@DIALOGUE"))
				{
					DialogueLibrary.Add(new Dialogue(line.Substring(9).Trim()));
					cDia = DialogueLibrary[DialogueLibrary.Count - 1];
					continue;
				}


				if (line.Trim().StartsWith("ASK"))
				{
					if (cDia == null)
					{
						continue;
					}
					cInf = new Info();
					cTop = new Topic(line.Substring(line.IndexOf("\"") + 1, line.LastIndexOf("\"") - line.IndexOf("\"") - 1), cInf);
					cDia.addTopic(cTop);
					continue;
				}





				if (line.Trim().StartsWith("SAY"))
				{
					if (cDia == null || cTop == null)
					{
						continue;
					}
					cTop.TopicInfo.addLine(line.Substring(line.IndexOf("\"") + 1, line.LastIndexOf("\"") - line.IndexOf("\"") - 1));
					continue;
				}




				if (line.Trim().StartsWith("GOODBYE"))
				{
					if (cDia == null || cTop == null)
					{
						continue;
					}
					cTop.TopicInfo.isGoodbye = true;
					cTop.TopicInfo.addLine(line.Substring(line.IndexOf("\"") + 1, line.LastIndexOf("\"") - line.IndexOf("\"") - 1));
					continue;
				}



				if (line.Trim().StartsWith("GOTO"))
				{
					if (cDia == null || cTop == null)
					{
						continue;
					}
					cTop.TopicInfo.setChoice(line.Substring(line.IndexOf("\"") + 1, line.LastIndexOf("\"") - line.IndexOf("\"") - 1));
					continue;
				}
			}
		}




		/// <summary>
		/// Get a Dialogue by it's ID
		/// </summary>
		/// <param name="id">The unique identifier of the Dialogue</param>
		/// <returns></returns>
		public static Dialogue getDialogueByID(String id)
		{
			foreach (Dialogue d in DialogueLibrary)
			{
				if (d.ID == id)
				{
					return d;
				}
			}

			return null;
		}




	}
}
