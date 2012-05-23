using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Classes
{
	class Animation
	{
		Rectangle[,] frames;
		float frameLength = 1f / 3f;
		float timer = 0f;
		int currentFrame = 0;
		int currentLine = 0;
		int maxFramesX = 0;
		bool loop = true;
		int loop_dir = 1; //1 = forward, -1 = backwards
		int frameWidth;
		int frameHeight;
		AnimationCycle animationState;


		/// <summary>
		/// Controls the current AnimationCycle Row
		/// </summary>
		public enum AnimationCycle
		{
			Walk,
			Idle,
			Talk
		}

		


		/// <summary>
		/// Gets or sets the FPS of the animation
		/// </summary>
		public int FramesPerSecond
		{
			get { return (int)(1f / frameLength); }

			set { frameLength = 1f / (float)value; }
		}



		/// <summary>
		/// Gets the Rectangle containing the current frame of animation
		/// </summary>
		public Rectangle CurrentFrame
		{
			get { return frames[currentFrame, currentLine]; }
		}



		/// <summary>
		/// Gets or sets the current Line of the frame Animation
		/// </summary>
		public int CurrentLine
		{
			get { return this.currentLine; }
			//set { this.currentLine = value; }
		}

		public AnimationCycle AnimationState
		{
			get { return this.animationState; }
			set 
			{ 
				this.animationState = value;
				switch (this.animationState)
				{
					case AnimationCycle.Idle:
						this.setAnimationLine(1, 0);
						break;

					case AnimationCycle.Talk:
						this.setAnimationLine(2, 3);
						break;

					case AnimationCycle.Walk:
						this.setAnimationLine(0, 5);
						break;
				}
			}
		}



		/// <summary>
		/// Sets the Line of the Animation to play.
		/// </summary>
		/// <param name="Line">The 0-based Line of the Animation you wish to play</param>
		/// <param name="maxFrames">The 0-based Number of Frames the Animation has</param>
		private void setAnimationLine(int Line, int maxFrames)
		{
			this.currentLine = Line;
			this.maxFramesX = maxFrames;
		}



		/// <summary>
		/// Creates an animation object
		/// </summary>
		/// <param name="width">the total width of the input image</param>
		/// <param name="height">the height of the input image</param>
		/// <param name="numFrames">the number of frames in the sprite-sheet</param>
		/// <param name="xOffset">the X origin of the sprite sheet</param>
		/// <param name="yOffset">the y origin of the sprite sheet</param>
		public Animation(int width, int height, int numFramesX, int numFramesY, int xOffset, int yOffset, bool isLoop = true)
		{
			frames = new Rectangle[numFramesX, numFramesY];
			frameWidth = width / numFramesX;
			frameHeight = height / numFramesY;
			maxFramesX = numFramesX;
			loop = isLoop;

			for (int i = 0; i < numFramesX; i++)
			{
				for (int n = 0; n < numFramesY; n++)
				{
					frames[i, n] = new Rectangle(xOffset + (frameWidth * i), yOffset + (frameHeight * n), frameWidth, frameHeight);
				}
			}
		}



		/// <summary>
		/// update the animation
		/// </summary>
		/// <param name="elapsed">seconds since the last frame</param>
		public void Update(float elapsed)
		{
			timer += elapsed;

			if (this.animationState == AnimationCycle.Idle)
			{
				currentFrame = 0;
				currentLine = 1;
				return;
			}

			if (timer >= frameLength)
			{
				if (loop)
				{
					timer = 0f;
					currentFrame = (currentFrame + 1) % maxFramesX;
				}
				else
				{
					if (currentFrame + 1 == maxFramesX)
						loop_dir = -1;
					if (currentFrame == 0)
						loop_dir = 1;
					timer = 0f;
					currentFrame = currentFrame + loop_dir;
				}
			}
		}





		/// <summary>
		/// resets the animation
		/// </summary>
		public void Reset()
		{
			currentFrame = 0;
			timer = 0f;
		}

	}
}
