using Engine.Rendering;

namespace Engine.Game
{
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
