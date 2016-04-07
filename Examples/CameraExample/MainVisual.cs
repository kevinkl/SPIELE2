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
			GL.UniformMatrix4(shader.GetUniformLocation("MVP"), false, ref mvp);
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
		private Matrix4 mvp = Matrix4.Identity;

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
					new Vector3(RndCoord(), RndCoord(), RndCoord()),
					new Vector3(RndSpeed(), RndSpeed(), RndSpeed())
					));
			}
			uint bufferID; //our vbo handler
			GL.GenBuffers(1, out bufferID);
			GL.BindBuffer(BufferTarget.ArrayBuffer, bufferID);//bind to context
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(VertexFormat.size * vertices.Count), vertices.ToArray(), BufferUsageHint.StaticDraw);
		}

		private void LoadShader()
		{
			shader = ShaderLoader.FromFiles(@"..\..\vertex.glsl", @"..\..\fragment.glsl");
		}
	}
}
