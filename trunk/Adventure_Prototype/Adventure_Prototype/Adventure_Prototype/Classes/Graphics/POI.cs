using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Classes.Graphics;

namespace Classes.Graphics
{
	public class POI : Classes.Pathfinding.Polygon 
	{


		private String sName;
		private String sOnLook;
		private String sOnUse;
		private string sOnTalk;



		public POI(bool change, Game game)
			: base(change, game)
		{

		}




		public string Name
		{
			get { return this.sName; }
			set { this.sName = value; }
		}



		public string onLook
		{
			get { return this.sOnLook; }
			set { this.sOnLook = value; }
		}



		public string onUse
		{
			get { return this.sOnUse; }
			set { this.sOnUse = value; }
		}



		public string onTalk
		{
			get { return this.sOnTalk; }
			set { this.sOnTalk = value; }
		}
	}
}
