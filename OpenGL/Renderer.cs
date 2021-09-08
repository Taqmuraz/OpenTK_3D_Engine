using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Engine;

namespace WinGL
{
	public class Renderer
	{
		public const int ATTRIBUTES_COUNT = 3;
		public StaticShader shader { get; private set; }

		public Renderer(StaticShader shader, Matrix4x4 projectionMatrix)
		{
			this.shader = shader;
			shader.start();
			shader.loadProjectionMatrix(projectionMatrix);
			shader.stop();
		}

		public void render(SafeList<Engine.Game.Renderer> renderers)
		{
			foreach (Engine.Game.Renderer renderer in renderers)
			{
				PrepareTexturedModel(renderer);
				PrepareInstance(renderer);
				GL.DrawElements(BeginMode.Triangles, MasterRenderer.models[renderer.model.modelIndex].vertexCount, DrawElementsType.UnsignedInt, 0);
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

			Engine.Rendering.Texture texture = renderer.material.mainTexture;

			MasterRenderer.SetCulling(renderer.material.cullFaces);

			shader.loadTextureParams(texture);

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
			shader.loadTransformationMatrix(transformationMatrix);
		}
	}
}