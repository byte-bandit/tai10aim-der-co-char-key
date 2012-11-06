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
	public class Item :Entity
	{

		private Texture2D image;
		private String id;
		private String tooltip;
        private String use;
        private String talk;
        private String look;
        private int x;
        private int y;

        private Color floatingLineColor = default(Color);

		#region Properties

        public int X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public int Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

		public Texture2D Image
		{
			get { return image; }
			set { this.image = value; }
		}
		public String Tooltip
		{
			get { return tooltip; }
			set { this.tooltip = value; }
		}
        public String Use
        {
            get { return use; }
            set { this.use = value; }
        }
        public String Talk
        {
            get { return talk; }
            set { this.talk = value; }
        }
        public String Look
        {
            get { return look; }
            set { this.look = value; }
        }
		public String ID
		{
			get { return this.id; }
		}

		#endregion

        public Rectangle getDrawingRectangle()
        {
            return new Rectangle(this.x, this.y, this.image.Width, this.image.Height);
        }


        public override Vector2 GetFloatingLinePosition()
        {
            return new Vector2(this.x,this.y);
        }



        public override Color GetFloatingLineColor()
        {
            return this.floatingLineColor;
        }

        public override void SetFloatingLineColor(Color color)
        {
            this.floatingLineColor = color;
        }

        public Item(Texture2D Image, String ID, String Tooltip, String Use, String Look, String Talk)
			: base(GameRef.Game)
		{
			this.image = Image;
			this.id = ID;
			this.Visible = false;
            this.use = Use;
            this.talk = Talk;
            this.tooltip = Tooltip;
            this.look = Look;
		}

	}
}
