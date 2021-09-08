namespace Engine.Game
{
	public sealed class Camera : Component
	{
		public static Camera mainCamera { get; private set; }

		public Color32 clearColor { get; set; } = new Color32(0.7f, 0.7f, 1.0f, 1.0f);

		public float fieldOfView { get; set; } = 60f;
		public float nearPlane { get; set; } = 0.01f;
		public float farPlane { get; set; } = 1500f;

		public Matrix4x4 projectionMatrix
		{
			get
			{
				return Matrix4x4.CreateFrustumMatrix(60f, (float)Project.mainPanel.screenWidth / (float)Project.mainPanel.screenHeight, nearPlane, farPlane);
			}
		}

		[BehaviourEvent]
		void Start()
		{
			mainCamera = this;
		}
	}
}
