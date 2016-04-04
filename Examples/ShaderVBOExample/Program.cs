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
			public Vector2 position;
			public Vector2 velocity;

			public VertexFormat(Vector2 position, Vector2 velocity)
			{
				this.position = position;
				this.velocity = velocity;
			}

			public static readonly int size = Marshal.SizeOf(typeof(VertexFormat));
			public static readonly int startVelocity = Marshal.SizeOf(typeof(Vector2));
		};

		private GameWindow gameWindow = new GameWindow();
		private const int particelCount = 1000;
		private Shader shader;
		private TimeSource timeSource = new TimeSource(50.0f);

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
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += game_RenderFrame;
			LoadShader();
			InitVBOs();
			timeSource.IsLooping = true;
			timeSource.IsRunning = true;
		}

		private void InitVBOs()
		{
			var rnd = new Random(12);
			Func<float> RndCoord = () => (float)((rnd.NextDouble() - 0.5) * 2.0);
			Func<float> RndSpeed = () => (float)((rnd.NextDouble() - 0.5) * 0.1);
			var vertices = new List<VertexFormat>();
			for (int i = 0; i < particelCount; ++i)
			{
				vertices.Add(new VertexFormat(
					new Vector2(RndCoord(), RndCoord()),
					new Vector2(RndSpeed(), RndSpeed())
					));
				Console.WriteLine(vertices[i].velocity);
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
				layout(location = 0) in vec2 in_position;  //set the frist input on location (index) 0 ; in_position is our attribute 
				layout(location = 1) in vec2 in_speed;
				uniform float time;
				void main() {
					gl_Position = vec4(in_position + time * in_speed, 0.0, 1.0);//w is 1.0, also notice cast to a vec4
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
			GL.Uniform1(shader.GetUniformLocation("time"), timeSource.Position);
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, VertexFormat.size, 0);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, VertexFormat.size, VertexFormat.startVelocity);
			GL.DrawArrays(PrimitiveType.Points, 0, particelCount);
			shader.End();
			GL.DisableVertexAttribArray(1);
			GL.DisableVertexAttribArray(0);
			gameWindow.SwapBuffers();
		}
	}
}