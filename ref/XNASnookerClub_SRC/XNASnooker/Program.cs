using System;
//using xnaIntro;

namespace Snooker.Client.XNA
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            using (XNASnooker game = new XNASnooker())
            {
                game.Run();
            }
            //using (Game1 game = new Game1())
            //{
            //    game.Run();
            //}
        }
    }
}

