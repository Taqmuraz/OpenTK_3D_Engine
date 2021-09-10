using Engine;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace WinGL
{
	public class MasterRenderer
	{
		public static MasterRenderer masterRenderer { get; private set; }// = new MasterRenderer ();

		public static SafeList<Engine.Game.Renderer> renderers { get; private set; } = new List<Engine.Game.Renderer>();
		public static SafeList<RawModel> models { get; private set; } = new List<RawModel>();

		static Dictionary<string, int> modelIndices = new Dictionary<string, int>();

		static List<string> modelsToLoad = new List<string>();

		public static Engine.Rendering.Model LoadModel(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) throw new System.ArgumentException("fileName is null or empty");
			if (modelIndices.ContainsKey(fileName))
			{
				return new Engine.Rendering.Model(modelIndices[fileName]);
			}

			modelsToLoad.Add(fileName);
			int index = models.Count + modelsToLoad.Count - 1;
			modelIndices.Add(fileName, index);

			return new Engine.Rendering.Model(index);
		}

		Renderer renderer;

		void LoadModelFromQueue(string fileName)
		{
			var mesh = Engine.Rendering.OBJFileLoader.LoadOBJ(fileName);
			models.Add(Loader.LoadModel(mesh));
		}

		public MasterRenderer()
		{
			GL.Enable(EnableCap.DepthTest);
			//CreateProjectionMatrix();

			renderer = new Renderer();
		}

		public void Render(Engine.Game.Light sun, Engine.Game.Camera camera)
		{
			foreach (var model in modelsToLoad)
			{
				try
				{
					LoadModelFromQueue(model);
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
			}
			modelsToLoad.Clear();

			if (camera == null) return;

			Prepare(camera);

			renderer.Render(renderers, sun);
		}

		public static void SetCulling(bool enable)
		{
			if (enable)
			{
				GL.Enable(EnableCap.CullFace);
				GL.CullFace(CullFaceMode.Back);
			}
			else
			{
				GL.Disable(EnableCap.CullFace);
			}
		}

		public void CleanUp()
		{
			Shader.CleanShaders();
		}

		public void Prepare(Engine.Game.Camera camera)
		{
			var clearColor = camera.clearColor;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.ClearColor(clearColor.r, clearColor.g, clearColor.b, 1f);
		}

		private void CreateProjectionMatrix()
		{
			/*float aspectRatio = MainWindow.WindowWidth / MainWindow.WindowHeight;
			float y_scale = 1f / Mathf.Tan(FOV * 0.5f);
			float x_scale = y_scale / aspectRatio;
			float frustum_length = FAR_PLANE - NEAR_PLANE;

			projectionMatrix = Matrix4x4.identity;


			projectionMatrix.column_0.x = x_scale;
			projectionMatrix.column_1.y = y_scale;
			projectionMatrix.column_2.z = -((FAR_PLANE + NEAR_PLANE) / frustum_length);
			projectionMatrix.column_2.w = -1f;
			projectionMatrix.column_3.z = -((2f * NEAR_PLANE * FAR_PLANE) / frustum_length);
			projectionMatrix.column_3.w = 0f;

			//projectionMatrix.rotate(180f, new Vector3f(0f, 1f, 0f));
			*/
		}

		public static void CreateMasterRenderer()
		{
			masterRenderer = new MasterRenderer();
			//Shader.LoadShader("Diffuse", Shader.ShadersPath + "vertexShader.glsl", Shader.ShadersPath + "fragmentShader.glsl");
		}
	}
}