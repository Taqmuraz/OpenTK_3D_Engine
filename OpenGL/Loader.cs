using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace WinGL
{
	public static class Loader
    {
		private static List<int> vaos = new List<int>();
		private static List<int> vbos = new List<int>();
		private static Dictionary<string, int> textures = new Dictionary<string, int>();

		public static RawModel LoadToVAO(float[] positions, float[] textureCoords, float[] normals, int[] indices)
		{
			int vaoID = CreateVAO();
			BindIndicesBuffer(indices);

			StoreDataInAttributeList(0, 3, positions);
			StoreDataInAttributeList(1, 2, textureCoords);
			StoreDataInAttributeList(2, 3, normals);

			UnbindVAO();
			return new RawModel(vaoID, indices.Length);
		}

		public static int LoadTexture(string fileName)
		{
			if (string.IsNullOrEmpty(fileName)) throw new System.ArgumentNullException("fileName");

			if (textures.ContainsKey(fileName)) return textures[fileName];

			Bitmap bmp = new Bitmap(1, 1);
			if (File.Exists(fileName))
			{
				bmp = (Bitmap)Image.FromFile(fileName);
				bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			}

			int width = bmp.Width;
			int height = bmp.Height;
			BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			int id = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, id);

			textures.Add(fileName, id);

			int linear = (int)TextureMinFilter.Linear;
			int repeat = (int)TextureWrapMode.Repeat;
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, repeat);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			bmp.UnlockBits(data);

			return id;
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
				GL.DeleteTexture(texture.Value);
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
			GL.BufferData( BufferTarget.ArrayBuffer, data.Length, data, BufferUsageHint.StaticDraw);
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
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length, indices, BufferUsageHint.StaticDraw);
		}
	}
}