using OpenTK.Graphics.OpenGL;
using Engine;

namespace WinGL
{
	public abstract class ShaderProgram
	{
		private int programID;
		private int vertexShaderID;
		private int fragmentShaderID;

		public const int MATRIX_SIZE = 16;

		private static float[] matrixBuffer = new float[MATRIX_SIZE];

		public ShaderProgram()
		{
			vertexShaderID = LoadShader(VertexFile(), ShaderType.VertexShader);
			fragmentShaderID = LoadShader(FragmentFile(), ShaderType.FragmentShader);
			programID = GL.CreateProgram();
			GL.AttachShader(programID, vertexShaderID);
			GL.AttachShader(programID, fragmentShaderID);
			BindAttributes();
			GL.LinkProgram(programID);
			GL.ValidateProgram(programID);
			GetAllUniformLocations();

			//System.out.println(fragmentFile());
		}

		protected abstract string VertexFile();
		protected abstract string FragmentFile();

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

		protected abstract void GetAllUniformLocations();

		public int GetUniformLocation(string uniformName)
		{
			return GL.GetUniformLocation(programID, uniformName);
		}

		protected abstract void BindAttributes();

		protected void bindAttribute(int attribute, string variableName)
		{
			GL.BindAttribLocation(programID, attribute, variableName);
		}

		protected void loadFloat(int location, float value)
		{
			GL.Uniform1(location, value);
		}

		protected void loadVector(int location, Vector3 vector)
		{
			GL.Uniform3(location, vector.x, vector.y, vector.z);
		}

		protected void loadVector2(int location, Vector2 vector)
		{
			GL.Uniform2(location, vector.x, vector.y);
		}

		protected void loadBoolean(int location, bool value)
		{
			float toLoad = value ? 1f : 0f;
			GL.Uniform1(location, toLoad);
		}

		protected void loadMatrix(int location, Matrix4x4 matrix)
		{
			for (int i = 0; i < MATRIX_SIZE; i++) matrixBuffer[i] = matrix[i / 4, i % 4];
			GL.UniformMatrix4(location, 1, false, matrixBuffer);
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
			if (p == 0)
			{
				GL.GetShaderInfoLog(shaderID, 500, out int l, out string info);
				throw new System.Exception("Shader compile error : " + info);
			}
			return shaderID;
		}
	}
}