namespace Engine.Rendering
{
	public class Texture
	{
		public Texture(int width, int height, int textureIndex)
		{
			this.width = width;
			this.height = height;
			this.textureIndex = textureIndex;
		}

		public int width { get; private set; }
		public int height { get; private set; }
		public int textureIndex { get; private set; }
	}
}
