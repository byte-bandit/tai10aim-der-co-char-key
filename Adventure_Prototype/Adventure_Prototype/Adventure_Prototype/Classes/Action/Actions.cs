using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Classes;
using Classes.Pipeline;
using Classes.Inventory;
using Classes.IO;

namespace Classes.Action
{
	public class Actions
	{
		private List<String> effects;
		private object trigger;
		private object trigger2;
		private Cursor.CursorAction typ;

		public enum Action
		{
			walk,
			talk,
			look,
			use
		}
		
		#region Properties 
		public Cursor.CursorAction Typ
		{
			get { return typ; }
			set { this.typ = value; }
		}

		public Actions()
		{
		}

		public List<string> Effects
		{
			get { return effects; }
			set { this.effects = value; }
		}

		public object Trigger
		{
			get{return trigger;}
			set { this.trigger = value; }
		}

		public object Trigger2
		{
			get { return trigger2; }
			set { this.trigger2 = value; }
		}
		#endregion

	}
}
