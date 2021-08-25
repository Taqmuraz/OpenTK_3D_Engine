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
            var mainWindow = new MainWindow(600, 400, "Five USSR's");
            mainWindow.VSync = VSyncMode.Adaptive;
            new OTKProject(mainWindow).CreateMainPanel(null);
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