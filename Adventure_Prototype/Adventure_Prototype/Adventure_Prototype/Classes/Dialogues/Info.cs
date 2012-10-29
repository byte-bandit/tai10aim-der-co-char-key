using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Dialogues
{
	class Info
	{

		//private List<String> lines;			//Holding all the lines for the program
		//private int iterator;				//Used to iterate through the lines
		private bool goodbye;				//Wether to end the conversation after this dialog option
		private String text;
		//private String choice_t;

		/// <summary>
		/// Info is used as a return value for all topics featuring the answer text of the NPCs
		/// </summary>
		public Info(String text)
		{
			this.text = text;
			this.goodbye = false;
		}



		public String Text
		{
			get { return this.text; }
		}


		public Info(String text, Boolean isGoodbye)
		{
			this.text = text;
			this.goodbye = isGoodbye;
		}


		/// <summary>
		/// Add a new Line for the Info. Each Line will be printed seperately.
		/// </summary>
		/// <param name="line">The new Line you want the Info to say.</param>
		//public void addLine(String line)
		//{
		//    this.lines.Add(line);
		//}


		/// <summary>
		/// Sets a Choice to be displayed after the Info.
		/// </summary>
		/// <param name="choice"></param>
		//public void setChoice(String choice)
		//{
		//    this.choice_t = choice;
		//}



		/// <summary>
		/// Gets the Choice to be displayed after the Info.
		/// </summary>
		/// <param name="choice"></param>
		//public Dialogue getChoice()
		//{
		//    return DialogueManager.getDialogueByID(this.choice_t);
		//}


		/// <summary>
		/// Returns line - by - line, starting at line 0. Returns empty String when done.
		/// </summary>
		/// <returns></returns>
		//public String getNextLine()
		//{
		//    if (this.iterator < this.lines.Count)
		//    {
		//        this.iterator++;
		//        return this.lines[this.iterator - 1];
		//    }
		//    else
		//    {
		//        this.iterator = 0;
		//        return "";
		//    }
		//}



		/// <summary>
		/// Returns all Lines of the Info.
		/// </summary>
		/// <returns></returns>
		//public List<String> getAllLines()
		//{
		//    return this.lines;
		//}



		/// <summary>
		/// Returns the Line at the 0-based given Index or an empty String.
		/// </summary>
		/// <param name="index">The index to look for the line.</param>
		/// <returns></returns>
		//public String getLineAt(int index)
		//{
		//    if (index < this.lines.Count)
		//    {
		//        return this.lines[index];
		//    }
		//    else
		//    {
		//        return "";
		//    }
		//}


		/// <summary>
		/// Gets or sets wether to end the dialog after the last Line.
		/// </summary>
		public bool isGoodbye
		{
			get { return this.goodbye; }
			set { this.goodbye = value; }
		}


		/// <summary>
		/// Returns true wether the Info triggers a Choice or not.
		/// </summary>
		//public bool isChoiceTrigger
		//{
		//    get { if (this.choice_t != null) { return true; } else { return false; } }
		//}
	}
}
