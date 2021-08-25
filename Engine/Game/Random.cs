namespace Engine.Game
{
	public static class Random
	{
		static System.Random random = new System.Random();

		public static float Range(float min, float max)
		{
			return (float)(min + random.NextDouble() * (max - min));
		}
		public static int Range(int min, int max)
		{
			return random.Next(min, max);
		}
	}
}
