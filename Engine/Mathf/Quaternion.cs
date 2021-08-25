namespace Engine
{
	public struct Quaternion
	{
		public float x, y, z, w;

		public static readonly Quaternion identity = new Quaternion(0f, 0f, 0f, 1f);

		public Quaternion(float x, float y, float z, float w)
		{
			float d = 1f / Mathf.Sqrt(x * x + y * y + z * z + w * w);

			if (float.IsNaN(d)) throw new System.ArgumentException($"Wrong quaternion arguments : {x},{y},{z},{w}");

			this.x = x * d;
			this.y = y * d;
			this.z = z * d;
			this.w = w * d;
		}

		public Matrix4x4 ToMatrix()
		{
			Vector4 c0 = new Vector4();
			Vector4 c1 = new Vector4();
			Vector4 c2 = new Vector4();
			Vector4 c3 = new Vector4(0f, 0f, 0f, 1f);

			c0.x = 1f - 2f * y * y - 2f * z * z;
			c0.y = 2f * x * y + 2f * z * w;
			c0.z = 2f * x * z - 2f * y * w;

			c1.x = 2f * x * y - 2f * z * w;
			c1.y = 1f - 2 * x * x - 2f * z * z;
			c1.z = 2f * y * z - 2f * x * w;

			c2.x = 2f * x * z + 2f * y * w;
			c2.y = 2f * y * z - 2f * z * w;
			c2.z = 1f - 2f * x * x - 2f * y * y;

			return new Matrix4x4(c0, c1, c2, c3);
		}

		public static Quaternion operator * (Quaternion a, Quaternion b)
		{
			float x, y, z, w;
			x = a.x * b.w + a.y * b.z - a.z * b.y + a.w * b.x;
			y = -a.x * b.z + a.y * b.w + a.z * b.x + a.w * b.y;
			z = a.x * b.y - a.y * b.x + a.z * b.w + a.w * b.z;
			w = -a.x * b.x - a.y * b.y - a.z * b.z + a.w * b.w;
			return new Quaternion(x, y, z, w);
		}
	}
}