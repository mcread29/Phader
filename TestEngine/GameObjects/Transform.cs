using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System.ComponentModel.Design.Serialization;
using System.Numerics;

namespace Phader.GameObjects
{
    public class Transform
    {
        //A transform abstraction.
        //For a transform we need to have a position a scale and a rotation,
        //depending on what application you are creating, the type for these may vary.

        //Here we have chosen a vec3 for position, float for scale and quaternion for rotation,
        //as that is the most normal to go with.
        //Another example could have been vec3, vec3, vec4, so the rotation is an axis angle instead of a quaternion

        public GameObject GameObject { get; init; }
        public Transform Parent { get; private set; } = null;
        public List<Transform> children { get; init; }
        public Transform(GameObject gameObject)
        {
            GameObject = gameObject;
            children = new List<Transform>();
        }

        public Transform(GameObject gameObject, int x, int y)
        {
            Position = new Vector2(x, y);
            GameObject = gameObject;
            children = new List<Transform>();
        }

        public bool AddChild(Transform child)
        {
            if (children.IndexOf(child) != -1)
            {
                return false;
            }
            else
            {
                children.Add(child);
                child.SetParent(this);
                return true;
            }
        }

        public bool RemoveChild(Transform child)
        {
            child.RemoveParent();
            return children.Remove(child);
        }

        public void SetParent(Transform parent)
        {
            Parent = parent;
        }

        public void RemoveParent()
        {
            Parent = null;
        }

        public Vector2 Position { get; private set; } = new Vector2(0);
        public Transform SetPosition(Vector2 position)
        {
            Position = position;
            return this;
        }
        public Transform SetPosition(float x, float y)
        {
            return SetPosition(new Vector2(x, y));
        }

        public Vector2 Scale { get; private set; } = Vector2.One;
        public Transform SetScale(Vector2 scale)
        {
            Scale = scale;
            return this;
        }
        public Transform SetScale(float x, float y)
        {
            return SetScale(new Vector2(x, y));
        }

        public float Rotation { get; private set; } = 0f;
        public Transform SetRotation(float radians)
        {
            Rotation = radians;
            return this;
        }

        public float Angle { get { return (180f / (float)Math.PI) * Rotation; } }
        public Transform SetAngle(float angle)
        {
            return SetRotation(angle / (180 / (float)Math.PI));
        }
    }
}