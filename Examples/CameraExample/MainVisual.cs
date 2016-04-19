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
			shader = ShaderLoader.FromFiles(@"..\..\vertex.glsl", @"..\..\fragment.glsl");

			geometry = CreateMesh(shader);

			CreatePerInstanceAttributes(geometry, shader);

			GL.Enable(EnableCap.DepthTest);
			timeSource.Start();
		}

		public void Render()
		{
			var time = (float)timeSource.Elapsed.TotalSeconds;
			var mtxProjection = Matrix4.CreatePerspectiveFieldOfView(0.5f, 1.0f, 0.1f, 100.0f);
			var mtxView = Matrix4.CreateTranslation(0, 0, -10.7f);
			mtxView.Transpose();
			mvp = mtxProjection * mtxView;
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			GL.Uniform1(shader.GetUniformLocation("time"), time);
			GL.UniformMatrix4(shader.GetUniformLocation("mvp"), false, ref mvp);
			geometry.Draw(particelCount);
			shader.End();
		}

		private const int particelCount = 10000;
		private Shader shader;
		private Stopwatch timeSource = new Stopwatch();
		private Matrix4 mvp = Matrix4.Identity;
		private VAO geometry;

		private static VAO CreateMesh(Shader shader)
		{
			Mesh mesh = Meshes.CreateSphere(0.03f, 2);
			var vao = new VAO();
			vao.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}

		private static void CreatePerInstanceAttributes(VAO vao, Shader shader)
		{
			//per instance attributes
			var rnd = new Random(12);
			Func<float> Rnd01 = () => (float)rnd.NextDouble();
			Func<float> RndCoord = () => (Rnd01() - 0.5f) * 2.0f;
			var instancePositions = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instancePositions[i] = new Vector3(RndCoord(), RndCoord(), RndCoord());
			}
			vao.SetAttribute(shader.GetAttributeLocation("instancePosition"), instancePositions, VertexAttribPointerType.Float, 3, true);

			Func<float> RndSpeed = () => (Rnd01() - 0.5f) * 0.1f;
			var instanceSpeeds = new Vector3[particelCount];
			for (int i = 0; i < particelCount; ++i)
			{
				instanceSpeeds[i] = new Vector3(RndSpeed(), RndSpeed(), RndSpeed());
			}
			vao.SetAttribute(shader.GetAttributeLocation("instanceSpeed"), instanceSpeeds, VertexAttribPointerType.Float, 3, true);
		}
	}
}
