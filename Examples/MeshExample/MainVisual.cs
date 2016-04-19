using Framework;
using OpenTK.Graphics.OpenGL;

namespace Example
{
	public class MainVisual
	{
		public MainVisual()
		{
			const string path = "../../";
			const string pathMedia = path + "media/";
			shader = ShaderLoader.FromFiles(path + "vertex.glsl", path + "fragment.glsl");
			
				//load geometry
			Mesh mesh = Obj2Mesh.FromObj(pathMedia + "suzanne.obj");
			geometry.SetAttribute(shader.GetAttributeLocation("position"), mesh.positions.ToArray(), VertexAttribPointerType.Float, 3);
			geometry.SetAttribute(shader.GetAttributeLocation("normal"), mesh.normals.ToArray(), VertexAttribPointerType.Float, 3);
			//geometry.SetAttribute(shader.GetAttributeLocation("uv"), mesh.uvs.ToArray(), VertexAttribPointerType.Float, 2);
			geometry.SetID(mesh.ids.ToArray(), PrimitiveType.Triangles);

			GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line); //draw wireframe

			//texDiffuse = TextureLoader.FromFile(pathMedia + "capsule0.jpg");
		}

		public void Render()
		{
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			shader.Begin();
			//texDiffuse.BeginUse();
			geometry.Draw();
			//texDiffuse.EndUse();
			shader.End();
		}

		private Shader shader;
		private VAO geometry = new VAO();
		//private Texture texDiffuse;
	}
}
