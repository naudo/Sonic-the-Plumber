using System;

namespace XSonic
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (XSonicGame game = new XSonicGame())
            {
                game.Run();
            }
        }
    }
}

