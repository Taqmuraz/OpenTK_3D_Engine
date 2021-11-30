using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace WinGL
{
	public static class Loader
    {
		private static List<int> vaos = new List<int>();
		private static List<int> vbos = new List<int>();
		private static Dictionary<string, Engine.Rendering.Texture> textures = new Dictionary<string, Engine.Rendering.Texture>();
		static Dictionary<Engine.Rendering.Mesh, RawModel> models = new Dictionary<Engine.Rendering.Mesh, RawModel>();

		static List<(Engine.Rendering.Mesh mesh, RawModel model)> modelsToLoad = new List<(Engine.Rendering.Mesh mesh, RawModel model)>();

		static void LoadToVAO(Engine.Rendering.Mesh mesh, RawModel model)
		{
			model.vaoID = CreateVAO();
			BindIndicesBuffer(mesh.indices);

			StoreDataInAttributeList(0, 3, mesh.vertices);
			StoreDataInAttributeList(1, 2, mesh.textureCoords);
			StoreDataInAttributeList(2, 3, mesh.normals);

			UnbindVAO();
		}

		public static RawModel LoadModel(Engine.Rendering.Mesh mesh)
		{
			if (mesh == null) throw new System.ArgumentNullException("mesh");
			else if (models.ContainsKey(mesh)) return models[mesh];
			else
			{
				var model = new RawModel(0, mesh.indices.Length);
				modelsToLoad.Add((mesh, model));
				models.Add(mesh, model);
				return model;
			}
		}

		struct TextureToLoad
		{
			public readonly string file;
			public readonly Engine.Rendering.Texture texture;

			public TextureToLoad(string file, Engine.Rendering.Texture texture)
			{
				this.file = file;
				this.texture = texture;
			}
		}

		static List<TextureToLoad> texturesToLoad = new List<TextureToLoad>();

		public static Engine.Rendering.Texture LoadTexture(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) throw new System.ArgumentNullException("fileName");

			if (textures.ContainsKey(fileName))
			{
				return textures[fileName];
			}
			else
			{
				var texture = new Engine.Rendering.Texture(0, 1, 1);
				textures.Add(fileName, texture);
				lock (texturesToLoad)
				{
					texturesToLoad.Add(new TextureToLoad(fileName, texture));
				}
				return texture;
			}
		}

		public static void UpdateLoader()
		{
			lock (texturesToLoad)
			{
				foreach (var texture in texturesToLoad)
				{
					try
					{
						ProcessTexture(texture);
					}
					catch (System.Exception ex)
					{
						Debug.LogError(ex);
					}
				}
				texturesToLoad.Clear();
			}

			lock (modelsToLoad)
			{
				foreach (var model in modelsToLoad)
				{
					try
					{
						LoadToVAO(model.mesh, model.model);
					}
					catch (System.Exception ex)
					{
						Debug.LogError(ex);
					}
				}
				modelsToLoad.Clear();
			}
		}

		static void ProcessTexture(TextureToLoad tex)
		{
			Bitmap bmp = new Bitmap(1, 1);
			if (File.Exists(tex.file))
			{
				bmp = (Bitmap)Image.FromFile(tex.file);
			}

			int width = bmp.Width;
			int height = bmp.Height;
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			int id = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, id);

			tex.texture.textureIndex = id;
			tex.texture.width = width;
			tex.texture.height = height;

			int linear = (int)TextureMinFilter.Linear;
			int repeat = (int)TextureWrapMode.Repeat;
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, repeat);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			bmp.UnlockBits(data);
		}

		public static void CleanUp()
		{
			foreach (int vao in vaos)
			{
				GL.DeleteVertexArray(vao);
			}
			foreach (int vbo in vbos)
			{
				GL.DeleteBuffer(vbo);
			}
			foreach (var texture in textures)
			{
				GL.DeleteTexture(texture.Value.textureIndex);
			}
		}

		private static int CreateVAO()
		{
			int vaoID = GL.GenVertexArray();
			vaos.Add(vaoID);
			GL.BindVertexArray(vaoID);
			return vaoID;
		}
		private static int CreateVBO()
		{
			int vboID = GL.GenBuffer();
			vbos.Add(vboID);
			return vboID;
		}
		private static void StoreDataInAttributeList(int attributeNumber, int coordinateSize, float[] data)
		{
			int vboID = CreateVBO();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
			GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(attributeNumber, coordinateSize, VertexAttribPointerType.Float, false, 0, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
		private static void UnbindVAO()
		{
			GL.BindVertexArray(0);
		}
		private static void BindIndicesBuffer(int[] indices)
		{
			int vboID = CreateVBO();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
		}
	}
}