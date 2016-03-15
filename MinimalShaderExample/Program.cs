using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Example
{
	class MyApplication
	{
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

		}

		private void LoadShader()
		{
			string sVertexShader = @"
				#version 130				
				varying vec2 uv;
				void main() {
					gl_Position = gl_Vertex;
					uv = gl_Vertex.xy * 0.5f + 0.5f;
				}";
			string sFragmentShd = @"
			varying vec2 uv;
			void main() {
				vec3 color = vec3(uv, 1.0);
				gl_FragColor = vec4(color, 1.0);
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
			GL.Begin(PrimitiveType.Quads);
			GL.Color3(Color.White);
			GL.Vertex2(-1.0, -1.0);
			GL.Vertex2(1.0, -1.0);
			GL.Vertex2(1.0, 1.0);
			GL.Vertex2(-1.0, 1.0);
			GL.End();
			shader.End();
			gameWindow.SwapBuffers();
		}
	}
}