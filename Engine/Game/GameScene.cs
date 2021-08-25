using System.Collections.Generic;

namespace Engine.Game
{
	public class GameScene
	{
		List<GameObjectInstancer> instancers = new List<GameObjectInstancer>();
		List<GameObject> gameObjects = new List<GameObject>();

		static readonly GameScene emptyScene = new GameScene();
		public static GameScene activeScene { get; private set; } = emptyScene;

		public void AddGameObjectInstancer (GameObjectInstancer instancer)
		{
			instancers.Add(instancer);
		}
		public void AddGameObjectInstancer(GameObjectInstancer.InstantiateDelegate instantiateFunc)
		{
			instancers.Add(new GameObjectInstancer(instantiateFunc));
		}
		public void AddGameObject(GameObject gameObject)
		{
			gameObjects.Add(gameObject);
		}

		public void LoadScene()
		{
			if (activeScene == this)
			{
				Debug.LogError("Scene already loaded");
				return;
			}

			activeScene.UnloadScene();
			activeScene = this;

			foreach (var instancer in instancers)
			{
				try
				{
					instancer.Instantiate();
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex);
				}
			}
		}
		public void UnloadScene()
		{
			if (activeScene != this)
			{
				Debug.LogError("Scene not loaded yet, nothing to unload");
				return;
			}

			foreach (var gameObject in gameObjects)
			{
				gameObject.Destroy();
			}

			activeScene = emptyScene;
		}
	}
}
