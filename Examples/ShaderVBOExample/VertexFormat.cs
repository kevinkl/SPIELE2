using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Example
{
	struct VertexFormat
	{
		public Vector2 position;
		public Vector2 velocity;

		public VertexFormat(Vector2 position, Vector2 velocity)
		{
			this.position = position;
			this.velocity = velocity;
		}

		public static readonly int size = Marshal.SizeOf(typeof(VertexFormat));

		public static void Activate()
		{
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, VertexFormat.size, 0);
			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, VertexFormat.size, VertexFormat.startVelocity);
		}

		public static void Deactive()
		{
			GL.DisableVertexAttribArray(1);
			GL.DisableVertexAttribArray(0);
		}

		private static readonly int startVelocity = Marshal.SizeOf(typeof(Vector2));
	};
}
