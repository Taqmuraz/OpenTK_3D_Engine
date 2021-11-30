using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Engine;

namespace WinGL
{
	public class Renderer
	{
		public const int ATTRIBUTES_COUNT = 3;

		Shader shader;

		public Renderer()
		{
		}

		public void Render(SafeList<Engine.Game.Renderer> renderers, Engine.Game.Light sun)
		{
			Matrix4x4 proj = Engine.Game.Camera.mainCamera.projectionMatrix;
			Matrix4x4 view = Engine.Game.Camera.mainCamera.transform.localToWorld.GetInversed();

			proj.column_2 = -proj.column_2;

			GL.FrontFace(FrontFaceDirection.Cw);

			foreach (Engine.Game.Renderer renderer in renderers)
			{
				if (MasterRenderer.models.Count <= renderer.model.modelIndex) continue;

				if (renderer.material == null) shader = Shader.GetShader("default");
				else shader = Shader.GetShader(renderer.material.shader.shaderName);

				shader.Start();
				if (sun != null) shader.LoadLight(sun);
				shader.LoadProjectionMatrix(proj);
				shader.LoadViewMatrix(view);
				shader.SetFloat("time", Engine.Game.Time.time);

				PrepareTexturedModel(renderer);
				PrepareInstance(renderer);
				GL.DrawElements(BeginMode.Triangles, MasterRenderer.models[renderer.model.modelIndex].vertexCount, DrawElementsType.UnsignedInt, 0);

				shader.Stop();
			}
			UnbindTexturedModel();
		}

		private void PrepareTexturedModel(Engine.Game.Renderer renderer)
		{
			RawModel model = MasterRenderer.models[renderer.model.modelIndex];
			GL.BindVertexArray(model.vaoID);

			int attributesCount = 3;

			for (int i = 0; i < attributesCount; i++)
			{
				GL.EnableVertexAttribArray(i);
			}

			MasterRenderer.SetCulling(renderer.material.cullFaces);

			shader.LoadTextureParams(renderer.material);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, renderer.material.mainTexture.textureIndex);
		}

		private void UnbindTexturedModel()
		{
			for (int i = 0; i < ATTRIBUTES_COUNT; i++)
			{
				GL.DisableVertexAttribArray(i);
			}
			GL.BindVertexArray(0);
		}

		protected void PrepareInstance(Engine.Game.Renderer renderer)
		{
			Matrix4x4 transformationMatrix = renderer.transform.localToWorld;
			shader.LoadTransformationMatrix(transformationMatrix);
		}
	}
}