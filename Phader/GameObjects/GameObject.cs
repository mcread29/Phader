using System.Numerics;

namespace Phader.GameObjects
{
    public class GameObject
    {
        public Transform Transform { get; private set; }
        private List<Components.IComponent> components;

        public Vector2 Origin { get; private set; } = new Vector2(0);
        public GameObject SetOrigin(Vector2 origin)
        {
            Origin = origin;
            return this;
        }
        public GameObject SetOrigin(float x, float y)
        {
            return this.SetOrigin(new Vector2(x, y));
        }

        public Vector2 Size { get; private set; } = new Vector2(1);
        public GameObject SetSize(Vector2 size)
        {
            Size = size;
            return this;
        }
        public GameObject SetSize(int x, int y)
        {
            return SetSize(new Vector2(x, y));
        }

        public int Width { get { return (int)Size.X; } }
        public GameObject SetWidth(int width)
        {
            return SetSize(new Vector2(width, Size.Y));
        }

        public int Height { get { return (int)Size.Y; } }
        public GameObject SetHeight(int height)
        {
            return SetSize(new Vector2(Size.X, height));
        }

        public GameObject()
        {
            Transform = new Transform(this);
            components = new List<Components.IComponent>();
        }

        public GameObject(int x, int y)
        {
            Transform = new Transform(this, x, y);
            components = new List<Components.IComponent>();
        }

        ~GameObject()
        {
            Transform = null;
            foreach (Phader.Components.IComponent component in components)
            {
                component.Destroy();
            }
            components.RemoveRange(0, components.Count);
            components = null;
        }

        public void AddChild(GameObject child)
        {
            Transform.AddChild(child.Transform);
        }

        public Matrix4x4 ModelMatrix
        {
            get
            {
                Matrix4x4 matrix = Matrix4x4.Identity
                    * Matrix4x4.CreateTranslation(new Vector3(0f - Origin.X, Origin.Y - 1f, 0f))
                    * Matrix4x4.CreateFromQuaternion(Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Transform.Rotation))
                    * Matrix4x4.CreateScale(new Vector3(Size.X * Transform.Scale.X, Size.Y * Transform.Scale.Y, 1f))
                    * Matrix4x4.CreateTranslation(new Vector3(Transform.Position.X, -Transform.Position.Y, 0f));
                if (Transform.Parent != null) matrix = matrix * Transform.Parent.GameObject.ModelMatrix;
                return matrix;
            }
        }
    }
}