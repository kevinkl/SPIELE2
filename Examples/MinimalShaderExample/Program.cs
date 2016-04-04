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
				#version 430 core				
				out vec3 pos; 
				void main() {
					const vec3 vertices[4] = vec3[4](vec3(-0.9, -0.8, 0.5),
                                    vec3( 0.9, -0.9, 0.5),
                                    vec3( 0.9,  0.8, 0.5),
                                    vec3(-0.9,  0.9, 0.5));
					pos = vertices[gl_VertexID];
					gl_Position = vec4(pos, 1.0);
				}";
			string sFragmentShd = @"
			#version 430 core
			in vec3 pos;
			out vec4 color;
			void main() {
				color = vec4(pos + 1.0, 1.0);
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
			GL.DrawArrays(PrimitiveType.Quads, 0, 4);
			shader.End();
			gameWindow.SwapBuffers();
		}
	}
}