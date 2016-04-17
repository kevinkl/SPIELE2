using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Diagnostics;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			LoadShader();
			var locPos = shader.GetAttributeLocation("position");
			var locNormal = shader.GetAttributeLocation("normal");
			var locInstPos = shader.GetAttributeLocation("instancePosition");
			var locInstSpeed = shader.GetAttributeLocation("instanceSpeed");
			geometry = InitVA(locPos, locNormal, locInstPos, locInstSpeed);
			GL.Enable(EnableCap.DepthTest);
			//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
			timeSource.Start();
		}

		public void Render()
		{
			var time = (float)timeSource.Elapsed.TotalSeconds;
			mvp = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.0f, 0.1f, 10.0f);
			var rotY = Matrix4.CreateRotationY(time * 0.5f);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			GL.Uniform3(shader.GetUniformLocation("light"), Vector3.Transform(light, rotY));
			GL.UniformMatrix4(shader.GetUniformLocation("mvp"), false, ref mvp);
			geometry.Draw(particelCount);
			shader.End();
		}

		private const int particelCount = 10000;
		private Vector3 light = new Vector3(0.0f, 0.0f, -1.0f);
		private Shader shader;
		private Stopwatch timeSource = new Stopwatch();
		private Matrix4 mvp = Matrix4.Identity;
		private VAO geometry;

		private static VAO InitVA(int locPos, int locNormal, int locInstPos, int locInstSpeed)
		{
			Mesh mesh = Meshes.CreateSphere(0.03f, 2);
			var vao = new VAO();
			
			vao.SetAttribute(locPos, mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(locNormal, mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
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
