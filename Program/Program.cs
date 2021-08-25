using System;
using OpenTK;

namespace WinGL
{
	static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var mainWindow = new MainWindow(600, 400, "OpenGL test");
            Project.mainPanel = mainWindow;
            mainWindow.VSync = VSyncMode.Adaptive;
            try
            {
                mainWindow.Run();
            }
            finally
            {
                Engine.Game.GameThread.isPlaying = false;
            }
        }
    }
}