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
			var locPos = shader.GetAttributeLocation("position");
			var locInstPos = shader.GetAttributeLocation("instancePosition");
			var locInstSpeed = shader.GetAttributeLocation("instanceSpeed");
			geometryInstance = InitVA(locPos, locInstPos, locInstSpeed);
			GL.Enable(EnableCap.DepthTest);
			//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			timeSource.IsLooping = true;
			timeSource.IsRunning = true;
		}

		public void Render()
		{
			//mvp = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.0f, 0.1f, 10.0f);
			//mvp = Matrix4.CreateRotationX(timeSource.Position * 0.7f) * Matrix4.CreateRotationY(timeSource.Position) * Matrix4.CreateRotationZ(timeSource.Position * 0.3f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			GL.Uniform1(shader.GetUniformLocation("time"), timeSource.Position);
			GL.UniformMatrix4(shader.GetUniformLocation("mvp"), false, ref mvp);
			geometryInstance.Draw(particelCount);
			shader.End();
		}

		private const int particelCount = 1000;
		private Shader shader;
		private TimeSource timeSource = new TimeSource(50.0f);
		private Matrix4 mvp = Matrix4.Identity;
		private VAO geometryInstance;

		private static VAO InitVA(int locPos, int locInstPos, int locInstSpeed)
		{
			Mesh mesh = BasicMeshes.CreateSphere(0.03f, 2);
			var vao = new VAO();
			
			vao.SetAttribute(locPos, mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);

			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			vao.SetAttribute(locInstPos, instancePositions, VertexAttribPointerType.Float, 3, true);

			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.1f;
			var instanceSpeeds = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instanceSpeeds[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
			}
			vao.SetAttribute(locInstSpeed, instanceSpeeds, VertexAttribPointerType.Float, 3, true);
			return vao;
		}

		private void LoadShader()
		{
			//try
			//{
				shader = ShaderLoader.FromFiles(@"..\..\vertex.glsl", @"..\..\fragment.glsl");
			//}
			//catch (Exception e)
			{
				//todo: show a shader error form
			}
		}
	}
}
