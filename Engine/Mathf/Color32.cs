using System;

namespace Engine
{
	public struct Color32
	{
		public byte rByte, gByte, bByte, aByte;

		const float D = 1f / 255f;

		public float r => rByte * D;
		public float g => gByte * D;
		public float b => bByte * D;
		public float a => aByte * D;

		public Color32(byte r, byte g, byte b, byte a)
		{
			this.rByte = r;
			this.gByte = g;
			this.bByte = b;
			this.aByte = a;
		}
		public Color32(float r, float g, float b, float a)
		{
			this.rByte = (byte)(r * 255f);
			this.gByte = (byte)(g * 255f);
			this.bByte = (byte)(b * 255f);
			this.aByte = (byte)(a * 255f);
		}

		public static readonly Color32 red = new Color32 (255, 0, 0, 255);
		public static readonly Color32 green = new Color32 (0, 255, 0, 255);
		public static readonly Color32 blue = new Color32 (0, 0, 255, 255);
		public static readonly Color32 white = new Color32 (255, 255, 255, 255);

		public Vector4 ToVector4()
		{
			return new Vector4(r, g, b, a);
		}

		public static readonly Color32 black = new Color32 (0, 0, 0, 255);

		public static Color32 operator * (Color32 a, float b)
		{
			return new Color32(a.rByte * b, a.gByte * b, a.bByte * b, a.aByte);
		}
		public static Color32 operator *(Color32 a, Color32 b)
		{
			return new Color32(a.rByte * b.rByte, a.gByte * b.gByte, a.bByte * b.bByte, a.aByte * b.aByte);
		}
		public static Color32 operator +(Color32 a, Color32 b)
		{
			return new Color32(a.rByte + b.rByte, a.gByte + b.gByte, a.bByte + b.bByte, a.aByte + b.aByte);
		}

		public static implicit operator System.Drawing.Color (Color32 color)
		{
			return System.Drawing.Color.FromArgb (color.aByte, color.rByte, color.gByte, color.bByte);
		}
		public static implicit operator Color32(System.Drawing.Color color)
		{
			return new Color32(color.R, color.G, color.B, color.A);
		}
	}
}