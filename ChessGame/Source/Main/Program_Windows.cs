#if WINDOWS
using System;

namespace ChessGame
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application. 
        /// 
        /// https://msdn.microsoft.com/en-us/library/ms182351(VS.80).aspx
        /// STAThreadAttribute indicates that the COM threading model for the application is single-threaded apartment
        /// This attribute must be present on the entry point of any application that uses Windows Forms.
        /// If it is omitted, the Windows components might not work correctly.
        /// If the attribute is not present, the application uses the multithreaded apartment model, which is not supported for Windows Forms.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (ChessGame game = new ChessGame())
                game.Run(); // Start the game
        }
    }
}
#endif