using Framework;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			LoadShader();
			var locPos = shader.GetAttributeLocation("position");
			var locNormal = shader.GetAttributeLocation("normal");
			geometry = InitVA(locPos, locNormal);
			GL.Enable(EnableCap.DepthTest);
			//GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //draw wireframe
			timeSource.IsLooping = true;
			timeSource.IsRunning = true;
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			GL.Uniform3(shader.GetUniformLocation("lightPosition"), lightPosition);
			geometry.Draw();
			shader.End();
		}

		private Vector3 lightPosition = new Vector3(1.0f, 1.0f, -1.0f);
		private Shader shader;
		private TimeSource timeSource = new TimeSource(50.0f);
		private VAO geometry;

		private static VAO InitVA(int locPos, int locNormal)
		{
			Mesh mesh = BasicMeshes.CreateSphere(0.9f, 3);
			var vao = new VAO();
			vao.SetAttribute(locPos, mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetAttribute(locNormal, mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			vao.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);
			return vao;
		}

		private void LoadShader()
		{
			shader = ShaderLoader.FromFiles(@"..\..\vertex.glsl", @"..\..\fragment.glsl");
		}
	}
}
