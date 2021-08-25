using System;
using System.Drawing;

namespace Engine
{
	public struct Vector2
	{
		public float x, y;

		public Vector2 (float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public float this[int index]
		{
			get => index == 0 ? x : y;
			set
			{
				switch (index)
				{
					case 0: x = value; break;
					case 1: y = value; break;
					default: throw new System.IndexOutOfRangeException();
				}
			}
		}

		public override string ToString ()
		{
			return $"( {x.ToString("F3")}, {y.ToString("F3")} )";
		}

		public static explicit operator Point (Vector2 v)
		{
			return new Point ((int)v.x, (int)v.y);
		}
		public static implicit operator PointF (Vector2 v)
		{
			return new PointF (v.x, v.y);
		}

		public static explicit operator Size(Vector2 v)
		{
			return new Size((int)v.x, (int)v.y);
		}
		public static implicit operator SizeF(Vector2 v)
		{
			return new SizeF(v.x, v.y);
		}

		public static explicit operator Vector2 (Point v)
		{
			return new Vector2 (v.X, v.Y);
		}
		public static implicit operator Vector2 (PointF v)
		{
			return new Vector2 (v.X, v.Y);
		}

		public static explicit operator Vector2(Size v)
		{
			return new Vector2(v.Width, v.Height);
		}
		public static implicit operator Vector2(SizeF v)
		{
			return new Vector2(v.Width, v.Height);
		}

		public static bool operator == (Vector2 a, Vector2 b)
		{
			return (a - b).length <= Mathf.Epsilon;
		}
		public static bool operator != (Vector2 a, Vector2 b)
		{
			return (a - b).length >= Mathf.Epsilon;
		}

		public static Vector2 operator + (Vector2 a, Vector2 b)
		{
			return new Vector2 (a.x + b.x, a.y + b.y);
		}
		public static Vector2 operator - (Vector2 a, Vector2 b)
		{
			return new Vector2 (a.x - b.x, a.y - b.y);
		}
		public static Vector2 operator * (Vector2 a, float b)
		{
			return new Vector2 (a.x * b, a.y * b);
		}
		public static Vector2 operator / (Vector2 a, float b)
		{
			return b == 0 ? new Vector2() : new Vector2 (a.x / b, a.y / b);
		}

		public static Vector2 operator * (Vector2 a, Vector2 b)
		{
			return new Vector2(a.x * b.x, a.y * b.y);
		}
		public static Vector2 operator / (Vector2 a, Vector2 b)
		{
			return new Vector2(b.x == 0f ? 1 : (a.x / b.x), b.y == 0f ? 0f : (a.y / b.y));
		}

		public static Vector2 operator - (Vector2 v)
		{
			return new Vector2 (-v.x, -v.y);
		}

		public static implicit operator Vector3 (Vector2 v)
		{
			return new Vector3(v.x, v.y, 0f);
		}
		public static explicit operator Vector2 (Vector3 v)
		{
			return new Vector2(v.x, v.y);
		}

		public static Vector2 Min(Vector2 a, Vector2 b)
		{
			return new Vector2(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y));
		}
		public static Vector2 Max(Vector2 a, Vector2 b)
		{
			return new Vector2(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y));
		}

		public static Vector2 ProjectOnPoint(Vector2 vector, Vector2 a, Vector2 b)
		{
			Vector2 p = vector;
			var ap = p - a;
			var ab = b - a;

			var dot2 = Dot(ab, ab);
			var t = dot2 == 0 ? 0f : Dot(ap, ab) / dot2;
			var result = a + ab * t;
			return result;
		}

		public static void Clamp(ref Vector2 v, Vector2 range)
		{
			if (v.x > range.x) v.x = range.x;
			if (v.y > range.y) v.y = range.y;
			if (v.x < 0) v.x = 0;
			if (v.y < 0) v.y = 0;
		}
		public static void Clamp(ref Vector2 v, Vector2 min, Vector2 max)
		{
			v.x = Mathf.Clamp(v.x, min.x, max.x);
			v.y = Mathf.Clamp(v.y, min.y, max.y);
		}

		public Vector2 Abs()
		{
			Vector2 v = this;
			v.x = v.x.Abs();
			v.y = v.y.Abs();
			return v;
		}

		public float ToAngle()
		{
			if (x == 0f && y == 0f) return 0f;
			var value = normalized;
			return Mathf.ACos(value.x) * (value.y > 0 ? 1f : -1f);
		}

		public static float Dot (Vector2 a, Vector2 b)
		{
			return a.x * b.x + a.y * b.y;
		}

		public static float Angle (Vector2 a, Vector2 b)
		{
			a = a.normalized;
			b = b.normalized;
			return Dot(a, b).ACos();
		}

		public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
		{
			return a + (b - a) * t.Clamp(0f, 1f);
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2 vector && vector == this;
		}

		public override int GetHashCode()
		{
			int hashCode = 1502939027;
			hashCode = hashCode * -1521134295 + x.GetHashCode();
			hashCode = hashCode * -1521134295 + y.GetHashCode();
			return hashCode;
		}

		public float length
		{
			get
			{
				return (x * x + y * y).Sqrt();
			}
		}

		public Vector2 normalized
		{
			get
			{
				if (length == 0)
				{
					return Vector2.zero;
				}
				return this / length;
			}
		}

		public static readonly Vector2 right = new Vector2 (1, 0);
		public static readonly Vector2 zero = new Vector2 (0, 0);
		public static readonly Vector2 one = new Vector2 (1, 1);
		public static readonly Vector2 left = new Vector2 (-1, 0);
		public static readonly Vector2 up = new Vector2 (0, 1);
		public static readonly Vector2 down = new Vector2 (0, -1);
	}
}