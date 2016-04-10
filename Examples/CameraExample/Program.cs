using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;

namespace Example
{
	class MyApplication
	{
		private GameWindow gameWindow;
		private MainVisual visual;

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
			var mode = new GraphicsMode(new ColorFormat(32), 24, 8, 0);
			gameWindow = new GameWindow(1024, 1024, mode, "Example", GameWindowFlags.Default, DisplayDevice.Default, 4, 3, GraphicsContextFlags.ForwardCompatible);

			gameWindow.WindowState = WindowState.Fullscreen;
			gameWindow.KeyDown += GameWindow_KeyDown;
			gameWindow.Resize += (s, arg) => GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
			gameWindow.RenderFrame += (s, arg) => visual.Render();			
			gameWindow.RenderFrame += (s, arg) => gameWindow.SwapBuffers();
			visual = new MainVisual();
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