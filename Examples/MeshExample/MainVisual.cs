using Framework;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			//load geometry
			//Mesh mesh = Meshes.CreateSphere(0.9f, 3);
			Mesh mesh = Obj2Mesh.FromObj(@"..\..\media\suzanne.obj");
			var locPos = shader.GetAttributeLocation("position");
			var locNormal = shader.GetAttributeLocation("normal");
			geometry.SetAttribute(locPos, mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			geometry.SetAttribute(locNormal, mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			geometry.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //draw wireframe
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			geometry.Draw();
			shader.End();
		}

		private Shader shader = ShaderLoader.FromFiles(@"..\..\vertex.glsl", @"..\..\fragment.glsl");
		private VAO geometry = new VAO();
	}
}
