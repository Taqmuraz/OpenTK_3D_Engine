using Engine;

namespace WinGL
{
	public class ModelTexture
	{

		public int textureID;

		public Vector2 offset { get; set; } = new Vector2(0f, 0f);
		public Vector2 tiling { get; set; } = new Vector2(1f, 1f);

		public ModelTexture(string name)
		{
			textureID = Loader.LoadTexture(name);
		}

		public ModelTexture(int id)
		{
			textureID = id;
		}
	}
}