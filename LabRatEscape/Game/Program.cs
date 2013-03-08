using System;

namespace LabRatEscape
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (LREGame game = new LREGame())
            {
                game.Run();
            }
        }
    }
}

