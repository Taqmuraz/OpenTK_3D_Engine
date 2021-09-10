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
			float zoom = 0f;
			if (Input.GetKey(KeyCode.W)) input.z += 1f;
			if (Input.GetKey(KeyCode.S)) input.z -= 1f;
			if (Input.GetKey(KeyCode.A)) input.x -= 1f;
			if (Input.GetKey(KeyCode.D)) input.x += 1f;
			if (Input.GetKey(KeyCode.Q)) input.y -= 1f;
			if (Input.GetKey(KeyCode.E)) input.y += 1f;

			if (Input.GetKey(KeyCode.V)) zoom += 1f;
			if (Input.GetKey(KeyCode.B)) zoom -= 1f;

			transform.position += input * Time.deltaTime * 5;

			Camera.mainCamera.fieldOfView += zoom * Time.deltaTime * 50f;
		}
	}
}
