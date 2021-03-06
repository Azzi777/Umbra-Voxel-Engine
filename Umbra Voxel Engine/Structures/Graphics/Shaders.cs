﻿using System;
using Umbra.Definitions.Globals;
using Umbra.Implementations;
using OpenTK.Graphics.OpenGL;

namespace Umbra.Structures.Graphics
{
	static public class Shaders
	{
		static public Shader DefaultShaderProgram { get; private set; }

		static public void CompileShaders()
		{
			DefaultShaderProgram = new Shader(Content.Load<string>(Constants.Content.Shaders.Raytracer));

			GetVariables(DefaultShaderProgram.ProgramID);
		}

		static public int PositionID { get; private set; }
		static public int LookAtID { get; private set; }
		static public int ResolutionID { get; private set; }
		static public int TimeID { get; private set; }
		static public int OffsetID { get; private set; }
		static public int WorldSizeID { get; private set; }
		static public int ChunkSizeID { get; private set; }


		static private void GetVariables(int shaderProgram)
		{

			LookAtID = GL.GetUniformLocation(shaderProgram, "cam_lookat");
			PositionID = GL.GetUniformLocation(shaderProgram, "cam_pos");
			ResolutionID = GL.GetUniformLocation(shaderProgram, "resolution");
			TimeID = GL.GetUniformLocation(shaderProgram, "time");
			OffsetID = GL.GetUniformLocation(shaderProgram, "offset");
			WorldSizeID = GL.GetUniformLocation(shaderProgram, "world_size");
			ChunkSizeID = GL.GetUniformLocation(shaderProgram, "chunk_size");
		}
	}

	public class Shader
	{
		public int ProgramID { get; private set; }

		public Shader(string fragmentShader)
		{
			CompileShader(fragmentShader);
			GL.UseProgram(ProgramID);
		}

		public void CompileShader(string fragmentShader)
		{
			int fragmentShaderID;

			ProgramID = GL.CreateProgram();
			fragmentShaderID = GL.CreateShader(ShaderType.FragmentShader);

			GL.ShaderSource(fragmentShaderID, fragmentShader);

			GL.CompileShader(fragmentShaderID);


			int compileResult;
			GL.GetShader(fragmentShaderID, ShaderParameter.CompileStatus, out compileResult);
			if (compileResult != 1)
			{
				string error = GL.GetShaderInfoLog(fragmentShaderID);

				throw new Exception("Error while compiling the fragment shader. This means that you probably have an outdated graphics card driver. \n\nError message: \"" + error + "\"");
			}


			GL.AttachShader(ProgramID, fragmentShaderID);

			GL.LinkProgram(ProgramID);

			string info = GL.GetProgramInfoLog(ProgramID);
			System.Console.WriteLine(info);

			if (fragmentShaderID != 0)
			{
				GL.DeleteShader(fragmentShaderID);
			}
		}
	}
}
