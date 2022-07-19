using System.Collections.Generic;
using Silk.NET.OpenGL;

namespace Phader.Textures
{
    public class TextureManager : IDisposable
    {
        public Phader.Scene.Scene Scene { get; init; }

        private GL _gl;
        private Dictionary<string, Texture> textures;

        public TextureManager(GL gl, Scene.Scene scene)
        {
            _gl = gl;
            Scene = scene;
            textures = new Dictionary<string, Texture>();
        }

        public Textures.Texture AddTexture(string key, string path)
        {
            if (textures.ContainsKey(key) == false)
            {
                Textures.Texture tex = new Textures.Texture(_gl, path);
                textures.Add(key, tex);
                return tex;
            }
            Console.WriteLine($"{key} already in texture manager!");
            return null;
        }

        public Textures.Texture GetTexture(string key)
        {
            if (textures.ContainsKey(key))
            {
                return textures[key];
            }
            return null;
        }

        public bool RemoveTexture(string key)
        {
            if (textures.ContainsKey(key))
            {
                Textures.Texture texture = textures[key];
                texture.Dispose();
                return textures.Remove(key);
            }
            return false;
        }

        public bool RemoveTexture(Textures.Texture texture)
        {
            if (textures.ContainsValue(texture))
            {
                texture.Dispose();
                string key = textures.First(x => x.Value == texture).Key;
                return textures.Remove(key);
            }
            return false;
        }

        public void Dispose()
        {
            Dictionary<string, Textures.Texture>.KeyCollection keys = textures.Keys;
            foreach (string key in keys)
            {
                RemoveTexture(key);
            }
            textures.Clear();
        }
    }
}