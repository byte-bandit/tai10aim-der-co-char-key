using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Dialogues
{
	class Dialogue
	{
		private int participants;			//TODO!!! Change this to List<NPC> once class NPC has been written in final Project !!!!
		private List<Topic> topics;
		private String id;


		/// <summary>
		/// Creates a new Dialogue.
		/// </summary>
		/// <param name="id">A unique ID from what to call the Dialogue.</param>
		public Dialogue(String id)
		{
			this.topics = new List<Topic>();
			this.id = id;
		}




		public String ID
		{
			get { return this.id; }
		}


		/// <summary>
		/// Add a Topic to the Dialogue
		/// </summary>
		/// <param name="topic">The Topic to be added to the Dialogue.</param>
		/// <returns></returns>
		public bool addTopic(Topic topic)
		{
			this.topics.Add(topic);
			return true;
		}


		/// <summary>
		/// Add a Topic to the Dialogue
		/// </summary>
		/// <param name="text">The text of the new Topic</param>
		/// <param name="info">The Info of the Topic</param>
		/// <returns></returns>
		public bool addTopic(String text, Info info)
		{
			this.topics.Add(new Topic(text, info));
			return true;
		}



		/// <summary>
		/// Get the Topics of the Dialogue
		/// </summary>
		public List<Topic> Topics
		{
			get { return this.topics; }
		}



		/// <summary>
		/// Add a Topic to the Dialogue
		/// </summary>
		/// <param name="text">The text of the new Topic</param>
		/// <param name="lines">A List of Strings you want the NPC to answer.</param>
		/// <param name="goodbye">Wether to end the Dialogue after the last line has been said.</param>
		/// <returns></returns>
		public bool addTopic(String text, List<String> lines, bool goodbye)
		{
			Info tmpInfo = new Info();

			foreach (String n in lines)
			{
				tmpInfo.addLine(n);
			}

			tmpInfo.isGoodbye = goodbye;

			this.topics.Add(new Topic(text, tmpInfo));
			return true;
		}
	}
}
