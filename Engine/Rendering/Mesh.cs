namespace Engine.Rendering
{
	public class Mesh
	{
		public float[] vertices { get; set; }
		public float[] textureCoords { get; set; }
		public float[] normals { get; set; }
		public int[] indices { get; set; }
		public float furthestPoint { get; set; }

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
