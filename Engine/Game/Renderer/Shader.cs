namespace Engine.Game
{
	public sealed class Shader
	{
		private Shader()
		{

		}

		public string shaderName { get; private set; }

		public static Shader LoadShader(string name)
		{
			Shader shader = new Shader();
			shader.shaderName = name;
			return shader;
		}
	}
}
