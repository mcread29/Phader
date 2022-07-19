using System;
using System.Numerics;
using System.Globalization;
using Phader.Textures;

namespace Phader.GameObjects
{
    public class Sprite : GameObject
    {
        public Sprite(int x, int y, Texture texture) : base(x, y)
        {
            Texture = texture;
            SetSize((int)Texture.width, (int)Texture.height);

            Tint = new Vector3(1f, 1f, 1f);
            Alpha = 1f;
        }

        public Texture Texture { get; private set; }
        public Sprite SetTexture(Texture texture)
        {
            Texture = texture;
            return this;
        }

        public Vector3 Tint { get; private set; }
        public Sprite SetTint(Vector3 tint)
        {
            Tint = tint;
            return this;
        }

        public float Alpha { get; private set; }
        public Sprite SetAlpha(float alpha)
        {
            Alpha = alpha;
            return this;
        }
    }
}