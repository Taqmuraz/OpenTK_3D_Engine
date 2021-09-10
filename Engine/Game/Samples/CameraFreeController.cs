namespace Engine.Game
{
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
			Vector2 rotate = new Vector2();
			float zoom = 0f;
			if (Input.GetKey(KeyCode.W)) input.z += 1f;
			if (Input.GetKey(KeyCode.S)) input.z -= 1f;
			if (Input.GetKey(KeyCode.A)) input.x -= 1f;
			if (Input.GetKey(KeyCode.D)) input.x += 1f;
			if (Input.GetKey(KeyCode.Q)) input.y -= 1f;
			if (Input.GetKey(KeyCode.E)) input.y += 1f;

			if (Input.GetKey(KeyCode.V)) zoom += 1f;
			if (Input.GetKey(KeyCode.B)) zoom -= 1f;

			if (Input.GetKey(KeyCode.Left)) rotate.y -= 1f;
			if (Input.GetKey(KeyCode.Right)) rotate.y += 1f;
			if (Input.GetKey(KeyCode.Up)) rotate.x -= 1f;
			if (Input.GetKey(KeyCode.Down)) rotate.x += 1f;

			rotate *= Time.deltaTime * 90f;

			transform.localRotation *= Quaternion.Euler(rotate.x, rotate.y, 0f);

			transform.position += input * Time.deltaTime * 5;

			Camera.mainCamera.fieldOfView += zoom * Time.deltaTime * 50f;
		}
	}
}
