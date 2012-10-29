using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Classes.Pipeline;

namespace Classes
{
	public abstract class Character : DrawableGameComponent 
	{
		protected Vector2 position;
		protected Vector2 target;
		protected Vector2 velocity;
		protected float scale;
		protected Animation gfxInfo;
		protected Texture2D gfx;
		protected Game game;
		protected String id;
		protected String name;
		protected Room room;

		private String pathToTexture;
		private LinkedList<Vector2> walkingRoute = new LinkedList<Vector2>();


		private String sOnLook;
		private String sOnUse;
		private string sOnTalk;

		private Color floatingLineColor = default(Color);

		private Color highlight = Color.White;

		private bool isMirrored;

		public Character(Game game, Room room, String id, String name, Animation animation = null, Texture2D sprite = null, float scale = 1.0f)
			: base(game)
		{
			this.game = game;
			this.id = id;
			this.name = name;
			this.isMirrored = false;
			this.room = room;
			this.scale = scale;
			

			if (animation != null)
			{
				this.gfxInfo = animation;
			}

			if (sprite != null)
			{
				this.gfx = sprite;
			}
		}








		public Character(Game game, Room room, String id, String name, Animation animation = null, String sprite = null, float scale = 1.0f)
			: base(game)
		{
			this.game = game;
			this.id = id;
			this.name = name;
			this.isMirrored = false;
			this.room = room;
			this.scale = scale;


			if (animation != null)
			{
				this.gfxInfo = animation;
			}

			if (sprite != null)
			{
				this.pathToTexture = sprite;
				this.Initialize();
			}
		}




		public override void Initialize()
		{

			this.gfx = this.game.Content.Load<Texture2D>(this.pathToTexture);
			
			base.Initialize();
		}




		public override void Draw(GameTime gameTime)
		{
			SpriteEffects se = new SpriteEffects();

			if (this.isMirrored)
				se = SpriteEffects.FlipHorizontally;
			else
				se = SpriteEffects.None;

			Graphics.GraphicsManager.spriteBatch.Begin();
			Graphics.GraphicsManager.spriteBatch.Draw(this.gfx, new Vector2(this.position.X, this.position.Y - ((this.scale - 1)*this.gfxInfo.CurrentFrame.Height  )), this.gfxInfo.CurrentFrame, highlight, 0.0f, Vector2.Zero, this.scale, se, 1.0f);
			Graphics.GraphicsManager.spriteBatch.End();
			base.Draw(gameTime);
		}




		/// <summary>
		/// Gets a Rectangle forming the Characters border
		/// </summary>
		/// <returns></returns>
		public Rectangle getDrawingRect()
		{
			return new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.gfx.Width/(int)GameRef.AnimationFrames.X, (int)this.gfx.Height/(int)GameRef.AnimationFrames.Y);
		}







		public Color FloatingLineColor
		{
			get { return this.floatingLineColor; }
			set { this.floatingLineColor = value; }
		}





		/// <summary>
		/// Gets or sets whether the Character is drawn mirrored or not.
		/// </summary>
		public bool IsMirrored
		{
			get { return this.isMirrored; }
			set { this.isMirrored = value; }
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
			get { return this.name; }
			set { this.name = value; }
		}





		/// <summary>
		/// Gets or sets the String or Function for the Talk Action
		/// </summary>
		public String OnTalk
		{
			get { return this.sOnTalk; }
			set { this.sOnTalk = value; }
		}





		public Texture2D GFX
		{
			get { return this.gfx; }
			set { this.gfx = value; }
		}





		public Animation GFXInfo 
		{
			get { return this.gfxInfo; }
			set { this.gfxInfo = value;}
		}

		public Vector2 Position
		{
			get { return this.position; }
			set { this.position = value; this.target = this.getPivotPoint(); }
		}







		public void setWalkingTarget(Vector2 WalkingTarget)
		{
			//foreach (Rectangle r in myPolygon)
			//{
			//    if (WalkingTarget.X > r.X && WalkingTarget.X < r.X + r.Width && WalkingTarget.Y > r.Y && WalkingTarget.Y < r.Y + r.Height)
			//    {
			//        //Walking Target inside Rectangle r
			//        this.target = WalkingTarget;
			//        this.gfxInfo.AnimationState = Animation.AnimationCycle.Walk;
			//        return; //No need to check further
			//    }
			//}

			Pathfinding.Path route = new Pathfinding.Path(this.getPivotPoint(), WalkingTarget, SceneryManager.CurrentRoom.WalkAreas);
			this.walkingRoute = route.findPath();

			if (this.walkingRoute.Count > 0)
			{
				this.target = walkingRoute.First.Value;
				this.walkingRoute.RemoveFirst();
				this.gfxInfo.AnimationState = Animation.AnimationCycle.Walk;
			}
		}





		private Vector2 getPivotPoint()
		{
			return new Vector2(this.position.X + ((this.gfxInfo.CurrentFrame.Width) / 2), this.position.Y + this.gfxInfo.CurrentFrame.Height );
		}

		public override void Update(GameTime gameTime)
		{

			//scale the character
			if (this.room.ScaleCharacters == true)
			{
				Vector4 scaleParams = this.room.getScalingParams();

				if (scaleParams != Vector4.Zero)
				{
					int minLvl = (int)scaleParams.X;
					float minVal = scaleParams.Y;
					int maxLvl = (int)scaleParams.Z;
					float maxVal = scaleParams.W;

					if (this.getPivotPoint().Y >= minLvl && this.getPivotPoint().Y <= maxLvl)
					{
						float dRatio = this.getPivotPoint().Y - minLvl;
						dRatio = dRatio / (maxLvl - minLvl);

						this.scale = minVal + (maxVal * dRatio);
					}
				}
			}

			//mirror the Character

			if (this.velocity.X > 0.1f)
				this.isMirrored = false;
			if (this.velocity.X < -0.1f)
				this.isMirrored = true;

			//get elapsed seconds
			float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

			this.gfxInfo.Update(elapsed * velocity.Length());


			if (Vector2.Distance(this.getPivotPoint(), this.target) > 3.0f)
			{
				if (this.gfxInfo.AnimationState != Animation.AnimationCycle.Walk)
				{
					this.gfxInfo.AnimationState = Animation.AnimationCycle.Walk;
				}
				this.velocity = Vector2.Subtract(this.target, this.getPivotPoint());
				this.velocity.Normalize();
				this.velocity = Vector2.Multiply(this.velocity, 3.0f);
				this.position = Vector2.Add(this.position, this.velocity);
			}
			else
			{
				//Get next target in route
				if (this.walkingRoute.Count == 0)
				{
					this.gfxInfo.AnimationState = Animation.AnimationCycle.Idle;
				}
				else
				{
					this.target = this.walkingRoute.First.Value;
					this.walkingRoute.RemoveFirst();
				}
			}

			base.Update(gameTime);
		}
	}
}
