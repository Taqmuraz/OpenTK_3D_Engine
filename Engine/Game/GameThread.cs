using System.Threading;
using System.Collections.Generic;

namespace Engine.Game
{
	public static class GameThread
	{
		static Thread thread;

		static List<BehaviourEventsHandler> gameHandlers = new List<BehaviourEventsHandler>();
		static List<BehaviourEventsHandler> handlersToAdd = new List<BehaviourEventsHandler>();
		static List<BehaviourEventsHandler> handlersToRemove = new List<BehaviourEventsHandler>();

		public abstract class GameThreadHandler : BehaviourEventsHandler
		{
			bool m_active;
			public bool isActive
			{
				get => m_active && !destroyed;
				set => m_active = value;
			}

			bool destroyed;

			public GameThreadHandler()
			{
				isActive = true;
				handlersToAdd.Add(this);
			}

			public void Destroy()
			{
				if (destroyed) return;

				try
				{
					CallEvent("OnDestroy");
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				handlersToRemove.Add(this);
				destroyed = true;
			}
		}

		public static void Start()
		{
			Time.StartTime();
			Input.StartInput();

			isPlaying = true;
			thread = new Thread(ThreadStart);

			thread.Start();
		}

		public static bool isPlaying { get; set; }

		static void SendHandlersMessage(string message)
		{
			if (handlersToAdd.Count != 0) gameHandlers.AddRange(handlersToAdd);
			for (int i = 0; i < handlersToRemove.Count; i++) gameHandlers.Remove(handlersToRemove[i]);
			handlersToAdd.Clear();
			handlersToRemove.Clear();

			lock (gameHandlers)
			{
				foreach (var handler in gameHandlers)
				{
					try
					{
						handler.CallEvent(message);
					}
					catch (System.Exception ex)
					{
						Debug.LogError(ex);
						//Project.instance.mainPanel.Log(ex.ToString());
					}
				}
			}
		}

		public static object updateLock = new object();

		static void ThreadStart()
		{
			//GameScenes.gameScenes[1].LoadScene();
			//GameScenes.LoadScene(1);
			var camera = new GameObject("Camera").AddComponent<Camera>();

			camera.gameObject.AddComponent<CameraFreeController>();

			var model = Project.mainPanel.LoadModel("./Data/Models/Soldier.obj");
			var texture = Project.mainPanel.LoadTexture("./Data/Models/Soldier.png");
			var shader = Shader.LoadShader("default");

			Vector3[] rotations = new Vector3[3];
			rotations[0] = new Vector3(0f, 0f, -90f);
			rotations[1] = Vector3.zero;
			rotations[2] = new Vector3(90f, 0f, 0f);

			camera.transform.position = new Vector3(0f, 1f, 5f);
			for (int i = 0; i < 3; i++)
			{
				var soldier = new GameObject("Soldier_" + i.ToString()).AddComponent<Renderer>();
				soldier.gameObject.AddComponent<Soldier>();
				soldier.material = new Material(shader);
				soldier.material.mainTexture = texture;
				soldier.model = model;
				soldier.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
				Vector3 pos = new Vector3();
				pos[i] = 1f;
				soldier.transform.localEulerAngles = rotations[i];
				soldier.transform.position = pos;
				soldier.material.color = new Color32(pos.x, pos.y, pos.z, 1f);
			}

			var flyStation = new GameObject("Station").AddComponent<Renderer>();
			flyStation.material = new Material(Shader.LoadShader("default"));
			flyStation.material.mainTexture = Project.mainPanel.LoadTexture("./Data/Models/Grass.png");
			flyStation.model = Project.mainPanel.LoadModel("./Data/Models/CollisionMap.obj");
			flyStation.material.tiling = new Vector2(10f, 10f);

			//var cube = new GameObject("Cube").AddComponent<Renderer>();
			//cube.material = soldier.material;
			//cube.model = Project.mainPanel.LoadModel("./Data/Models/FlyStation.obj");
			//cube.gameObject.AddComponent<RendererDisableController>();

			while (isPlaying)
			{
				lock (updateLock)
				{
					try
					{
						Time.Update();

						SendHandlersMessage("EarlyUpdate");

						//Collider.UpdatePhysics();

						SendHandlersMessage("Update");

						SendHandlersMessage("LateUpdate");

						Input.UpdateInput();

						SendHandlersMessage("OnPreRender");
						//Project.instance.mainPanel.DrawCall();
						SendHandlersMessage("OnPostRender");

						//Project.Log(Time.deltaTime.ToString("F3"));
					}
					catch (System.Exception ex)
					{
						Debug.LogError(ex);
						//new ErrorForm(ex).ShowDialog();
						break;
					}
				}
				Thread.Sleep(10);
			}
		}
	}
}
