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
			if (Input.GetKey(KeyCode.Up)) rotate.x += 1f;
			if (Input.GetKey(KeyCode.Down)) rotate.x -= 1f;

			rotate *= Time.deltaTime * 360f;

			transform.localEulerAngles += rotate;
			//transform.localRotation *= Quaternion.Euler(rotate);
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
		Vector3 euler;

		[BehaviourEvent]
		void Update()
		{
			Vector3 input = new Vector3();
			Vector2 mouse = Input.mouseDelta;
			Vector3 rotate;
			float zoom = 0f;
			if (Input.GetKey(KeyCode.MouseR))
			{
				input.x = mouse.x * -0.1f;
				input.y = mouse.y * 0.1f;
				rotate = Vector3.zero;
			}
			else
			{
				rotate = new Vector3(mouse.y, mouse.x, 0f);
			}

			if (Input.GetKey(KeyCode.W)) input.z += 1f;
			if (Input.GetKey(KeyCode.S)) input.z -= 1f;
			if (Input.GetKey(KeyCode.A)) input.x -= 1f;
			if (Input.GetKey(KeyCode.D)) input.x += 1f;
			if (Input.GetKey(KeyCode.Q)) input.y -= 1f;
			if (Input.GetKey(KeyCode.E)) input.y += 1f;

			/*float rs = 10f;
			if (Input.GetKey(KeyCode.Left)) rotate.y -= rs;
			if (Input.GetKey(KeyCode.Right)) rotate.y += rs;
			if (Input.GetKey(KeyCode.Up)) rotate.x -= rs;
			if (Input.GetKey(KeyCode.Down)) rotate.x += rs;
			*/
			if (Input.GetKey(KeyCode.V)) zoom += 1f;
			if (Input.GetKey(KeyCode.B)) zoom -= 1f;

			rotate *= Time.deltaTime * 15f;

			euler += rotate;
			euler.x = Mathf.Clamp(euler.x, -70f, 70f);

			transform.localEulerAngles = euler;
			//transform.localRotation = Quaternion.AxisAngle(Vector3.right, euler.x) * Quaternion.AxisAngle(Vector3.up, euler.y);

			transform.position += transform.localToWorld.MultiplyVector(input) * Time.deltaTime * 5;

			Camera.mainCamera.fieldOfView += zoom * Time.deltaTime * 50f;
		}
	}
}
