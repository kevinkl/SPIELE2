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
			gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += (s, arg) => visual.Render();			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
		}

		private void GameWindow_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape: gameWindow.Close(); break;
			}
		}
	}
}