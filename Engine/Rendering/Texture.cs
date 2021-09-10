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

		public int width { get; set; }
		public int height { get; set; }
		public int textureIndex { get; set; }
	}
}
