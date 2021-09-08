namespace Engine.Rendering
{
	public class Model
	{
		public Model(int modelIndex)
		{
			this.modelIndex = modelIndex;
		}

		public int modelIndex { get; private set; }
	}
}
