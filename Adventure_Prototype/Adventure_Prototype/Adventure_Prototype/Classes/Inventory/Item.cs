using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

using Classes.Pipeline;
using Classes.Graphics;

namespace Classes.Inventory
{
	public class Item :DrawableGameComponent
	{

		private Texture2D image;
		private String id;
		private Texture2D tooltip;

		#region Properties

		public Texture2D Image
		{
			get { return image; }
			set { this.image = value; }
		}
		public Texture2D Tooltip
		{
			get { return tooltip; }
			set { this.tooltip = value; }
		}
		public String ID
		{
			get { return this.id; }
		}

		#endregion

		public Item(Texture2D Image, String ID)
			: base(GameRef.Game)
		{
			this.image = Image;
			this.id = ID;
			this.Visible = false;
		}

	}
}
