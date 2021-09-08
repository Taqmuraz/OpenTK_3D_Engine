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
            Debug.DebugStart();

            try
            {
                var mainWindow = new MainWindow(600, 400, "OpenGL test");
                Project.mainPanel = mainWindow;
                mainWindow.VSync = VSyncMode.Adaptive;
                try
                {
                    Engine.Game.GameThread.Start();
                    mainWindow.Run();
                }
                finally
                {
                    Engine.Game.GameThread.isPlaying = false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            finally
            {
                Debug.DebugEnd();
            }
        }
    }
}