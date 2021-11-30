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
			Matrix4x4 a = new Matrix4x4
				(
					w, z, -y, x,
					-z, w, x, y,
					y, -x, w, z,
					-x, -y, -z, w
				);
			Matrix4x4 b = new Matrix4x4
				(
					w, z, -y, -x,
					-z, w, x, -y,
					y, -x, w, -z,
					x, y, z, w
				);
			return b * a;
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

		public static Quaternion AxisAngle(Vector3 axis, float angle)
		{
			axis = axis.normalized;
			Quaternion q = new Quaternion();
			float sin = Mathf.Sin(angle * 0.5f);
			q.x = axis.x * sin;
			q.y = axis.y * sin;
			q.z = axis.z * sin;
			q.w = Mathf.Cos(angle * 0.5f);
			return q;
		}

		public static Quaternion Euler(Vector3 euler)
		{
			return AxisAngle(Vector3.right, euler.x) * AxisAngle(Vector3.up, euler.y) * AxisAngle(Vector3.forward, euler.z);
		}

		public static Quaternion Euler(float x, float y, float z)
		{
			x *= 0.5f;
			y *= 0.5f;
			z *= 0.5f;

			float c1 = y.Cos();
			float c2 = z.Cos();
			float c3 = x.Cos();

			float s1 = y.Sin();
			float s2 = z.Sin();
			float s3 = x.Sin();

			Quaternion q = new Quaternion();
			q.w = c1 * c2 * c3 - s1 * s2 * s3;
			q.x = s1 * s2 * c3 + c1 * c2 * s3;
			q.y = s1 * c2 * c3 + c1 * s2 * s3;
			q.z = c1 * s2 * c3 - s1 * c2 * s3;

			return q;
		}

		public override string ToString()
		{
			return $"({x}, {y}, {z}, {w})";
		}
	}
}