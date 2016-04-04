using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			LoadShader();
			InitVBOs();
			timeSource.IsLooping = true;
			timeSource.IsRunning = true;
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.PointSize(1.0f);
			shader.Begin();
			GL.Uniform1(shader.GetUniformLocation("time"), timeSource.Position);
			VertexFormat.Activate();
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			VertexFormat.Deactive();
			shader.End();
			GL.DisableVertexAttribArray(1);
			GL.DisableVertexAttribArray(0);
		}

		private const int particelCount = 100000;
		private Shader shader;
		private TimeSource timeSource = new TimeSource(50.0f);

		private void InitVBOs()
		{
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.1f;
			var vertices = new List<VertexFormat>();
			for (int i = 0; i < particelCount; ++i)
			{
				vertices.Add(new VertexFormat(
					new Vector2(RndCoord(), RndCoord()),
					new Vector2(RndSpeed(), RndSpeed())
					));
			}
			uint vbo; //our vbo handler
			GL.GenBuffers(1, out vbo);
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);//bind to context
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexFormat.size * vertices.Count), vertices.ToArray(), BufferUsageHint.StaticDraw);
		}

		private void LoadShader()
		{
			string sVertexShader = @"
				#version 430 core				
				layout(location = 0) in vec2 in_position;  //set the first input on location (index) 0 ; in_position is our attribute 
				layout(location = 1) in vec2 in_speed;
				uniform float time;
				void main() {
					gl_Position = vec4(in_position + time * in_speed, 0.0, 1.0);//w is 1.0, also notice cast to a vec4
				}";
			string sFragmentShd = @"
			#version 430 core
			out vec4 color;
			void main() {
				color = vec4(0.0, 0.0, 1.0, 1.0);
			}";
			//read shader from file
			//string fileName = @"..\..\..\GLSL pixel shader\Hello world.glsl";
			//try
			//{
			//	using (StreamReader sr = new StreamReader(fileName))
			//	{
			//		sFragmentShd = sr.ReadToEnd();
			//		sr.Dispose();
			//	}
			//}
			//catch { };
			shader = ShaderLoader.FromStrings(sVertexShader, sFragmentShd);
		}
	}
}
