using System;

namespace Adventure_Prototype
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
				foreach (String s in args)
				{
					if (s == "-editor")
					{
						game._EDITOR = true;
					}

					if (s == "-noMusic")
					{
						game.musicVolume  = 0.0f;
					}
				}
                game.Run();
            }
        }
    }
#endif
}

