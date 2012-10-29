using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Classes.Dialogues
{
	class FloatingLine
	{

		private String n_text;
		private int countdown;
		private Vector2 pos;
		private Color drawColor;

		private const int TEXT_TIME_MULTIPLIER = 20;







		public FloatingLine(String text, float X, float Y, Color color = default(Color))
		{
			this.n_text = text;
			this.pos = new Vector2(X, Y);
			this.countdown = text.Length * TEXT_TIME_MULTIPLIER;
			if (color == default(Color)) { color = Color.LightBlue; }
			this.drawColor = color;
		}


		public FloatingLine(String text, Vector2 position, Color color = default(Color))
		{
			this.n_text = text;
			this.pos = position;
			this.countdown = text.Length * TEXT_TIME_MULTIPLIER;
			if (color == default(Color)) { color = Color.LightBlue; }
			this.drawColor = color;
		}





		public Vector2 Position
		{
			get { return this.pos; }
		}




		public int RemainingTime
		{
			get { return this.countdown; }
		}



		public void Kill()
		{
			this.countdown = 0;
		}






		public void Update()
		{
			this.countdown--;
		}




		public void Draw()
		{
			Graphics.GraphicsManager.drawText(n_text, Vector2.Add(pos, new Vector2(10,-30)), Graphics.GraphicsManager.font02, this.drawColor, true);
		}

	}
}
