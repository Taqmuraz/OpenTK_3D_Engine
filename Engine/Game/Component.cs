namespace Engine.Game
{

	public class Component : BehaviourEventsHandler, IInitializable<GameObject>
	{
		bool destroyed;

		public string name => gameObject.name;

		public Transform transform { get; private set; }

		public GameObject gameObject { get; private set; }

		protected override void OnEventCall(string name)
		{
			base.OnEventCall(name);
			if (gameObject == null) throw new System.Exception("Can't call event on destroyed component");
		}

		void IInitializable<GameObject>.Initialize(GameObject arg)
		{
			gameObject = arg;
			transform = gameObject.transform;
		}

		public void Destroy()
		{
			if (!destroyed)
			{
				gameObject.RemoveComponent(this);
				destroyed = true;
			}
		}

		public override string ToString()
		{
			return gameObject.name;
		}
	}
}
