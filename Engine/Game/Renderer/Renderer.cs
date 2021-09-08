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
		public Texture mainTexture { get; private set; }
		public bool cullFaces { get; private set; } = true;
	}
	public class Renderer : Component
	{
		public Material material { get; set; }
		public Model model { get; set; }

		[BehaviourEvent]
		void Start()
		{
			Project.mainPanel.RegisterRenderer(this);
		}
		[BehaviourEvent]
		void OnDestroy()
		{
			Project.mainPanel.UnregisterRenderer(this);
		}
	}
}
