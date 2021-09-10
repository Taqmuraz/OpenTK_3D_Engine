namespace WinGL
{
	public class RawModel
	{
		public int vaoID { get; set; }
		public int vertexCount { get; private set; }

		public RawModel(int vaoID, int vertexCount)
		{
			this.vaoID = vaoID;
			this.vertexCount = vertexCount;
		}
	}
}