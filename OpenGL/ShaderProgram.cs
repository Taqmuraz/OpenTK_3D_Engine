using OpenTK.Graphics.OpenGL;
using Engine;

namespace WinGL
{
	public sealed class Shader : ShaderProgram
	{
		public static readonly string ShadersPath = "./Data/Shaders/";

		public Shader(string vertexFile, string fragmentFile) : base(vertexFile, fragmentFile)
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
		public void LoadViewMatrix(Matrix4x4 matrix)
		{
			matrix.column_3 = -matrix.column_3;
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

	public abstract class ShaderProgram
	{
		private int programID;
		private int vertexShaderID;
		private int fragmentShaderID;

		public const int MATRIX_SIZE = 16;

		private static float[] matrixBuffer = new float[MATRIX_SIZE];

		public ShaderProgram(string vertexFile, string fragmentFile)
		{
			vertexShaderID = LoadShader(vertexFile, ShaderType.VertexShader);
			fragmentShaderID = LoadShader(fragmentFile, ShaderType.FragmentShader);
			programID = GL.CreateProgram();
			GL.AttachShader(programID, vertexShaderID);
			GL.AttachShader(programID, fragmentShaderID);
			BindAttributes();
			GL.LinkProgram(programID);
			GL.ValidateProgram(programID);

			//System.out.println(fragmentFile());
		}

		public void Start()
		{
			GL.UseProgram(programID);
		}
		public void Stop()
		{
			GL.UseProgram(0);
		}
		public void cleanUp()
		{
			Stop();
			GL.DetachShader(programID, vertexShaderID);
			GL.DetachShader(programID, fragmentShaderID);
			GL.DeleteShader(vertexShaderID);
			GL.DeleteShader(fragmentShaderID);
			GL.DeleteProgram(programID);
		}

		SafeDictionary<string, int> uniforms = new SafeDictionary<string, int>();

		public int GetUniformLocation(string uniformName)
		{
			if (string.IsNullOrEmpty(uniformName)) throw new System.ArgumentException("uniformName is null or empty");
			else if (uniforms.ContainsKey(uniformName)) return uniforms[uniformName];
			else
			{
				int location = GL.GetUniformLocation(programID, uniformName);
				uniforms.Add(uniformName, location);
				return location;
			}
		}

		protected virtual void BindAttributes()
		{
			BindAttribute(0, "position");
			BindAttribute(1, "textureCoords");
			BindAttribute(2, "normal");
		}

		protected void BindAttribute(int attribute, string variableName)
		{
			GL.BindAttribLocation(programID, attribute, variableName);
		}

		protected void LoadFloat(int location, float value)
		{
			GL.Uniform1(location, value);
		}

		protected void LoadVector3(int location, Vector3 vector)
		{
			GL.Uniform3(location, vector.x, vector.y, vector.z);
		}
		protected void LoadVector4(int location, Vector4 vector)
		{
			GL.Uniform4(location, vector.x, vector.y, vector.z, vector.w);
		}

		protected void LoadVector2(int location, Vector2 vector)
		{
			GL.Uniform2(location, vector.x, vector.y);
		}

		protected void LoadBoolean(int location, bool value)
		{
			float toLoad = value ? 1f : 0f;
			GL.Uniform1(location, toLoad);
		}

		protected void LoadMatrix(int location, Matrix4x4 matrix)
		{
			for (int i = 0; i < MATRIX_SIZE; i++) matrixBuffer[i] = matrix[i / 4, i % 4];
			GL.UniformMatrix4(location, 1, false, matrixBuffer);
		}

		public void SetFloat(string name, float value)
		{
			LoadFloat(GetUniformLocation(name), value);
		}
		public void SetVector2(string name, Vector2 vector)
		{
			LoadVector2(GetUniformLocation(name), vector);
		}
		public void SetVector3(string name, Vector3 vector)
		{
			LoadVector3(GetUniformLocation(name), vector);
		}
		public void SetVector4(string name, Vector4 vector)
		{
			LoadVector4(GetUniformLocation(name), vector);
		}
		public void SetBool(string name, bool value)
		{
			LoadBoolean(GetUniformLocation(name), value);
		}
		public void SetMatrix(string name, Matrix4x4 matrix)
		{
			LoadMatrix(GetUniformLocation(name), matrix);
		}

		private static int LoadShader(string file, ShaderType type)
		{
			System.Text.StringBuilder shaderSource = new System.Text.StringBuilder();

			using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
			{
				string line;
				while (!string.IsNullOrEmpty(line = reader.ReadLine()))
				{
					shaderSource.Append(line).Append("//\n");
				}
			}

			int shaderID = GL.CreateShader(type);
			GL.ShaderSource(shaderID, shaderSource.ToString());
			GL.CompileShader(shaderID);
			GL.GetShader(shaderID, ShaderParameter.CompileStatus, out int p);
			if (p != 0)
			{
				GL.GetShaderInfoLog(shaderID, 500, out int l, out string info);
				throw new System.Exception($"Shader compile error : {info}, message length = {l}, file = {file}");
			}
			return shaderID;
		}
	}
}