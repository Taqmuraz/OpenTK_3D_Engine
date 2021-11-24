using System;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;

namespace WinGL
{
    class TestForm : Form
    {
        public TestForm()
		{

		}

		protected override void OnPaint(PaintEventArgs e)
		{
            Engine.Rendering.Mesh mesh = Engine.Rendering.OBJFileLoader.LoadOBJ("./Data/Models/Soldier.obj");
			for (int i = 0; i < mesh.indices.Length; i++)
			{
                int index = mesh.indices[i];
                //PointF p = new PointF(mesh.vertices[index], mesh.vertices[index]);
			}
		}
	}

	public static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //Application.Run(new TestForm());
            //return;
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