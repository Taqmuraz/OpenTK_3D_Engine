using Engine.Game;
using System;

namespace Engine
{
	public struct Rect
	{
		public Vector2 min;
		public Vector2 max;

		public Rect(float x, float y, float width, float height)
		{
			min = new Vector2(x, y);
			max = new Vector2(x + width, y + height);
		}

		public Rect(Vector2 min, Vector2 max)
		{
			this.min = min;
			this.max = max;
		}

		public static Rect FromCenterAndSize(Vector2 center, Vector2 size)
		{
			return new Rect(center - size * 0.5f, center + size * 0.5f);
		}

		public override string ToString()
		{
			return $"({min.x}, {min.y}, {max.x - min.x}, {max.y - min.y})";
		}

		public Vector2 size => max - min;
		public Vector2 center => (min + max) * 0.5f;
		public Vector2 upperLeft => new Vector2(min.x, max.y);
		public Vector2 downRight => new Vector2(max.x, min.y);
		public Vector2 bottom => new Vector2((min.x + max.x) * 0.5f, min.y);
		public Vector2 top => new Vector2((min.x + max.x) * 0.5f, max.y);
		public Vector2 right => new Vector2(max.x, (min.y + max.y) * 0.5f);
		public Vector2 left => new Vector2(min.x, (min.y + max.y) * 0.5f);

		public bool IntersectsWith(Rect rect)
		{
			Rect r = this;
			Vector2.Clamp(ref r.min, rect.min, rect.max);
			Vector2.Clamp(ref r.max, rect.min, rect.max);
			Vector2 area = r.size;
			return area.x * area.y != 0f;
		}

		public Rect Spread(Rect rect)
		{
			Vector2 nMin = new Vector2();
			Vector2 nMax = new Vector2();
			nMin.x = Mathf.Min(min.x, rect.min.x);
			nMin.y = Mathf.Min(min.y, rect.min.y);
			nMax.x = Mathf.Max(max.x, rect.max.x);
			nMax.y = Mathf.Max(max.y, rect.max.y);
			return new Rect(nMin, nMax);
		}
		public Rect Spread(Vector2 point)
		{
			return new Rect(Vector2.Min(min, point), Vector2.Max(max, point));
		}

		public static implicit operator System.Drawing.RectangleF(Rect rect)
		{
			return new System.Drawing.RectangleF(rect.min.x, rect.min.y, rect.size.x, rect.size.y);
		}
		public static explicit operator System.Drawing.Rectangle(Rect rect)
		{
			return new System.Drawing.Rectangle((int)rect.min.x, (int)rect.min.y, (int)rect.size.x, (int)rect.size.y);
		}
		public static implicit operator Rect(System.Drawing.RectangleF rectangle)
		{
			return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
		}

		public Rect MultiplyMatrix(Matrix4x4 matrix)
		{
			return FromCenterAndSize((Vector2)matrix.MultiplyPoint(center), (Vector2)matrix.MultiplyVector(size).Abs());
		}
		public Rect MultiplyMatrixSize(Matrix4x4 matrix)
		{
			return FromCenterAndSize(center, (Vector2)matrix.MultiplyVector(size).Abs());
		}

		public Matrix4x4 ToMatrixUV()
		{
			return Matrix4x4.CreateWorldMatrix(new Vector3(max.x - min.x, 0f, 0f), new Vector3(0f, max.y - min.y, 0f), Vector3.forward, min);
		}
		public Matrix4x4 ToMatrixTransform()
		{
			return Matrix4x4.CreateWorldMatrix(new Vector3(max.x - min.x, 0f, 0f), new Vector3(0f, max.y - min.y, 0f), Vector3.forward, center);
		}

		public bool Contains(Vector2 point)
		{
			return point.x <= max.x && point.x >= min.x && point.y <= max.y && point.y >= min.y;
		}
	}
}