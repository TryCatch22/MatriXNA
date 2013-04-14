using System;

namespace Matrix
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Matrix game = new Matrix())
            {
                game.Run();
            }
        }
    }
#endif
}

