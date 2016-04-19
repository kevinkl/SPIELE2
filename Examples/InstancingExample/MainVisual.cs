using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

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
			geometry = CreateMesh(locPos, locNormal, locInstPos, locInstSpeed);

			CreatePerInstanceAttributes(geometry, locInstPos, locInstSpeed);

			GL.Enable(EnableCap.DepthTest);
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			geometry.Draw(particelCount);
			shader.End();
		}

		private const int particelCount = 10000;
		private Shader shader;
		private VAO geometry;

		private static VAO CreateMesh(int locPos, int locNormal, int locInstPos, int locInstSpeed)
		{
			Mesh mesh = Meshes.CreateSphere(0.03f, 2);
			var vao = new VAO();

			vao.SetAttribute(locPos, mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(locNormal, mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}

		private static void CreatePerInstanceAttributes(VAO vao, int locInstPos, int locInstSpeed)
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
			vao.SetAttribute(locInstPos, instancePositions, VertexAttribPointerType.Float, 3, true);

			//todo: add per instance attribute speed
		}

		private void LoadShader()
		{
			shader = ShaderLoader.FromFiles(@"..\..\vertex.glsl", @"..\..\fragment.glsl");
		}
	}
}
