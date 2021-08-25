namespace Engine.Rendering
{
	public class Mesh
	{
		private float[] vertices;
		private float[] textureCoords;
		private float[] normals;
		private int[] indices;
		private float furthestPoint;

		public Mesh(float[] vertices, float[] textureCoords, float[] normals, int[] indices,
				float furthestPoint)
		{
			this.vertices = vertices;
			this.textureCoords = textureCoords;
			this.normals = normals;
			this.indices = indices;
			this.furthestPoint = furthestPoint;
		}
	}
}
