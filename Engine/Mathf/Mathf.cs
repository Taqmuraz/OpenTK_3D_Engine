using System;

namespace Engine
{
	public static class Mathf
	{
		public const float PI = (float)Math.PI;
		public const float Deg2Rad = PI / 180f;
		public const float Rad2Deg = 180f / PI;
		public const float Epsilon = 1.401298E-45f;

		public static float SinRad(this float a)
		{
			return (float)Math.Sin(a);
		}
		public static float CosRad(this float a)
		{
			return (float)Math.Cos(a);
		}

		public static float Sin (this float a)
		{
			return (float)Math.Sin (a * Deg2Rad);
		}
		public static float Cos (this float a)
		{
			return (float)Math.Cos (a * Deg2Rad);
		}
		public static float Sqrt (this float a)
		{
			return (float)Math.Sqrt (a.Abs());
		}
		public static float ASin (this float a)
		{
			return (float)Math.Asin (a) * Rad2Deg;
		}
		public static float ACos (this float a)
		{
			return (float)Math.Acos (a) * Rad2Deg;
		}
		public static float Sign (this float a)
		{
			return Math.Sign (a);
		}
		public static float Lerp(this float a, float b, float t)
		{
			return a + (b - a) * t.Clamp(0f, 1f);
		}

		public static float Pow(this float a, float n)
		{
			return (float)Math.Pow(a, n);
		}

		public static float Tan(this float v)
		{
			var cos = Cos(v);
			return cos == 0 ? 0 : Sin(v) / Cos(v);
		}

		public static float Atan(this float v)
		{
			return (float)Math.Atan(v) * Rad2Deg;
		}

		public static int Round(this float i)
		{
			return (int)Math.Round(i);
		}

		public static void Swap<T>(this object obj, ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}
		public static void Swap<T>(ref T a, ref T b)
		{
			T temp = a;
			a = b;
			b = temp;
		}

		public static float Min (float a, float b)
		{
			if (a > b) return b;
			return a;
		}
		public static float Min(float a, float b, float c)
		{
			if (a <= b && a <= c) return a;
			if (b <= c && b <= a) return b;
			return c;
		}
		public static float Max(float a, float b)
		{
			if (a < b) return b;
			return a;
		}
		public static float Max(float a, float b, float c)
		{
			if (a >= b && a >= c) return a;
			if (b >= c && b >= a) return b;
			return c;
		}
		public static float Abs (this float a)
		{
			if (a < 0) return -a;
			return a;
		}
		public static float Determinant (float a1, float b1, float a2, float b2)
		{
			return a1 * b2 - a2 * b1;
		}
		public static float Determinant (Vector2 axis_a, Vector2 axis_b)
		{
			return Determinant(axis_a.x, axis_b.x, axis_a.y, axis_b.y);
		}
		public static int Clamp (this int a, int min, int max)
		{
			if (a > max) a = max;
			if (a < min) a = min;
			return a;
		}
		public static float Clamp(this float a, float min, float max)
		{
			if (a > max) a = max;
			if (a < min) a = min;
			return a;
		}
	}
}

