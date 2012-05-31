using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Audio;

using Classes;
using Classes.Pipeline;

namespace Classes.IO
{
	class SoundManager
	{
		static SoundEffect click;

		public static void LoadContent()
		{
			click = GameRef.Game.Content.Load<SoundEffect>("Audio/UI/click");
		}


		public static void playBackgroundMusic(String assetName, bool loop)
		{
			SoundEffect music = GameRef.Game.Content.Load<SoundEffect>(assetName);
			music.Play();
		}

		public static void Click()
		{
			click.Play();
		}
	}
}
