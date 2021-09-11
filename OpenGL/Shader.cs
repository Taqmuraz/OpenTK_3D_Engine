using Engine;

namespace WinGL
{
	public sealed class Shader : ShaderProgram
	{
		public static readonly string ShadersPath = "./Data/Shaders/";

		private Shader(string vertexFile, string fragmentFile) : base(vertexFile, fragmentFile)
		{
		}

		public void LoadTransformationMatrix(Matrix4x4 matrix)
		{
			SetMatrix("transformationMatrix", matrix);
		}
		public void LoadProjectionMatrix(Matrix4x4 projection)
		{
			SetMatrix("projectionMatrix", projection);
		}

		public static Shader defaultShader { get; set; } = new Shader(Shader.ShadersPath + "vertexShader.glsl", Shader.ShadersPath + "fragmentShader.glsl");
		static SafeDictionary<string, Shader> shaders = new SafeDictionary<string, Shader>(() => defaultShader);

		public static void LoadShader(string name, string vertexFile, string fragmentFile)
		{
			if (shaders.ContainsKey(name)) return;
			else shaders.Add(name, new Shader(vertexFile, fragmentFile));
		}

		public static Shader GetShader(string name)
		{
			return shaders[name];
		}

		public static void CleanShaders()
		{
			foreach (var shader in shaders)
			{
				shader.Value.CleanUp();
			}
		}

		public void LoadViewMatrix(Matrix4x4 matrix)
		{
			//matrix.column_3 = -matrix.column_3;
			SetMatrix("viewMatrix", matrix);
		}
		public void LoadLight(Engine.Game.Light light)
		{
			SetFloat("ambienceIntencivity", light.lightBrightness);
			SetVector3("lightPosition", light.transform.position);
			SetVector4("lightColor", light.lightColor.ToVector4());
		}
		public void LoadTextureParams(Engine.Game.Material material)
		{
			Vector2 tiling = material.tiling;
			Vector2 offset = material.offset;
			SetVector4("textureVector", new Vector4(tiling.x, tiling.y, offset.x, offset.y));
		}
	}
}