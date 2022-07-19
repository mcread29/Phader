using System.Numerics;
using Silk.NET.OpenGL;
using Phader.GlObjects;
using Phader.GameObjects;

namespace Phader.Rendering
{
    public class SpriteRenderer : IDisposable
    {
        private readonly float[] TextureVertices = {
          //X   Y   Z   U   V
            0f, 1f, 0f, 0f, 1f, // top right
            1f, 0f, 0f, 1f, 0f, // bottom right
            0f, 0f, 0f, 0f, 0f, // bottom left
            1f, 1f, 0f, 1f, 1f  // top left
        };

        private readonly uint[] Indices =
        {
            0, 1, 2,
            0, 3, 1
        };

        public GlObjects.Shader Shader { get; init; }

        private BufferObject<float> Vbo;
        private BufferObject<uint> Ebo;
        private VertexArrayObject<float, uint> Vao;

        private GL _gl;

        public SpriteRenderer(GL gl, GlObjects.Shader shader)
        {
            _gl = gl;
            Shader = shader;

            Ebo = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
            Vbo = new BufferObject<float>(gl, TextureVertices, BufferTargetARB.ArrayBuffer);
            Vao = new VertexArrayObject<float, uint>(gl, Vbo, Ebo);

            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

            Vao.Bind();
        }

        public SpriteRenderer(GL gl)
        {
            _gl = gl;
            Shader = new GlObjects.Shader(_gl);

            Ebo = new BufferObject<uint>(gl, Indices, BufferTargetARB.ElementArrayBuffer);
            Vbo = new BufferObject<float>(gl, TextureVertices, BufferTargetARB.ArrayBuffer);
            Vao = new VertexArrayObject<float, uint>(gl, Vbo, Ebo);

            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5, 0);
            Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5, 3);

            Vao.Bind();
        }

        public void SetOrthoProjection(float width, float height)
        {
            Shader.Use();
            Matrix4x4 projection = Matrix4x4.CreateOrthographic(width, height, 100f, -100f);
            Shader.SetUniform("uProjection", projection);
        }

        public void SetView(float width, float height, int xOffset = 0, int yOffset = 0)
        {
            Shader.Use();
            Vector3 CameraPosition = new Vector3((width / 2f) + xOffset, (-height / 2f) + yOffset, 100.0f);
            Vector3 CameraTarget = new Vector3((width / 2f) + xOffset, (-height / 2f) + yOffset, 0.0f);
            Vector3 CameraDirection = Vector3.Normalize(CameraPosition - CameraTarget);
            Vector3 CameraRight = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, CameraDirection));
            Vector3 CameraUp = Vector3.Cross(CameraDirection, CameraRight);
            var view = Matrix4x4.CreateLookAt(CameraPosition, CameraTarget, CameraUp);
            Shader.SetUniform("uView", view);
        }

        public unsafe void RenderSprite(Sprite sprite)
        {
            Shader.Use();

            Shader.SetUniform("uTexture0", 0);
            Shader.SetUniform("uModel", sprite.ModelMatrix);
            Shader.SetUniform("uTint", sprite.Tint);
            Shader.SetUniform("uAlpha", sprite.Alpha);

            sprite.Texture.Bind(TextureUnit.Texture0);

            _gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
        }

        public void Dispose()
        {
            Vbo.Dispose();
            Ebo.Dispose();
            Vao.Dispose();
            Shader.Dispose();
        }
    }
}