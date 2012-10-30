using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Classes.Dialogues
{
	class Topic
	{

		private String id;
		private String text;
		private List<Info> info = new List<Info>();
		private List<Topic> choice = new List<Topic>();
		private int iterator = 0;
		private Color drawColor;





		public String getNextInfoLine()
		{
			if (this.iterator == this.info.Count)
			{
				this.iterator = 0;
				return null;
			}
			else
			{
				iterator++;
				return this.info[iterator - 1].Text;
			}
		}




		public Boolean isGoodbye()
		{
			foreach (Info i in this.info)
			{
				if (i.isGoodbye)
				{
					return true;
				}
			}
			return false;
		}



		public Topic()
		{

		}



		/// <summary>
		/// Returns an Instance of Topic
		/// </summary>
		/// <param name="text">The Topics sentence</param>
		/// <param name="info">The Topics Info</param>
		public Topic(String id)
		{
			this.id = id;
		}



		public String ID
		{
			get { return this.id; }
			set { this.id = value; }
		}



		public Topic(String id, String text, List<Info> info = null, List<Topic> choice = null)
		{
			this.id = id;
			this.text = text;

			if (choice != null)
			{
				this.choice = choice;
			}

			if (info != null)
			{
				this.info = info;
			}
		}


		/// <summary>
		/// Gets or sets the current color the text of the topic will be displayed in
		/// </summary>
		public Color color
		{
			get { return this.drawColor; }
			set { this.drawColor = value; }
		}



		/// <summary>
		/// Get the Info of the Topic
		/// </summary>
		public List<Info> Info
		{
			get { return this.info; }
			set { this.info = value; }
		}




		public List<Topic> Choice
		{
			get { return this.choice; }
			set { this.choice = value; }
		}




		/// <summary>
		/// Returns the Topic's Text
		/// </summary>
		/// <returns></returns>
		public String getText()
		{
			return this.text;
		}



	}
}
