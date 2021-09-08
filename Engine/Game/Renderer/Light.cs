namespace Engine.Game
{
	public class Light : Component
	{
		public enum LightType
		{
			Directional = 0,
			Spot = 1,
			Point = 2
		}

		public LightType lightType { get; set; } = LightType.Directional;
		public Color32 lightColor { get; set; } = Color32.white;
		public float lightBrightness { get; set; } = 1.0f;
	}
}
