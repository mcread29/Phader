using System.Numerics;
using Silk.NET.OpenGL;

namespace Phader.Scene
{
    public abstract class Scene : IDisposable
    {
        public string Key { get; init; }
        public Textures.TextureManager textures;
        public Game Game { get; private set; }

        public List<GameObjects.Sprite> DisplayList { get; private set; }

        public Scene(string key)
        {
            Key = key;
        }

        public void Init(GL gl, Game game)
        {
            Game = game;
            textures = new Textures.TextureManager(gl, this);
            DisplayList = new List<GameObjects.Sprite>();
        }

        public abstract void Preload();
        public abstract void Create();
        public abstract void Update(double dt);
        public void Render(double dt)
        {
            foreach (GameObjects.Sprite sprite in DisplayList)
            {
                Game.SpriteRenderer.RenderSprite(sprite);
            }
        }
        public abstract void End();

        public void Dispose()
        {
            textures.Dispose();
        }
    }
}