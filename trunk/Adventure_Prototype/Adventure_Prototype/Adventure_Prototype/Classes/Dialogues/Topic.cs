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

		private String text;
		private Info info;
		private Color drawColor;




		/// <summary>
		/// Returns an Instance of Topic
		/// </summary>
		/// <param name="text">The Topics sentence</param>
		/// <param name="info">The Topics Info</param>
		public Topic(String text, Info info)
		{
			this.text = text;
			this.info = info;
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
		public Info TopicInfo
		{
			get { return this.info; }
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
