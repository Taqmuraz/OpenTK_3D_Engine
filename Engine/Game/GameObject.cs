using System.Collections.Generic;
using System.Linq;

namespace Engine.Game
{
	public sealed class GameObject : GameThread.GameThreadHandler
	{
		public Transform transform { get; private set; }

		public string name { get; set; }

		public GameObject(string name)
		{
			this.name = name;
			transform = AddComponent<Transform>();
			componentSearch = components.Concat(inOrderToAdd).Except(inOrderToRemove);
			GameScene.activeScene.AddGameObject(this);
		}

		List<Component> components = new List<Component>();
		List<Component> inOrderToAdd = new List<Component>();
		List<Component> inOrderToRemove = new List<Component>();

		IEnumerable<Component> componentSearch;

		public override string ToString()
		{
			return name;
		}

		public TComponent AddComponent<TComponent>() where TComponent : Component, IInitializable<GameObject>, new()
		{
			var component = new TComponent();
			component.Initialize(this);
			inOrderToAdd.Add(component);
			component.CallEvent("Start");
			return component;
		}
		public void RemoveComponent(Component component)
		{
			try
			{
				component.CallEvent("OnDestroy");
			}
			catch (System.Exception ex)
			{
				Debug.LogError(ex);
			}
			inOrderToRemove.Add(component);
		}

		[BehaviourEvent]
		void Update()
		{
			components.AddRange(inOrderToAdd);
			inOrderToAdd.Clear();
			for (int i = 0; i < inOrderToRemove.Count; i++) components.Remove(inOrderToRemove[i]);
			inOrderToRemove.Clear();
		}

		public TComponent GetComponent<TComponent> ()
		{
			foreach (var component in componentSearch)
			{
				if (component is TComponent t) return t;
			}
			return default;
		}

		protected sealed override void OnEventCall(string name)
		{
			foreach (var component in components)
			{
				try
				{
					component.CallEvent(name);
				}
				catch (System.Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
			}
		}
	}
}
