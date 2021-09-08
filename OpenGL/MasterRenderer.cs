using Engine;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace WinGL
{
	public class MasterRenderer
	{
		private static MasterRenderer masterRenderer;// = new MasterRenderer ();

		public static MasterRenderer getMasterRenderer()
		{
			return masterRenderer;
		}

		private const float FOV = 70f;
		private const float NEAR_PLANE = 0.01f;
		private const float FAR_PLANE = 1000f;

		public static SafeList<Engine.Game.Renderer> renderers { get; private set; } = new List<Engine.Game.Renderer>();
		public static SafeList<RawModel> models { get; private set; } = new List<RawModel>();

		static Dictionary<string, int> modelIndices = new Dictionary<string, int>();

		public static Engine.Rendering.Model LoadModel(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) throw new System.ArgumentException("fileName is null or empty");
			if (modelIndices.ContainsKey(fileName))
			{
				return new Engine.Rendering.Model(modelIndices[fileName]);
			}

			var mesh = Engine.Rendering.OBJFileLoader.LoadOBJ(fileName);
			models.Add(Loader.LoadToVAO(mesh.vertices, mesh.textureCoords, mesh.normals, mesh.indices));
			modelIndices.Add(fileName, models.Count - 1);
			return new Engine.Rendering.Model(models.Count - 1);
		}

		private static Color32 clearColor { get; set; } = new Color32(0.7f, 0.7f, 1.0f, 1.0f);

		private Matrix4x4 projectionMatrix;

		Renderer renderer;

		public MasterRenderer()
		{
			GL.Enable(EnableCap.DepthTest);
			CreateProjectionMatrix();

			var shader = new StaticShader();

			renderer = new Renderer(shader, projectionMatrix);
		}

		public void Render(Light sun, Camera camera)
		{
			Prepare();

			renderer.shader.start();
			renderer.shader.loadLight(sun);
			renderer.shader.loadProjectionMatrix(projectionMatrix);
			renderer.shader.loadViewMatrix(camera);

			renderer.render(renderers);
			renderer.shader.stop();
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
			renderer.shader.cleanUp();
		}

		public void Prepare()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			GL.ClearColor(clearColor.r, clearColor.g, clearColor.b, 1f);
		}

		private void CreateProjectionMatrix()
		{
			float aspectRatio = MainWindow.WindowWidth / MainWindow.WindowHeight;
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
		}

		public static void CreateMasterRenderer()
		{
			masterRenderer = new MasterRenderer();
		}
	}
}