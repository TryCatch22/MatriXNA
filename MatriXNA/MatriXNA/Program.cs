using System;

namespace MatriXNA
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MatriXNA game = new MatriXNA())
            {
                game.Run();
            }
        }
    }
#endif
}

