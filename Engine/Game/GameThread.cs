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

			//TexturesLoader.LoadTextures("./Data/Textures");

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
