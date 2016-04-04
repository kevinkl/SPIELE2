using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Example
{
	class MyApplication
	{
		struct VertexFormat
		{
			Vector3 position;
			Vector3 velocity;

			public VertexFormat(Vector3 position, Vector3 velocity)
			{
				this.position = position;
				this.velocity = velocity;
			}

			public static readonly int size = Marshal.SizeOf(typeof(VertexFormat));
		};

		private GameWindow gameWindow = new GameWindow();
		private const int particelCount = 400;
		private Shader shader;

		[STAThread]
		public static void Main()
		{
			var app = new MyApplication();
			app.Run();
		}

		private void Run()
		{
			gameWindow.Run(60.0);
		}

		private MyApplication()
		{
			gameWindow.KeyDown += (s, arg) => { if (arg.Key == Key.Escape) gameWindow.Close(); };
			gameWindow.RenderFrame += game_RenderFrame;
			LoadShader();
			InitVBOs();
		}

		private void InitVBOs()
		{
			var rnd = new Random(12);
			Func<float> RndCoord = () => (float)((rnd.NextDouble() - 0.5) * 2.0);
			Func<float> RndSpeed = () => (float)((rnd.NextDouble() - 0.5) * 20.0);
			var vertices = new List<VertexFormat>();
			for (uint i = 0; i < particelCount; ++i)
			{
				vertices.Add(new VertexFormat(
					new Vector3(RndCoord(), RndCoord(), RndCoord()),
					new Vector3(RndSpeed(), RndSpeed(), RndSpeed())
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
				layout(location = 0) in vec3 in_position;  //set the frist input on location (index) 0 ; in_position is our attribute 
				//layout(location = 1) in vec4 in_speed;
				void main() {
					gl_Position = vec4(in_position, 1.0);//w is 1.0, also notice cast to a vec4
				}";
			string sFragmentShd = @"
			#version 430 core
			out vec4 color;
			void main() {
				color = vec4(0.2, 0.3, 1.0, 1.0);
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

		private void game_RenderFrame(object sender, FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.PointSize(5.0f);
			shader.Begin();
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, VertexFormat.size, 0);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, VertexFormat.size, 9);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			shader.End();
			GL.DisableVertexAttribArray(1);
			GL.DisableVertexAttribArray(0);
			gameWindow.SwapBuffers();
		}
	}
}