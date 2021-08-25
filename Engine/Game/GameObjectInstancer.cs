namespace Engine.Game
{
	public class GameObjectInstancer
	{
		public delegate GameObject InstantiateDelegate ();

		InstantiateDelegate instantiateFunc;

		public GameObjectInstancer(InstantiateDelegate instantiateFunc)
		{
			if (instantiateFunc == null) throw new System.ArgumentNullException("instantiateFunc");
			this.instantiateFunc = instantiateFunc;
		}

		public GameObject Instantiate()
		{
			return instantiateFunc();
		}
	}
}
