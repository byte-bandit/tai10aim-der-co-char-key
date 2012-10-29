using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Classes.Graphics;

namespace Classes.Graphics
{
	public class WorldObject : Entity
	{

		private String textureString;
		private Texture2D texture;
		private Game game;
		private Vector2 position;
		private Color highlight = Color.White;

		private String sName;
		private String sOnLook;
		private String sOnUse;
		private String sOnTalk;

		private Color floatingLineColor = default(Color);



		/// <summary>
		/// Creates a new World Object
		/// </summary>
		/// <param name="game">The game to which the whole object is bound.</param>
		/// <param name="textureString">The string to the asset name of the desired texture.</param>
		public WorldObject(Game game, String textureString)
			: base(game)
		{
			this.game = game;
			this.textureString = textureString;
			this.Initialize();
			this.position = Vector2.Zero;
		}




		public override Vector2 GetFloatingLinePosition()
		{
			return this.Position;
		}



		public override Color GetFloatingLineColor()
		{
			return this.floatingLineColor;
		}

		public override void SetFloatingLineColor(Color color)
		{
			this.floatingLineColor = color;
		}








		/// <summary>
		/// Gets or sets the String or Function for the Look Action
		/// </summary>
		public String OnLook
		{
			get { return this.sOnLook; }
			set { this.sOnLook = value; }
		}







		/// <summary>
		/// Gets or sets the String or Function for the Use Action
		/// </summary>
		public String OnUse
		{
			get { return this.sOnUse; }
			set { this.sOnUse = value; }
		}




		/// <summary>
		/// Gets or sets the Name of the World Object
		/// </summary>
		public String Name
		{
			get { return this.sName; }
			set { this.sName = value; }
		}





		/// <summary>
		/// Gets or sets the String or Function for the Talk Action
		/// </summary>
		public String OnTalk
		{
			get { return this.sOnTalk; }
			set { this.sOnTalk = value; }
		}





		/// <summary>
		/// Gets a rectangle surrounding the boundaries of the image of the WorldObject
		/// </summary>
		/// <returns></returns>
		public Rectangle getDrawingRect()
		{
			return new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.size.X, (int)this.size.Y);
		}




		/// <summary>
		/// Gets the Texture of the WorldObject
		/// </summary>
		public String Texture
		{
			get { return this.textureString; }
		}




		/// <summary>
		/// Gets or sets the Position of the World Object
		/// </summary>
		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; }
		}




		/// <summary>
		/// Gets the size of the WorldObjects Texture, X = Width, Y = Height
		/// </summary>
		public Vector2 size
		{
			get { return new Vector2(this.texture.Width, this.texture.Height); }
		}




		/// <summary>
		/// Gets or sets the highlighted Color for the WorldObject. Default = Color.White
		/// </summary>
		public Color HighlightColor
		{
			get { return this.highlight; }
			set { this.highlight = value; }
		}




		/// <summary>
		/// Creates a new Instance of World Object.
		/// </summary>
		/// <param name="game">The game wo which to bind the Component.</param>
		/// <param name="texture">The texture of the game obejct.</param>
		public WorldObject(Game game, Texture2D texture)
			: base(game)
		{
			this.game = game;
			this.textureString = "Graphics/Sprites/" + texture.Name;
			this.texture = texture;
			this.position = Vector2.Zero;
		}



		/// <summary>
		/// Initialize the whole component
		/// </summary>
		public override void Initialize()
		{
			this.texture = game.Content.Load<Texture2D>(textureString);
			base.Initialize();
		}




		/// <summary>
		/// Draws the World Object on Screen
		/// </summary>
		/// <param name="gameTime">elapsed game time</param>
		public override void Draw(GameTime gameTime)
		{
			GraphicsManager.spriteBatch.Begin();
			GraphicsManager.spriteBatch.Draw(this.texture, this.position, this.highlight );
			GraphicsManager.spriteBatch.End();
			base.Draw(gameTime);
		}




		/// <summary>
		/// Update Logic for the world object
		/// </summary>
		/// <param name="gameTime">The elapsed GameTime</param>
		public override void Update(GameTime gameTime)
		{
			//Update?
			base.Update(gameTime);
		}
	}
}
