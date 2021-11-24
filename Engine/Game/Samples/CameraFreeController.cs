namespace Engine.Game
{
	public class Soldier : Component
	{
		[BehaviourEvent]
		void Update()
		{
			Vector2 rotate = new Vector2();

			if (Input.GetKey(KeyCode.Left)) rotate.y -= 1f;
			if (Input.GetKey(KeyCode.Right)) rotate.y += 1f;
			if (Input.GetKey(KeyCode.Up)) rotate.x -= 1f;
			if (Input.GetKey(KeyCode.Down)) rotate.x += 1f;

			rotate *= Time.deltaTime * 360f;

			transform.localEulerAngles += rotate;
		}
	}
	public class RendererDisableController : Component
	{
		[BehaviourEvent]
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Y)) gameObject.Destroy();
		}
	}
	public class CameraFreeController : Component
	{
		[BehaviourEvent]
		void Update()
		{
			Vector3 input = new Vector3();
			Vector2 mouse = Input.mouseDelta;
			Vector3 rotate = new Vector3(-mouse.y, -mouse.x, 0f);
			float zoom = 0f;
			if (Input.GetKey(KeyCode.W)) input.z += 1f;
			if (Input.GetKey(KeyCode.S)) input.z -= 1f;
			if (Input.GetKey(KeyCode.A)) input.x -= 1f;
			if (Input.GetKey(KeyCode.D)) input.x += 1f;
			if (Input.GetKey(KeyCode.Q)) input.y -= 1f;
			if (Input.GetKey(KeyCode.E)) input.y += 1f;

			if (Input.GetKey(KeyCode.V)) zoom += 1f;
			if (Input.GetKey(KeyCode.B)) zoom -= 1f;

			rotate *= Time.deltaTime * 15f;

			transform.localEulerAngles += rotate;

			transform.position += transform.localToWorld.MultiplyVector(input) * Time.deltaTime * 5;

			Camera.mainCamera.fieldOfView += zoom * Time.deltaTime * 50f;
		}
	}
}
