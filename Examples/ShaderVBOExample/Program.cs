using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow = new GameWindow();
		private MainVisual visual = new MainVisual();

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
			gameWindow.RenderFrame += (s, arg) => visual.Render();
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
		}
	}
}