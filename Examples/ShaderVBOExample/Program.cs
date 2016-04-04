using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
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

			public VertexFormat(Vector3 pos)
			{
				position = pos;
			}

			//public static readonly uint size = sizeof(float) * 3;
			public static readonly int size = Marshal.SizeOf(typeof(VertexFormat));
		};

		private GameWindow gameWindow = new GameWindow();
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
			gameWindow.RenderFrame += game_RenderFrame;
			LoadShader();
			InitVBOs();
		}

		private void InitVBOs()
		{
			var vertices = new List<VertexFormat>();
			vertices.Add(new VertexFormat(new Vector3(0.0f, 0.0f, 0.0f)));
			vertices.Add(new VertexFormat(new Vector3(1.0f, 0.0f, 0.0f)));
			vertices.Add(new VertexFormat(new Vector3(0.9f, 1.0f, 0.0f)));
			vertices.Add(new VertexFormat(new Vector3(0.0f, 1.0f, 0.0f)));

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
			shader.Begin();
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, VertexFormat.size, 0);
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shader.End();
			gameWindow.SwapBuffers();
		}
	}
}