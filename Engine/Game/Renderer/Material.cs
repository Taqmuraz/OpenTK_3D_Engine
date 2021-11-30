using Engine.Rendering;

namespace Engine.Game
{
	public sealed class Material
	{
		public Material(Shader shader)
		{
			this.shader = shader;
		}
		public Shader shader { get; private set; }
		public Texture mainTexture { get; set; }
		public Color32 color { get; set; } = Color32.white;
		public bool cullFaces { get; private set; } = true;

		public Vector2 tiling { get; set; } = Vector2.one;
		public Vector2 offset { get; set; }
	}
}
